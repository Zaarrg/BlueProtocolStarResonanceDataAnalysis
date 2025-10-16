#!/usr/bin/env python3
# -*- coding: utf-8 -*-

r"""
Monster table generator

Reads (choose one):
  1) --input "<...>\MonsterTable.json"
  2) --from-latest  → auto-pick newest: Data/RawGameData/<YYYYMMDD_HHMMSS>/Excels/MonsterTable.json
  3) default fallback (non-timestamped): Data/RawGameData/Excels/MonsterTable.json

Writes:
  Data/ProcessedGameData/monster_name_mapping.json   (default)

Mapping:
  "<Id>": "<Name>"  (falls back to NameDesign when Name is empty)

Usage:
  python monster_table_generator.py --from-latest --sort
  python monster_table_generator.py --input "G:\...\RawGameData\20251016_105706\Excels\MonsterTable.json" --output "G:\...\monster_name_mapping.json" --include-empty --sort
"""

from __future__ import annotations
import argparse
import json
import re
from pathlib import Path
from typing import Any, Dict, Optional

# ---------- locate repo root (robust to folder moves) ----------
def find_project_root(start: Path) -> Path:
    cur = start.resolve()
    for _ in range(8):
        if (cur / "Data").exists() and (cur / "Tools").exists():
            return cur
        if cur.parent == cur:
            break
        cur = cur.parent
    # fallback: assume script at Tools/GameTools/Monsters/
    return start.resolve().parents[3]

SCRIPT_DIR = Path(__file__).resolve().parent
PROJECT_ROOT = find_project_root(SCRIPT_DIR)

RAW_ROOT_DEFAULT = PROJECT_ROOT / "Data" / "RawGameData"
DEFAULT_INPUT_FALLBACK = RAW_ROOT_DEFAULT / "Excels" / "MonsterTable.json"
DEFAULT_OUTDIR = PROJECT_ROOT / "Data" / "ProcessedGameData"
DEFAULT_OUTPUT = DEFAULT_OUTDIR / "monster_name_mapping.json"

TS_DIR_RE = re.compile(r"^\d{8}_\d{6}$")  # e.g., 20251016_105706

def find_latest_monster_table(raw_root: Path) -> Optional[Path]:
    """
    Returns the MonsterTable.json under the newest timestamped run dir, if any.
    'Newest' is primarily by timestamped directory name, otherwise by mtime.
    """
    if not raw_root.exists():
        return None

    ts_dirs = [d for d in raw_root.iterdir() if d.is_dir() and TS_DIR_RE.match(d.name)]
    candidate: Optional[Path] = None

    if ts_dirs:
        # Pick max by directory name (YYYYMMDD_HHMMSS sorts lexicographically)
        newest = max(ts_dirs, key=lambda p: p.name)
        mt = newest / "Excels" / "MonsterTable.json"
        if mt.exists():
            return mt

    # Fallback: pick most recently modified subdir that contains the file
    latest_with_file: Optional[tuple[float, Path]] = None
    for d in raw_root.iterdir():
        if not d.is_dir():
            continue
        mt = d / "Excels" / "MonsterTable.json"
        if mt.exists():
            t = d.stat().st_mtime
            if latest_with_file is None or t > latest_with_file[0]:
                latest_with_file = (t, mt)

    if latest_with_file:
        return latest_with_file[1]

    return None

def load_json(path: Path) -> Any:
    # utf-8-sig handles BOM if present
    with path.open("r", encoding="utf-8-sig") as f:
        return json.load(f)

def extract_mapping(data: Any, include_empty: bool = False) -> Dict[str, str]:
    """
    Accepts either:
      - dict keyed by string IDs -> entry objects
      - list of entry objects with fields Id, Name, NameDesign
    Returns { "id": "name", ... }
    """
    out: Dict[str, str] = {}

    def choose_name(entry: dict) -> str:
        name = (entry.get("Name") or "").strip()
        if not name:
            name = (entry.get("NameDesign") or "").strip()
        return name

    if isinstance(data, dict):
        for k, entry in data.items():
            if isinstance(entry, dict):
                name = choose_name(entry)
                if name or include_empty:
                    out[str(k)] = name
    elif isinstance(data, list):
        for entry in data:
            if isinstance(entry, dict) and "Id" in entry:
                id_str = str(entry["Id"])
                name = choose_name(entry)
                if name or include_empty:
                    out[id_str] = name
    else:
        raise ValueError("Unexpected JSON structure: expected dict or list at root.")

    return out

def main() -> int:
    ap = argparse.ArgumentParser(description="Generate monster ID→Name mapping from MonsterTable.json")
    ap.add_argument("--input", help="Explicit path to MonsterTable.json")
    ap.add_argument("--from-latest", action="store_true",
                    help="Auto-pick newest Data/RawGameData/<YYYYMMDD_HHMMSS>/Excels/MonsterTable.json")
    ap.add_argument("--raw-root", default=str(RAW_ROOT_DEFAULT),
                    help="Root of RawGameData to search when using --from-latest")
    ap.add_argument("--output", default=str(DEFAULT_OUTPUT),
                    help="Path to monster_name_mapping.json")
    ap.add_argument("--include-empty", action="store_true", help="Include entries with empty names")
    ap.add_argument("--sort", action="store_true", help="Sort mapping by numeric Id before writing")
    args = ap.parse_args()

    output_path = Path(args.output).resolve()
    output_path.parent.mkdir(parents=True, exist_ok=True)

    # Resolve input
    if args.input:
        input_path = Path(args.input).resolve()
        resolved_from = "explicit --input"
    elif args.from_latest:
        cand = find_latest_monster_table(Path(args.raw_root).resolve())
        if not cand:
            raise FileNotFoundError(
                f"No timestamped MonsterTable.json found under: {args.raw_root}\n"
                f"Looked for: <run>/Excels/MonsterTable.json"
            )
        input_path = cand
        resolved_from = f"--from-latest ({input_path.parent.parent.name})"
    else:
        input_path = DEFAULT_INPUT_FALLBACK.resolve()
        resolved_from = "fallback (non-timestamped)"

    print(f"[i] Input  : {input_path}  [{resolved_from}]")
    print(f"[i] Output : {output_path}")

    if not input_path.exists():
        raise FileNotFoundError(f"MonsterTable.json not found: {input_path}")

    data = load_json(input_path)
    mapping = extract_mapping(data, include_empty=args.include_empty)

    if args.sort:
        try:
            mapping = dict(sorted(mapping.items(), key=lambda kv: int(kv[0])))
        except Exception:
            # if ids aren't numeric, fall back to string sort
            mapping = dict(sorted(mapping.items(), key=lambda kv: kv[0]))

    with output_path.open("w", encoding="utf-8") as f:
        json.dump(mapping, f, ensure_ascii=False, indent=2)

    print(f"[✓] Wrote {len(mapping):,} entries.")
    return 0

if __name__ == "__main__":
    raise SystemExit(main())
