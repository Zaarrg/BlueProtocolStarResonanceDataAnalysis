#!/usr/bin/env python3
# -*- coding: utf-8 -*-

r"""
Extract IL2CPP global-metadata.dat embedded in GameAssembly.dll
with progress logs and multiple dump options.

It scans for the IL2CPP metadata header 0xFAB11BAF (AF 1B B1 FA) under:
  • plain
  • reversed
  • 1-byte XOR (uniform key)
  • 4-byte repeating XOR

Logging:
  - Prints file size, section map, and per-phase timings.
  - Shows how many hits per mode were found.

Outputs:
  - --out FILE       write ONE decoded candidate
  - --dump-all DIR   write ALL plausible decoded candidates
  - --dump-top N     write top N "suspects" even if version check fails
  - --no-version-check  treat any decoded MAGIC as plausible (ignore version)
  - --extend BYTES   extend carve beyond section end by this many bytes (default 0)

Usage (PowerShell):
  py Tools\DataTools\Scripts\bspr_metadata_extractor.py `
     --dll "G:\...\GameAssembly.dll" `
     --out ".\Data\global-metadata.dat" `
     --dump-all ".\Data\Guesses"

  # If nothing plausible is found:
  py ...\bspr_metadata_extractor.py `
     --dll "G:\...\GameAssembly.dll" `
     --dump-top 5 --extend 65536 --no-version-check --scan-all
"""

from __future__ import annotations
import argparse
import mmap
import struct
import time
from pathlib import Path
from typing import List, Optional, Tuple

MAGIC = b"\xAF\x1B\xB1\xFA"
MAGIC_REV = MAGIC[::-1]

# ---------- logging ----------
def log(msg: str) -> None:
    print(msg, flush=True)

def fmt_bytes(n: int) -> str:
    for unit in ("B","KB","MB","GB"):
        if n < 1024 or unit == "GB":
            return f"{n:.0f} {unit}"
        n /= 1024
    return f"{n:.0f} B"

# ---------- tiny PE section parser ----------
class Section:
    __slots__ = ("name", "start", "end")
    def __init__(self, name: str, start: int, end: int):
        self.name, self.start, self.end = name, start, end
    @property
    def size(self) -> int:
        return self.end - self.start

def read_pe_sections(path: Path) -> List[Section]:
    data = path.read_bytes()
    if data[:2] != b"MZ":
        raise RuntimeError("Not a PE file (missing 'MZ').")
    e_lfanew = struct.unpack_from("<I", data, 0x3C)[0]
    if data[e_lfanew:e_lfanew+4] != b"PE\x00\x00":
        raise RuntimeError("Invalid PE signature.")
    coff_off = e_lfanew + 4
    # COFF: Machine(2) NumSections(2) Time(4) PtrSym(4) NumSym(4) OptHdrSize(2) Chars(2)
    _, num_sections, *_rest, opt_hdr_size, _chars = struct.unpack_from("<HHIIIHH", data, coff_off)
    opt_off  = coff_off + 20
    sect_off = opt_off + opt_hdr_size

    secs: List[Section] = []
    for i in range(num_sections):
        off = sect_off + i * 40  # IMAGE_SECTION_HEADER
        raw_name = data[off:off+8]
        name = raw_name.split(b"\x00", 1)[0].decode(errors="ignore")
        size_raw = struct.unpack_from("<I", data, off + 16)[0]  # SizeOfRawData
        ptr_raw  = struct.unpack_from("<I", data, off + 20)[0]  # PointerToRawData
        if size_raw and ptr_raw:
            secs.append(Section(name, ptr_raw, ptr_raw + size_raw))
    if not secs:
        raise RuntimeError("No PE sections found.")
    return secs

def section_for_offset(sections: List[Section], file_off: int) -> Optional[Section]:
    for s in sections:
        if s.start <= file_off < s.end:
            return s
    return None

# ---------- header helpers ----------
def decode_1xor(b: bytes, k: int) -> bytes:
    k &= 0xFF
    return bytes(x ^ k for x in b)

