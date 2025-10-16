#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
BPSR data extraction runner (override-friendly + timestamped runs)

Changes:
- Output is versioned by timestamp: Data/RawGameData/<YYYYMMDD_HHMMSS>/
- After StarResonanceTool runs, move stray 'Excels' and 'proto' folders
  (created next to this .py) into the timestamped output dir.

Examples:
  python bspr_data_extraction_pipline.py
  python bspr_data_extraction_pipline.py --game-dir "G:\\SteamLibrary\\steamapps\\common\\Blue Protocol Star Resonance\\bpsr"
  python bspr_data_extraction_pipline.py --dll "G:\\...\\GameAssembly.dll" --metadata "G:\\...\\global-metadata.dat" --pkg "G:\\...\\meta.pkg"
  python bspr_data_extraction_pipline.py --il2cppdumper ".\\Tools\\DataTools\\Il2CppDumper\\Il2CppDumper.exe" --star-tool ".\\Tools\\DataTools\\StarResonanceTool\\Build\\StarResonanceTool.exe"
"""

from __future__ import annotations
import argparse
import os
import re
import shutil
import subprocess
import sys
import time
from pathlib import Path

# ---------------------------- Config ----------------------------

GAME_FOLDER_NAME = "Blue Protocol Star Resonance"
GAME_SUBDIR = "bpsr"

EXECUTABLE_NAME = "BPSR_STEAM.exe"
DLL_NAME = "GameAssembly.dll"
GLOBAL_METADATA_REL = Path("BPSR_STEAM_Data/il2cpp_data/Metadata/global-metadata.dat")
META_PKG_REL = Path("BPSR_STEAM_Data/StreamingAssets/container/meta.pkg")

# Project layout (this script lives in Tools/DataTools/Scripts)
SCRIPT_DIR   = Path(__file__).resolve().parent
PROJECT_ROOT = SCRIPT_DIR.parents[2]  # .../BlueProtocolStarResonanceDataAnalysis
DATA_DIR     = PROJECT_ROOT / "Data"
DUMMY_OUT_DIR= DATA_DIR / "GameDummyDll"
RAW_OUT_DIR  = DATA_DIR / "RawGameData"

TOOLS_DIR = PROJECT_ROOT / "Tools" / "DataTools"
DEFAULT_IL2CPPDUMPER_EXE = TOOLS_DIR / "Il2CppDumper" / "Il2CppDumper.exe"
DEFAULT_STAR_TOOL_EXE    = TOOLS_DIR / "StarResonanceTool" / "Build" / "StarResonanceTool.exe"

# ------------------------- Steam discovery ----------------------

def _winreg_steam_path() -> Path | None:
    try:
        import winreg  # type: ignore
    except Exception:
        return None

    candidates = [
        (winreg.HKEY_CURRENT_USER, r"Software\Valve\Steam", "SteamPath"),
        (winreg.HKEY_LOCAL_MACHINE, r"SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath"),
        (winreg.HKEY_LOCAL_MACHINE, r"SOFTWARE\Valve\Steam", "InstallPath"),
    ]
    for root, subkey, value_name in candidates:
        try:
            with winreg.OpenKey(root, subkey) as k:
                val, _ = winreg.QueryValueEx(k, value_name)
                p = Path(val)
                if p.exists():
                    return p
        except FileNotFoundError:
            continue
        except Exception:
            continue
    return None

def _parse_libraryfolders(vdf_path: Path) -> list[Path]:
    libs: list[Path] = []
    if not vdf_path.exists():
        return libs

    text = vdf_path.read_text(encoding="utf-8", errors="ignore")

    for m in re.finditer(r'"\s*path\s*"\s*"([^"]+)"', text, re.IGNORECASE):
        p = Path(m.group(1).replace('\\\\', '\\'))
        if p.exists():
            libs.append(p)

    for m in re.finditer(r'"\s*\d+\s*"\s*"([^"]+)"', text):
        p = Path(m.group(1).replace('\\\\', '\\'))
        if p.exists():
            libs.append(p)

    steam_root = vdf_path.parent.parent  # .../Steam/steamapps
    if steam_root.parent.exists():
        libs.append(steam_root.parent)

    # Dedup while preserving order
    seen = set()
    uniq: list[Path] = []
    for p in libs:
        if p not in seen:
            uniq.append(p); seen.add(p)
    return uniq

def discover_steam_roots() -> list[Path]:
    roots: list[Path] = []

    reg_root = _winreg_steam_path()
    if reg_root:
        roots.append(reg_root)
        vdf = reg_root / "steamapps" / "libraryfolders.vdf"
        roots.extend(_parse_libraryfolders(vdf))

    for guess in [
        Path(r"C:\Program Files (x86)\Steam"),
        Path(r"C:\Program Files\Steam"),
    ]:
        if guess.exists():
            roots.append(guess)

    seen = set()
    uniq: list[Path] = []
    for p in roots:
        if p not in seen:
            uniq.append(p); seen.add(p)
    return uniq

def find_game_dir(explicit: str | None) -> Path:
    env_override = os.getenv("BPSR_DIR")
    if explicit:
        p = Path(explicit)
        if not p.exists():
            raise FileNotFoundError(f"--game-dir does not exist: {p}")
        return p

    if env_override:
        p = Path(env_override)
        if p.exists():
            return p

    for steam_root in discover_steam_roots():
        candidate = steam_root / "steamapps" / "common" / GAME_FOLDER_NAME
        if candidate.exists():
            bpsr_dir = candidate / GAME_SUBDIR
            return bpsr_dir if bpsr_dir.exists() else candidate

    for letter in range(ord("D"), ord("Z") + 1):
        root = Path(f"{chr(letter)}:\\SteamLibrary\\steamapps\\common\\{GAME_FOLDER_NAME}")
        if root.exists():
            bpsr_dir = root / GAME_SUBDIR
            return bpsr_dir if bpsr_dir.exists() else root

    raise FileNotFoundError(
        f"Could not locate '{GAME_FOLDER_NAME}' automatically. "
        f"Pass --game-dir or set BPSR_DIR to the game's '{GAME_SUBDIR}' folder."
    )

# ---------------------------- Helpers ---------------------------

def ensure_exists(path: Path, what: str):
    if not path.exists():
        raise FileNotFoundError(f"Missing {what}: {path}")

def is_pe(dll: Path) -> bool:
    try:
        with dll.open("rb") as f:
            return f.read(2) == b"MZ"
    except Exception:
        return False

def read_meta_header(meta: Path) -> tuple[bool, int | None]:
    try:
        with meta.open("rb") as f:
            head = f.read(8)
        if len(head) < 8 or head[:4] != b"\xAF\x1B\xB1\xFA":
            return (False, None)
        ver = int.from_bytes(head[4:8], "little", signed=False)
        return (True, ver)
    except Exception:
        return (False, None)

def run(cmd: list[str], dry_run: bool = False, cwd: Path | None = None):
    print(">", " ".join(f'"{c}"' if " " in c else c for c in cmd), f"(cwd={cwd})" if cwd else "")
    if dry_run:
        return 0
    try:
        res = subprocess.run(cmd, cwd=str(cwd) if cwd else None, check=True)
        return res.returncode
    except subprocess.CalledProcessError as e:
        raise RuntimeError(f"Command failed ({e.returncode}): {' '.join(cmd)}") from e

def move_dir_contents(src: Path, dst: Path):
    """
    Move directory 'src' into 'dst/src.name'. If the target exists, merge contents.
    """
    if not src.exists() or not src.is_dir():
        return False
    target = dst / src.name
    target.mkdir(parents=True, exist_ok=True)
    for root, dirs, files in os.walk(src):
        rel = Path(root).relative_to(src)
        troot = target / rel
        troot.mkdir(parents=True, exist_ok=True)
        for fn in files:
            s = Path(root) / fn
            d = troot / fn
            if d.exists():
                d.unlink()
            shutil.move(str(s), str(d))
    # remove empty folders from src
    shutil.rmtree(src, ignore_errors=True)
    return True

# --------------------------- Pipeline ---------------------------

def main():
    ap = argparse.ArgumentParser(description="BPSR data extraction pipeline")
    ap.add_argument("--game-dir", help="Path to the game's 'bpsr' directory (overrides auto-discovery)")

    # input overrides
    ap.add_argument("--dll", help="Override path to GameAssembly.dll")
    ap.add_argument("--metadata", help="Override path to global-metadata.dat")
    ap.add_argument("--pkg", help="Override path to meta.pkg")

    # tool overrides
    ap.add_argument("--il2cppdumper", help="Path to Il2CppDumper.exe (override default)")
    ap.add_argument("--star-tool", help="Path to StarResonanceTool.exe (override default)")

    # run folder options
    ap.add_argument("--run-id", help="Custom subfolder name under RawGameData (default: YYYYMMDD_HHMMSS)")
    ap.add_argument("--force", action="store_true", help="Clean DUMMY_OUT_DIR before running")
    ap.add_argument("--dry-run", action="store_true", help="Show what would be done without executing")
    args = ap.parse_args()

    # Resolve game dir
    game_dir = find_game_dir(args.game_dir)
    print(f"[i] Game base dir: {game_dir}")

    # Resolve inputs (allow overrides)
    exe_path  = game_dir / EXECUTABLE_NAME
    dll_path  = Path(args.dll).resolve() if args.dll else (game_dir / DLL_NAME)
    meta_path = Path(args.pkg).resolve() if args.pkg else (game_dir / META_PKG_REL)
    gmeta_path= Path(args.metadata).resolve() if args.metadata else (game_dir / GLOBAL_METADATA_REL)

    # Sanity checks
    ensure_exists(exe_path, "executable")
    ensure_exists(dll_path, "GameAssembly.dll")
    ensure_exists(meta_path, "meta.pkg")
    ensure_exists(gmeta_path, "global-metadata.dat")

    if not is_pe(dll_path):
        raise RuntimeError(f"{dll_path} is not a PE/DLL (missing 'MZ').")

    # Metadata preflight
    if gmeta_path.stat().st_size == 0:
        raise RuntimeError(f"{gmeta_path} is 0 bytes. Provide a valid file via --metadata (e.g., from your extractor).")

    ok_magic, ver = read_meta_header(gmeta_path)
    if not ok_magic:
        print(f"[warn] {gmeta_path.name} does not start with IL2CPP metadata magic (FAB11BAF). "
              f"Il2CppDumper may fail. If this is a carved/decoded blob, you can ignore this warning.")
    else:
        print(f"[i] Metadata header OK. Version={ver}")

    # Resolve tools (allow overrides)
    il2cppdumper_exe = Path(args.il2cppdumper).resolve() if args.il2cppdumper else DEFAULT_IL2CPPDUMPER_EXE
    star_tool_exe    = Path(args.star_tool).resolve() if args.star_tool else DEFAULT_STAR_TOOL_EXE

    ensure_exists(il2cppdumper_exe, "Il2CppDumper.exe")
    ensure_exists(star_tool_exe, "StarResonanceTool.exe")

    # Prepare outputs
    if args.force:
        if DUMMY_OUT_DIR.exists():
            print(f"[i] Removing {DUMMY_OUT_DIR}")
            if not args.dry_run:
                shutil.rmtree(DUMMY_OUT_DIR)

    # timestamped run folder under RawGameData
    run_id = args.run_id or time.strftime("%Y%m%d_%H%M%S")  # local time
    OUT_RUN_DIR = RAW_OUT_DIR / run_id

    if not args.dry_run:
        DUMMY_OUT_DIR.mkdir(parents=True, exist_ok=True)
        OUT_RUN_DIR.mkdir(parents=True, exist_ok=True)

    print(f"[i] Run output dir: {OUT_RUN_DIR}")

    # 1) Il2CppDumper (DLL FIRST, then metadata, then output)
    print("\n[1/3] Running Il2CppDumper …")
    run([
        str(il2cppdumper_exe),
        str(dll_path),
        str(gmeta_path),
        str(DUMMY_OUT_DIR),
    ], dry_run=args.dry_run)

    dummy_dll_dir = DUMMY_OUT_DIR / "DummyDll"
    ensure_exists(dummy_dll_dir, "Il2CppDumper output 'DummyDll'")

    # 2) StarResonanceTool → versioned output
    print("\n[2/3] Running StarResonanceTool …")
    star_cmd = [
        str(star_tool_exe),
        "--all",
        "--assetbundles",
        "--pkg", str(meta_path),
        "--dll", str(dummy_dll_dir),
        "--output", str(OUT_RUN_DIR),
    ]
    # ensure the tool's "stray" folders land in a known place (beside this script)
    run(star_cmd, dry_run=args.dry_run, cwd=SCRIPT_DIR)

    # 3) Collect stray 'Excels' and 'proto' folders created beside the .py
    print("\n[3/3] Collecting stray folders (Excels, proto) …")
    moved_any = False
    for stray_name in ("Excels", "proto"):
        stray = SCRIPT_DIR / stray_name
        if stray.exists() and stray.is_dir():
            ok = move_dir_contents(stray, OUT_RUN_DIR)
            if ok:
                print(f"    Moved '{stray_name}/' → {OUT_RUN_DIR / stray_name}")
                moved_any = True
        # Some builds may drop lowercase
        stray_lower = SCRIPT_DIR / stray_name.lower()
        if stray_lower.exists() and stray_lower.is_dir():
            ok = move_dir_contents(stray_lower, OUT_RUN_DIR)
            if ok:
                print(f"    Moved '{stray_name.lower()}/' → {OUT_RUN_DIR / stray_name.lower()}")
                moved_any = True
    if not moved_any:
        print("    No stray folders found.")

    print("\n✅ Done.")
    print(f"   Dummy DLLs: {dummy_dll_dir}")
    print(f"   Extracted data: {OUT_RUN_DIR}")

if __name__ == "__main__":
    try:
        main()
    except Exception as e:
        print(f"\nERROR: {e}", file=sys.stderr)
        sys.exit(1)
