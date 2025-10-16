# bspr_runtime_dump.py
# Usage example:
#   py bspr_runtime_dump.py --proc BPSR_STEAM.exe --dll-out .\Data\runtime_GameAssembly.dll ^
#       --meta-out .\Data\global-metadata.dat --meta-sig "11223344AABBCCDD" --meta-header-offset 252 --magic-fix "AF1BB1FA00000018"

import argparse, ctypes, struct, sys, time
from pathlib import Path

# --- WinAPI constants/structs (minimal) ---
PROCESS_QUERY_INFORMATION = 0x0400
PROCESS_VM_READ  = 0x0010
PROCESS_VM_WRITE = 0x0020
PROCESS_VM_OPERATION = 0x0008

PAGE_EXECUTE_READWRITE = 0x40

LIST_MODULES_ALL = 0x03

kernel32 = ctypes.WinDLL("kernel32", use_last_error=True)
psapi    = ctypes.WinDLL("psapi", use_last_error=True)

# Types
DWORD  = ctypes.c_uint32
SIZE_T = ctypes.c_size_t
LPVOID = ctypes.c_void_p
HANDLE = ctypes.c_void_p
HMODULE = HANDLE
WCHAR  = ctypes.c_wchar

class MODULEINFO(ctypes.Structure):
    _fields_ = [("lpBaseOfDll", LPVOID),
                ("SizeOfImage", DWORD),
                ("EntryPoint", LPVOID)]

# Toolhelp for PID by name (no psutil needed)
TH32CS_SNAPPROCESS = 0x00000002
class PROCESSENTRY32(ctypes.Structure):
    _fields_ = [("dwSize", DWORD),
                ("cntUsage", DWORD),
                ("th32ProcessID", DWORD),
                ("th32DefaultHeapID", LPVOID),
                ("th32ModuleID", DWORD),
                ("cntThreads", DWORD),
                ("th32ParentProcessID", DWORD),
                ("pcPriClassBase", ctypes.c_long),
                ("dwFlags", DWORD),
                ("szExeFile", ctypes.c_wchar * 260)]

CreateToolhelp32Snapshot = kernel32.CreateToolhelp32Snapshot
Process32FirstW = kernel32.Process32FirstW
Process32NextW  = kernel32.Process32NextW

OpenProcess = kernel32.OpenProcess
CloseHandle = kernel32.CloseHandle
ReadProcessMemory = kernel32.ReadProcessMemory
VirtualProtectEx = kernel32.VirtualProtectEx

EnumProcessModulesEx = psapi.EnumProcessModulesEx
GetModuleFileNameExW = psapi.GetModuleFileNameExW
GetModuleInformation = psapi.GetModuleInformation

EnumProcessModulesEx.argtypes = [HANDLE, ctypes.POINTER(HMODULE), DWORD, ctypes.POINTER(DWORD), DWORD]
GetModuleFileNameExW.argtypes  = [HANDLE, HMODULE, ctypes.c_wchar_p, DWORD]
GetModuleInformation.argtypes  = [HANDLE, HMODULE, ctypes.POINTER(MODULEINFO), DWORD]

OpenProcess.argtypes = [DWORD, ctypes.c_bool, DWORD]
ReadProcessMemory.argtypes = [HANDLE, LPVOID, LPVOID, SIZE_T, ctypes.POINTER(SIZE_T)]
VirtualProtectEx.argtypes = [HANDLE, LPVOID, SIZE_T, DWORD, ctypes.POINTER(DWORD)]

def die(msg):
    print(f"[err] {msg}")
    sys.exit(1)

def get_pid_by_name(name: str) -> int | None:
    snap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0)
    if snap == HANDLE(0xffffffffffffffff):  # INVALID_HANDLE_VALUE
        return None
    try:
        pe = PROCESSENTRY32()
        pe.dwSize = ctypes.sizeof(PROCESSENTRY32)
        if not Process32FirstW(snap, ctypes.byref(pe)):
            return None
        while True:
            if pe.szExeFile.lower() == name.lower():
                return int(pe.th32ProcessID)
            if not Process32NextW(snap, ctypes.byref(pe)):
                break
    finally:
        CloseHandle(snap)
    return None