def decode_4xor(b: bytes, k0: int, k1: int, k2: int, k3: int) -> bytes:
    ks = (k0 & 0xFF, k1 & 0xFF, k2 & 0xFF, k3 & 0xFF)
    out = bytearray(len(b))
    for i, x in enumerate(b):
        out[i] = x ^ ks[i & 3]
    return bytes(out)

def parse_header_version(buf8: bytes) -> Optional[int]:
    if len(buf8) < 8: return None
    if buf8[:4] != MAGIC: return None
    return int.from_bytes(buf8[4:8], "little", signed=False)

def plaus_version(v: int) -> bool:
    return 10 <= v <= 50  # typical IL2CPP metadata versions (e.g. 24–29+)

# ---------- candidate objects ----------
class Cand:
    __slots__ = ("off","mode","key","ver","sec")
    def __init__(self, off: int, mode: str, key: Tuple[int,...], ver: Optional[int], sec: Section):
        self.off, self.mode, self.key, self.ver, self.sec = off, mode, key, ver, sec

# ---------- scanning ----------
def _scan_ranges(sections: List[Section], mode: str, args) -> List[Tuple[int,int]]:
    """Return list of (start,end) ranges to scan (file offsets)."""
    if args.scan_all:
        # scan the whole file in one range; we'll derive length from mmap later
        return [(-1, -1)]
    wanted = set(s.strip().lower() for s in (args.sections or "").split(",") if s.strip())
    ranges: List[Tuple[int,int]] = []
    for s in sections:
        if not wanted or s.name.lower() in wanted:
            ranges.append((s.start, s.end))
    return ranges or [(-1, -1)]  # fallback to whole file

def find_hits(dll: Path, sections: List[Section], args) -> Tuple[List[Cand], List[Cand]]:
    """
    Returns (plausible, suspects). 'plausible' pass header + version checks,
    'suspects' have decoded magic but version outside expected (unless --no-version-check).
    """
    plausible: List[Cand] = []
    suspects:  List[Cand] = []

    with dll.open("rb") as f, mmap.mmap(f.fileno(), 0, access=mmap.ACCESS_READ) as mm:
        mv = memoryview(mm)
        try:
            file_len = len(mm)
            log(f"[i] Mapped {fmt_bytes(file_len)} from {dll.name}")

            ranges = _scan_ranges(sections, "scan", args)
            if ranges == [(-1, -1)]:
                ranges = [(0, file_len)]

            def add_candidate(i: int, mode: str, key: Tuple[int,...]):
                sec = section_for_offset(sections, i)
                if not sec:
                    return
                # read header8 (decode per mode) for validation
                raw8 = mv[i:i+8].tobytes()
                if mode == "xor1":
                    header8 = decode_1xor(raw8, key[0])
                elif mode == "xor4":
                    header8 = decode_4xor(raw8, *key)
                elif mode == "rev":
                    # "fix" only the magic; version at +4 is assumed in normal order.
                    header8 = MAGIC + raw8[4:8]
                else:
                    header8 = raw8

                if header8[:4] == MAGIC:
                    ver = parse_header_version(header8)
                    if args.no_version_check or (ver is not None and plaus_version(ver)):
                        plausible.append(Cand(i, mode, key, ver, sec))
                    else:
                        suspects.append(Cand(i, mode, key, ver, sec))

            # A) plain
            t0 = time.time()
            log("[*] Scanning for plain header...")
            total_hits = 0
            for (beg, end) in ranges:
                s = beg
                e = end
                at = s
                while True:
                    i = mm.find(MAGIC, at, e)
                    if i == -1: break
                    add_candidate(i, "plain", tuple())
                    total_hits += 1
                    at = i + 1
            log(f"    done in {time.time()-t0:.2f}s; hits={total_hits}")

            # B) reversed
            t0 = time.time()
            log("[*] Scanning for reversed header...")
            total_hits = 0
            for (beg, end) in ranges:
                at = beg
                while True:
                    i = mm.find(MAGIC_REV, at, end)
                    if i == -1: break
                    add_candidate(i, "rev", tuple())
                    total_hits += 1
                    at = i + 1
            log(f"    done in {time.time()-t0:.2f}s; hits={total_hits}")

            # C) 1-byte XOR
            t0 = time.time()
            log("[*] Scanning for 1-byte XOR (this can take a bit)...")
            step = 8
            processed = 0
            total_hits = 0
            for (beg, end) in ranges:
                i = beg
                while i <= end - 8:
                    k = mv[i] ^ MAGIC[0]
                    if k != 0 and (mv[i+1] ^ k) == MAGIC[1] and (mv[i+2] ^ k) == MAGIC[2] and (mv[i+3] ^ k) == MAGIC[3]:
                        add_candidate(i, "xor1", (int(k),))
                        total_hits += 1
                    i += 1
                    processed += 1
                # lightweight progress per range
                log(f"    scanned {fmt_bytes(end-beg)} in {time.time()-t0:.2f}s (running)")
            log(f"    done in {time.time()-t0:.2f}s; hits={total_hits}")

            # D) 4-byte repeating XOR
            t0 = time.time()
            log("[*] Scanning for 4-byte repeating XOR...")
            total_hits = 0
            for (beg, end) in ranges:
                i = beg
                while i <= end - 8:
                    k0 = mv[i]   ^ MAGIC[0]
                    k1 = mv[i+1] ^ MAGIC[1]
                    k2 = mv[i+2] ^ MAGIC[2]
                    k3 = mv[i+3] ^ MAGIC[3]
                    # quick precheck: decode first 8 bytes and validate version
                    hdr8 = decode_4xor(mv[i:i+8].tobytes(), k0, k1, k2, k3)
                    ver = parse_header_version(hdr8)
                    if hdr8[:4] == MAGIC and (args.no_version_check or (ver is not None and plaus_version(ver))):
                        add_candidate(i, "xor4", (int(k0), int(k1), int(k2), int(k3)))
                        total_hits += 1
                    i += 1
                log(f"    scanned {fmt_bytes(end-beg)} in {time.time()-t0:.2f}s (running)")
            log(f"    done in {time.time()-t0:.2f}s; hits={total_hits}")

        finally:
            # release memoryview before mmap closes to avoid BufferError
            del mv

    # de-dup identical offsets if multiple modes flagged same location
    seen = set()
    def uniq(cands: List[Cand]) -> List[Cand]:
        out: List[Cand] = []
        for c in cands:
            key = (c.off, c.sec.start, c.sec.end)
            if key in seen: continue
            seen.add(key)
            out.append(c)
        return out

    plausible = uniq(plausible)
    suspects  = uniq(suspects)
    return plausible, suspects

