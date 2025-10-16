#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
Skill table generator

Reads:
  Data/RawGameData/Excels/SkillTable.json

Writes:
  Data/ProcessedGameData/skill_name_mapping.json

Mapping:
  "<Id>": "<Name>"  (falls back to NameDesign when Name is empty)

Usage:
  python skill_table_generator.py
  python skill_table_generator.py --input "...\SkillTable.json" --output "...\skill_name_mapping.json" --include-empty --sort
"""

from __future__ import annotations
import argparse
import json
from pathlib import Path
from typing import Any, Dict

# ---------- locate repo root (robust to folder moves) ----------
def find_project_root(start: Path) -> Path:
    cur = start.resolve()
    for _ in range(8):
        if (cur / "Data").exists() and (cur / "Tools").exists():
            return cur
        if cur.parent == cur:
            break
        cur = cur.parent
    # fallback: assume script at Tools/GameTools/Skills/
    return start.resolve().parents[3]

SCRIPT_DIR = Path(__file__).resolve().parent
PROJECT_ROOT = find_project_root(SCRIPT_DIR)

DEFAULT_INPUT  = PROJECT_ROOT / "Data" / "RawGameData" / "Excels" / "SkillTable.json"
DEFAULT_OUTDIR = PROJECT_ROOT / "Data" / "ProcessedGameData"
DEFAULT_OUTPUT = DEFAULT_OUTDIR / "skill_name_mapping.json"


def load_json(path: Path) -> Any:
    # utf-8-sig handles BOM if present
    with path.open("r", encoding="utf-8-sig") as f:
        return json.load(f)


def extract_mapping(data: Any, include_empty: bool = False) -> Dict[str, str]:
    """
    Accepts either:
      - dict keyed by stringified IDs -> entry objects
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
            if not isinstance(entry, dict):
                continue
            name = choose_name(entry)
            if name or include_empty:
                out[str(k)] = name
    elif isinstance(data, list):
        for entry in data:
            if not isinstance(entry, dict):
                continue
            # Prefer explicit Id, else skip
            if "Id" not in entry:
                continue
            id_str = str(entry["Id"])
            name = choose_name(entry)
            if name or include_empty:
                out[id_str] = name
    else:
        raise ValueError("Unexpected JSON structure: expected dict or list at root.")

    return out


def main() -> int:
    ap = argparse.ArgumentParser(description="Generate skill ID→Name mapping from SkillTable.json")
    ap.add_argument("--input",  default=str(DEFAULT_INPUT),  help="Path to SkillTable.json")
    ap.add_argument("--output", default=str(DEFAULT_OUTPUT), help="Path to skill_name_mapping.json")
    ap.add_argument("--include-empty", action="store_true", help="Include entries with empty names")
    ap.add_argument("--sort", action="store_true", help="Sort mapping by numeric Id before writing")
    args = ap.parse_args()

    input_path  = Path(args.input).resolve()
    output_path = Path(args.output).resolve()
    output_path.parent.mkdir(parents=True, exist_ok=True)

    print(f"[i] Input : {input_path}")
    print(f"[i] Output: {output_path}")

    if not input_path.exists():
        raise FileNotFoundError(f"SkillTable.json not found: {input_path}")

    data = load_json(input_path)
    mapping = extract_mapping(data, include_empty=args.include_empty)

    if args.sort:
        # sort by numeric id if possible
        mapping = dict(sorted(mapping.items(), key=lambda kv: int(kv[0])))

    # Write pretty UTF-8 JSON
    with output_path.open("w", encoding="utf-8") as f:
        json.dump(mapping, f, ensure_ascii=False, indent=2)

    print(f"[✓] Wrote {len(mapping):,} entries.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