def open_proc(pid: int) -> HANDLE:
    h = OpenProcess(PROCESS_QUERY_INFORMATION|PROCESS_VM_READ|PROCESS_VM_WRITE|PROCESS_VM_OPERATION, False, pid)
    if not h:
        die(f"OpenProcess failed for PID {pid} (need admin?)")
    return h

def find_module(hproc: HANDLE, needle: str) -> tuple[int,int,str] | None:
    # enumerate modules
    needed = DWORD(0)
    EnumProcessModulesEx(hproc, None, 0, ctypes.byref(needed), LIST_MODULES_ALL)
    nmods = needed.value // ctypes.sizeof(HMODULE)
    arr = (HMODULE * nmods)()
    if not EnumProcessModulesEx(hproc, arr, needed, ctypes.byref(needed), LIST_MODULES_ALL):
        return None
    buf = ctypes.create_unicode_buffer(1024)
    for i in range(nmods):
        GetModuleFileNameExW(hproc, arr[i], buf, 1024)
        path = buf.value
        if path.lower().endswith(needle.lower()):
            mi = MODULEINFO()
            if not GetModuleInformation(hproc, arr[i], ctypes.byref(mi), ctypes.sizeof(mi)):
                continue
            base = ctypes.addressof(mi.lpBaseOfDll.contents) if isinstance(mi.lpBaseOfDll, ctypes.c_void_p) else mi.lpBaseOfDll
            base = int(mi.lpBaseOfDll)
            size = int(mi.SizeOfImage)
            return (base, size, path)
    return None

def prot_rwxe(hproc: HANDLE, addr: int, size: int):
    old = DWORD(0)
    if not VirtualProtectEx(hproc, LPVOID(addr), size, PAGE_EXECUTE_READWRITE, ctypes.byref(old)):
        # not fatal, try continue
        return None
    return old.value

def prot_restore(hproc: HANDLE, addr: int, size: int, old: int|None):
    if old is None: return
    _ = DWORD(0)
    VirtualProtectEx(hproc, LPVOID(addr), size, old, ctypes.byref(_))

def rpm_all(hproc: HANDLE, base: int, size: int) -> bytes:
    buf = (ctypes.c_char * size)()
    read = SIZE_T(0)
    ok = ReadProcessMemory(hproc, LPVOID(base), buf, size, ctypes.byref(read))
    if not ok or read.value == 0:
        die("ReadProcessMemory failed.")
    return bytes(buf[:read.value])

# ---- PE rebuild (headers + sections mapped back to raw layout) ----
def rebuild_pe_from_image(img: bytes) -> bytes:
    def u32(off): return struct.unpack_from("<I", img, off)[0]
    if img[:2] != b"MZ": die("GameAssembly in memory is not a PE (no MZ)")
    e_lfanew = u32(0x3C)
    if img[e_lfanew:e_lfanew+4] != b"PE\x00\x00": die("Invalid PE signature")
    coff = e_lfanew + 4
    num_sections = struct.unpack_from("<H", img, coff+2)[0]
    opt_size     = struct.unpack_from("<H", img, coff+16)[0]
    opt_off = coff + 20
    sect_off = opt_off + opt_size
    size_of_headers = struct.unpack_from("<I", img, opt_off+60)[0]  # OptionalHeader.SizeOfHeaders (PE32+ alike)
    # compute output size
    out_size = size_of_headers
    sections = []
    for i in range(num_sections):
        off = sect_off + i*40
        name = img[off:off+8].split(b"\x00",1)[0].decode(errors="ignore")
        vsize = struct.unpack_from("<I", img, off+8)[0]
        vaddr = struct.unpack_from("<I", img, off+12)[0]
        rsize = struct.unpack_from("<I", img, off+16)[0]
        rptr  = struct.unpack_from("<I", img, off+20)[0]
        sections.append((name, vaddr, rptr, rsize, vsize))
        out_size = max(out_size, rptr + rsize)
    out = bytearray(out_size)
    out[:size_of_headers] = img[:size_of_headers]
    for name, vaddr, rptr, rsize, vsize in sections:
        if rsize == 0: continue
        out[rptr:rptr+rsize] = img[vaddr:vaddr+rsize]
    return bytes(out)