# ---------- carving/decoding ----------
def carve_decoded(dll: Path, c: Cand, extend: int) -> bytes:
    with dll.open("rb") as f:
        start = c.off
        end   = c.sec.end + max(0, int(extend))
        f.seek(start)
        raw = f.read(end - start)
    if c.mode == "xor1":
        return decode_1xor(raw, c.key[0])
    elif c.mode == "xor4":
        return decode_4xor(raw, *c.key)
    else:
        return raw

# ---------- CLI ----------
def main() -> int:
    ap = argparse.ArgumentParser(description="Extract global-metadata.dat embedded in GameAssembly.dll (with logs)")
    ap.add_argument("--dll", required=True, help="Path to GameAssembly.dll")
    ap.add_argument("--out", help="Write ONE decoded candidate here (e.g., ./Data/global-metadata.dat)")
    ap.add_argument("--dump-all", help="Write ALL plausible decoded candidates to this folder")
    ap.add_argument("--dump-top", type=int, default=0, help="Also dump this many suspect hits (decoded) even if version check fails")
    ap.add_argument("--no-version-check", action="store_true", help="Treat any decoded MAGIC as plausible (ignore version)")
    ap.add_argument("--extend", type=int, default=0, help="Extend carve beyond section end by this many bytes")
    ap.add_argument("--scan-all", action="store_true", help="Scan the entire file, not only chosen sections")
    ap.add_argument("--sections", default="il2cpp,.rdata,.mrdata,.data,.idata,.rsrc,.rsrc2,.tvm0,.pdata,.Sgxm1",
                    help="Comma-separated section names to scan (ignored if --scan-all).")
    ap.add_argument("--index", type=int, help="Choose the Nth plausible candidate (1-based) for --out")
    ap.add_argument("--list", action="store_true", help="List plausible candidates and exit (no write)")
    args = ap.parse_args()

    dll = Path(args.dll).resolve()
    if not dll.exists() or not dll.is_file():
        log(f"[err] Missing DLL: {dll}")
        return 1
    if dll.read_bytes()[:2] != b"MZ":
        log(f"[err] Not a PE/DLL (no 'MZ'): {dll}")
        return 1

    log(f"[i] DLL: {dll}")
    log(f"[i] Size: {fmt_bytes(dll.stat().st_size)}")
    sections = read_pe_sections(dll)
    log("[i] Sections:")
    for s in sections:
        log(f"    - {s.name:<8} @ 0x{s.start:08X} .. 0x{s.end:08X}  ({fmt_bytes(s.size)})")

    t0 = time.time()
    plausible, suspects = find_hits(dll, sections, args)
    log(f"[i] Scanning complete in {time.time()-t0:.2f}s")
    log(f"[i] Plausible candidates: {len(plausible)} | Suspects: {len(suspects)}")

    if plausible:
        log("[i] Plausible list:")
        for i, c in enumerate(plausible, 1):
            log(f"  [{i:>2}] off=0x{c.off:08X} ver={c.ver if c.ver is not None else '?':>2} sec={c.sec.name:<8} carve≈{fmt_bytes(c.sec.end - c.off)} mode={c.mode} key={c.key}")

    if args.list and not args.dump_all and not args.out and args.dump_top <= 0:
        return 0

    # Dump ALL plausible
    if args.dump_all and plausible:
        outdir = Path(args.dump_all).resolve()
        outdir.mkdir(parents=True, exist_ok=True)
        count = 0
        for i, c in enumerate(plausible, 1):
            data = carve_decoded(dll, c, args.extend)
            if data[:4] != MAGIC and not args.no_version_check:
                continue
            fn = f"global-metadata.plaus{i:02d}_off{c.off:08X}_ver{c.ver if c.ver is not None else 0}_{c.mode}.dat"
            (outdir / fn).write_bytes(data)
            count += 1
        log(f"[✓] Wrote {count} plausible candidate file(s) to: {outdir}")

    # Dump TOP suspects (decoded) if requested
    if args.dump_top > 0 and suspects:
        outdir = Path(args.dump_all or "./Data/Guesses").resolve()
        outdir.mkdir(parents=True, exist_ok=True)
        # pick shortest carve first from data-ish sections
        def sec_pref(name: str) -> int:
            return 0 if name.lower() in {"il2cpp",".rdata",".mrdata",".data",".rsrc",".rsrc2",".tvm0",".pdata"} else 1
        suspects_sorted = sorted(suspects, key=lambda c: (sec_pref(c.sec.name), c.sec.end - c.off, c.off))
        wrote = 0
        for c in suspects_sorted[:args.dump_top]:
            data = carve_decoded(dll, c, args.extend)
            fn = f"global-metadata.sus_off{c.off:08X}_ver{c.ver if c.ver is not None else 0}_{c.mode}.dat"
            (outdir / fn).write_bytes(data)
            wrote += 1
        log(f"[✓] Wrote {wrote} suspect file(s) to: {outdir}")

    # Write ONE to --out
    if args.out:
        if not plausible and not args.no_version_check:
            log("[err] No plausible candidates to write to --out. Try --no-version-check, --dump-top, or --scan-all/--extend.")
            return 1
        pick = None
        if plausible:
            if args.index:
                if not (1 <= args.index <= len(plausible)):
                    log(f"[err] --index must be between 1 and {len(plausible)}")
                    return 1
                pick = plausible[args.index - 1]
            else:
                pick = plausible[0]
        else:
            # if user forced no-version-check and none were "plausible", take the first suspect
            pick = suspects[0]

        data = carve_decoded(dll, pick, args.extend)
        out = Path(args.out).resolve()
        out.parent.mkdir(parents=True, exist_ok=True)
        out.write_bytes(data)
        log(f"[✓] Wrote {out}  (mode={pick.mode}, key={pick.key}, ver={pick.ver if pick.ver is not None else '?'})")

    return 0

if __name__ == "__main__":
    raise SystemExit(main())
