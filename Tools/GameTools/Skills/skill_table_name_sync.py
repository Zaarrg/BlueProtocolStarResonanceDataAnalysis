#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
Copy SkillTable.json into ProcessedGameData and make NameDesign == Name for every entry.
- Reads:  Data/RawGameData/Excels/SkillTable.json
- Writes: Data/ProcessedGameData/SkillTable.json   (by default)

Usage:
  python skill_table_name_sync.py
  python skill_table_name_sync.py --input "...\SkillTable.json" --output "...\ProcessedGameData\SkillTable.json"
"""

from __future__ import annotations
import argparse
import json
from pathlib import Path
from typing import Any, MutableMapping
from collections import OrderedDict

# --- locate repo root based on script path (…/Tools/GameTools/Skills) ---
SCRIPT_DIR = Path(__file__).resolve().parent
PROJECT_ROOT = SCRIPT_DIR.parents[2]

DEFAULT_INPUT  = PROJECT_ROOT / "Data" / "RawGameData" / "Excels" / "SkillTable.json"
DEFAULT_OUTPUT = PROJECT_ROOT / "Data" / "ProcessedGameData" / "SkillTable.json"

def load_json_preserve_order(path: Path) -> Any:
    with path.open("r", encoding="utf-8-sig") as f:
        return json.load(f, object_pairs_hook=OrderedDict)

def dump_json(path: Path, obj: Any) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    with path.open("w", encoding="utf-8") as f:
        json.dump(obj, f, ensure_ascii=False, indent=2)

def sync_namedesign(entry: MutableMapping[str, Any]) -> None:
    """Force NameDesign to equal Name (use empty string if Name missing)."""
    if not isinstance(entry, MutableMapping):
        return
    name = entry.get("Name")
    if name is None:
        name = ""
    entry["NameDesign"] = name

def main() -> int:
    ap = argparse.ArgumentParser(description="Copy SkillTable and set NameDesign = Name for all entries.")
    ap.add_argument("--input",  default=str(DEFAULT_INPUT),  help="Path to source SkillTable.json")
    ap.add_argument("--output", default=str(DEFAULT_OUTPUT), help="Destination file in ProcessedGameData")
    args = ap.parse_args()

    in_path  = Path(args.input).resolve()
    out_path = Path(args.output).resolve()

    if not in_path.exists():
        raise FileNotFoundError(f"Input not found: {in_path}")

    print(f"[i] Reading : {in_path}")
    print(f"[i] Writing : {out_path}")

    data = load_json_preserve_order(in_path)

    total = 0
    if isinstance(data, dict):
        for _, entry in data.items():
            if isinstance(entry, MutableMapping):
                sync_namedesign(entry)
                total += 1
    elif isinstance(data, list):
        for entry in data:
            if isinstance(entry, MutableMapping):
                sync_namedesign(entry)
                total += 1
    else:
        raise ValueError("Unexpected JSON root (expected dict or list).")

    dump_json(out_path, data)
    print(f"[✓] Done. Entries processed: {total:,}")
    return 0

if __name__ == "__main__":
    raise SystemExit(main())