# ---- Metadata search helpers (plain/xor/xor4) ----
MAGIC = b"\xAF\x1B\xB1\xFA"
def decode_1xor(b: bytes, k: int) -> bytes: k &= 0xFF; return bytes(x ^ k for x in b)
def decode_4xor(b: bytes, k0,k1,k2,k3): ks=(k0&0xFF,k1&0xFF,k2&0xFF,k3&0xFF); return bytes(b ^ ks[i&3] for i,b in enumerate(b))
def plaus_ver(v:int)->bool: return 10 <= v <= 50
def parse_ver(hdr8:bytes):
    if len(hdr8)<8 or hdr8[:4]!=MAGIC: return None
    return int.from_bytes(hdr8[4:8], "little", signed=False)

def find_metadata_auto(img: bytes, sections:list[tuple[str,int,int,int,int]]):
    hits=[]
    n=len(img)
    # plain
    i=0
    while True:
        j=img.find(MAGIC,i)
        if j==-1: break
        hits.append(("plain", j, ()))
        i=j+1
    # 1-byte xor
    for i in range(0, n-8):
        k = img[i] ^ MAGIC[0]
        if k!=0 and (img[i+1]^k)==MAGIC[1] and (img[i+2]^k)==MAGIC[2] and (img[i+3]^k)==MAGIC[3]:
            hdr8 = decode_1xor(img[i:i+8], k)
            ver = parse_ver(hdr8)
            if ver is not None and plaus_ver(ver):
                hits.append(("xor1", i, (k,)))
    # 4-byte xor
    for i in range(0, n-8):
        k0 = img[i] ^ MAGIC[0]; k1= img[i+1]^MAGIC[1]; k2= img[i+2]^MAGIC[2]; k3= img[i+3]^MAGIC[3]
        hdr8 = decode_4xor(img[i:i+8], k0,k1,k2,k3)
        ver = parse_ver(hdr8)
        if ver is not None and plaus_ver(ver):
            hits.append(("xor4", i, (k0,k1,k2,k3)))
    # choose best: in data-ish section, shortest carve
    def sect_for(off):
        for name,vaddr,rptr,rsize,_ in sections:
            if vaddr <= off < vaddr + rsize: return (name, vaddr, rptr, rsize)
        return None
    cands=[]
    for m,off,key in hits:
        s=sect_for(off)
        if s:
            name,vaddr,rptr,rsize=s
            cands.append((m,off,key,name,vaddr,rptr,rsize))
    if not cands: return None
    pref = {".rdata",".data",".mrdata","il2cpp",".rsrc",".rsrc2",".tvm0",".pdata"}
    cands.sort(key=lambda t: (0 if t[3].lower() in pref else 1, (t[5]+t[6])-(t[5]+(t[1]-t[4])), t[1]))
    return cands[0]  # (mode, off, key, sectName, vaddr, rptr, rsize)

def main():
    ap = argparse.ArgumentParser(description="Dump GameAssembly + extract global-metadata.dat from a running IL2CPP game (Windows)")
    g = ap.add_mutually_exclusive_group(required=True)
    g.add_argument("--proc", help="Process name (e.g. BPSR_STEAM.exe)")
    g.add_argument("--pid", type=int, help="PID")
    ap.add_argument("--dll-out", required=True, help="Output path for fixed GameAssembly dump")
    ap.add_argument("--meta-out", required=True, help="Output path for global-metadata.dat")
    # metadata options matching BepInEx PR
    ap.add_argument("--meta-sig", help="Hex signature to locate metadata (MetadataSignatureToScan)")
    ap.add_argument("--meta-header-offset", type=int, default=252, help="Bytes to subtract from signature hit to get metadata start (default 252)")
    ap.add_argument("--magic-fix", help="8-byte hex to overwrite first 8 bytes of metadata (MagicToFix)")
    ap.add_argument("--extend", type=int, default=0, help="Extend carve beyond section end by N bytes")
    args = ap.parse_args()

    pid = args.pid
    if not pid:
        pid = get_pid_by_name(args.proc)
        if not pid: die(f"Process not found: {args.proc}")
    print(f"[i] Target PID: {pid}")

    hproc = open_proc(pid)
    try:
        info = find_module(hproc, "\\GameAssembly.dll")
        if not info:
            die("GameAssembly.dll not found in target process modules.")
        base, size, path = info
        print(f"[i] GameAssembly base=0x{base:016X} size={size:,}  ({path})")

        print("[*] Relaxing page protections (RWX) for read…")
        old = prot_rwxe(hproc, base, size)

        t0=time.time()
        img = rpm_all(hproc, base, size)
        print(f"[i] Read {len(img):,} bytes in {time.time()-t0:.2f}s")
    finally:
        try: prot_restore(hproc, base, size, old)
        except Exception: pass
        CloseHandle(hproc)

    # Parse section headers from the in-memory image for carving & PE rebuild
    def parse_sections(img: bytes):
        if img[:2]!=b"MZ": die("No MZ header in image")
        e_lfanew = struct.unpack_from("<I", img, 0x3C)[0]
        if img[e_lfanew:e_lfanew+4]!=b"PE\x00\x00": die("Invalid PE sig")
        coff = e_lfanew+4
        num = struct.unpack_from("<H", img, coff+2)[0]
        optsz = struct.unpack_from("<H", img, coff+16)[0]
        opt  = coff+20
        sect = opt + optsz
        secs=[]
        for i in range(num):
            off=sect + i*40
            name=img[off:off+8].split(b"\x00",1)[0].decode(errors="ignore")
            vsize=struct.unpack_from("<I", img, off+8)[0]
            vaddr=struct.unpack_from("<I", img, off+12)[0]
            rsize=struct.unpack_from("<I", img, off+16)[0]
            rptr =struct.unpack_from("<I", img, off+20)[0]
            secs.append((name,vaddr,rptr,rsize,vsize))
        return secs
    sections = parse_sections(img)

    # Rebuild a file-layout PE on disk
    dll_out = Path(args.dll_out); dll_out.parent.mkdir(parents=True, exist_ok=True)
    fixed = rebuild_pe_from_image(img)
    dll_out.write_bytes(fixed)
    print(f"[✓] Wrote fixed GameAssembly: {dll_out}  (size {len(fixed):,} bytes)")

    # Extract metadata
    meta_out = Path(args.meta_out); meta_out.parent.mkdir(parents=True, exist_ok=True)
    start_off = None
    mode = None
    key  = None
    end_off = None

    if args.meta_sig:
        sig = bytes.fromhex(args.meta_sig)
        pos = img.find(sig)
        if pos == -1:
            die("Signature not found in image; try without --meta-sig to use auto-scan.")
        start_off = pos - args.meta_header_offset
        if start_off < 0: die("Computed metadata start before image start; adjust --meta-header-offset")
        # find section containing start_off to define end
        for name,vaddr,rptr,rsize,_ in sections:
            if vaddr <= start_off < vaddr + rsize:
                end_off = vaddr + rsize + max(0,args.extend)
                mode="sig"
                key=()
                break
        if end_off is None: die("Start not inside any PE section; adjust parameters.")
        blob = bytearray(img[start_off:end_off])
        if args.magic_fix:
            fix = bytes.fromhex(args.magic_fix)
            if len(fix)!=8: die("--magic-fix must be 8 bytes (hex)")
            blob[:8] = fix
        meta_out.write_bytes(blob)
        print(f"[✓] Wrote metadata (sig mode) → {meta_out}  bytes={len(blob):,}")
        return

    # Auto-scan (plain/xor/xor4)
    picked = find_metadata_auto(img, sections)
    if not picked:
        die("Auto-scan found no plausible metadata header. It may be compressed; consider a signature/offset.")
    mode, off, key, sname, vaddr, rptr, rsize = picked
    end_off = vaddr + rsize + max(0,args.extend)
    raw = img[off:end_off]
    if mode == "xor1":
        blob = bytearray(decode_1xor(raw, key[0]))
    elif mode == "xor4":
        blob = bytearray(decode_4xor(raw, *key))
    else:
        blob = bytearray(raw)

    if args.magic_fix:
        fix = bytes.fromhex(args.magic_fix)
        if len(fix)!=8: die("--magic-fix must be 8 bytes (hex)")
        blob[:8] = fix

    meta_out.write_bytes(blob)
    print(f"[✓] Wrote metadata (auto mode={mode}, key={key}) → {meta_out}  bytes={len(blob):,}")

if __name__ == "__main__":
    main()
