#!/usr/bin/env python3
# -*- coding: utf-8 -*-

r"""
Skill table generator (+ BuffTable + RecountTable + placeholder fallbacks)

Passes:
  1) Build Id→Name from SkillTable (fallback NameDesign)
  2) Merge BuffTable:
     - append missing Ids from BuffTable
     - replace Name=="场地标记01" with Buff.Name (if exists)
  3) Fallback:
     - remaining Name=="场地标记01" → SkillTable.NameDesign (if exists)
  4) Fix Buff placeholder:
     - Name=="气刃突刺计数" → BuffTable.NameDesign (if exists)
  5) RecountTable override (last pass):
     - For each Id in RecountTable: set Name = RecountName (override if present, append if missing)

Reads via either:
  --input/--buff/--recount "<...>.json"
  --from-latest (Data/RawGameData/<YYYYMMDD_HHMMSS>/Excels)
  fallback non-timestamped: Data/RawGameData/Excels/*.json

Writes:
  Data/ProcessedGameData/skill_name_mapping.json (default)

Usage:
  python skill_table_generator.py --from-latest --sort
"""

from __future__ import annotations
import argparse
import json
import re
from pathlib import Path
from typing import Any, Dict, Optional, Tuple, List

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

RAW_ROOT_DEFAULT = PROJECT_ROOT / "Data" / "RawGameData"
DEFAULT_INPUT_FALLBACK   = RAW_ROOT_DEFAULT / "Excels" / "SkillTable.json"
DEFAULT_BUFF_FALLBACK    = RAW_ROOT_DEFAULT / "Excels" / "BuffTable.json"
DEFAULT_RECOUNT_FALLBACK = RAW_ROOT_DEFAULT / "Excels" / "RecountTable.json"
DEFAULT_OUTDIR  = PROJECT_ROOT / "Data" / "ProcessedGameData"
DEFAULT_OUTPUT  = DEFAULT_OUTDIR / "skill_name_mapping.json"

TS_DIR_RE = re.compile(r"^\d{8}_\d{6}$")  # e.g., 20251016_105706

PLACEHOLDER_SKILL = "场地标记01"
PLACEHOLDER_BUFF_NAME = "气刃突刺计数"

# ---------- latest file helpers ----------
def find_latest_file(raw_root: Path, filename: str) -> Optional[Path]:
    if not raw_root.exists():
        return None
    # Prefer proper timestamped dirs
    ts_dirs = [d for d in raw_root.iterdir() if d.is_dir() and TS_DIR_RE.match(d.name)]
    if ts_dirs:
        newest = max(ts_dirs, key=lambda p: p.name)
        cand = newest / "Excels" / filename
        if cand.exists():
            return cand
    # Otherwise take the most recently modified subdir that has the file
    latest: Optional[Tuple[float, Path]] = None
    for d in raw_root.iterdir():
        if not d.is_dir():
            continue
        cand = d / "Excels" / filename
        if cand.exists():
            t = d.stat().st_mtime
            if latest is None or t > latest[0]:
                latest = (t, cand)
    return latest[1] if latest else None

# ---------- IO ----------
def load_json(path: Path) -> Any:
    with path.open("r", encoding="utf-8-sig") as f:
        return json.load(f)

# SkillTable → (name_map, namedesign_map)
def extract_maps_from_skill(data: Any, include_empty: bool = False) -> Tuple[Dict[str, str], Dict[str, str]]:
    skill_map: Dict[str, str] = {}
    namedesign_map: Dict[str, str] = {}

    def push(entry: dict):
        sid = str(entry["Id"]) if "Id" in entry else None
        if sid is None:
            return
        nd = (entry.get("NameDesign") or "").strip()
        namedesign_map[sid] = nd
        nm = (entry.get("Name") or "").strip()
        if not nm:
            nm = nd
        if nm or include_empty:
            skill_map[sid] = nm

    if isinstance(data, dict):
        for k, entry in data.items():
            if isinstance(entry, dict):
                if "Id" not in entry:
                    try:
                        entry = {**entry, "Id": int(k)}
                    except Exception:
                        entry = {**entry, "Id": k}
                push(entry)
    elif isinstance(data, list):
        for entry in data:
            if isinstance(entry, dict) and ("Id" in entry):
                push(entry)
    else:
        raise ValueError("Unexpected SkillTable JSON structure.")

    return skill_map, namedesign_map

# BuffTable → (name_map, namedesign_map)
def extract_maps_from_buff(data: Any, include_empty: bool = False) -> Tuple[Dict[str, str], Dict[str, str]]:
    name_map: Dict[str, str] = {}
    nd_map: Dict[str, str] = {}

    def push(entry: dict, key: str):
        sid = key
        if "Id" in entry:
            sid = str(entry["Id"])
        nm = (entry.get("Name") or "").strip()
        nd = (entry.get("NameDesign") or "").strip()
        if nm or include_empty:
            name_map[sid] = nm
        nd_map[sid] = nd

    if isinstance(data, dict):
        for k, entry in data.items():
            if isinstance(entry, dict):
                push(entry, str(k))
    elif isinstance(data, list):
        for entry in data:
            if isinstance(entry, dict):
                k = str(entry.get("Id", ""))
                if k:
                    push(entry, k)
    else:
        raise ValueError("Unexpected BuffTable JSON structure.")
    return name_map, nd_map

# RecountTable → name_map (Id → RecountName)
def extract_map_from_recount(data: Any) -> Dict[str, str]:
    out: Dict[str, str] = {}

    def push(entry: dict, key: str):
        sid = key
        if "Id" in entry:
            sid = str(entry["Id"])
        nm = (entry.get("RecountName") or "").strip()
        if nm:
            out[sid] = nm

    if isinstance(data, dict):
        for k, entry in data.items():
            if isinstance(entry, dict):
                push(entry, str(k))
    elif isinstance(data, list):
        for entry in data:
            if isinstance(entry, dict):
                k = str(entry.get("Id", ""))
                if k:
                    push(entry, k)
    else:
        raise ValueError("Unexpected RecountTable JSON structure.")
    return out

# ---------- merges & fixes ----------
def merge_with_buff(skill_map: Dict[str, str], buff_name_map: Dict[str, str], include_empty: bool = False):
    appended: List[str] = []
    replaced_from_buff: List[str] = []
    placeholder_no_match: List[str] = []

    # Append missing
    for bid, bname in buff_name_map.items():
        if bid not in skill_map:
            if bname or include_empty:
                skill_map[bid] = bname
                appended.append(bid)

    # Replace placeholder using Buff.Name
    for sid, sname in list(skill_map.items()):
        if sname == PLACEHOLDER_SKILL:
            bname = buff_name_map.get(sid, "")
            if bname:
                skill_map[sid] = bname
                replaced_from_buff.append(sid)
            else:
                placeholder_no_match.append(sid)

    return appended, replaced_from_buff, placeholder_no_match

def fallback_placeholder_with_namedesign(skill_map: Dict[str, str], namedesign_map: Dict[str, str]):
    replaced_nd: List[str] = []
    still_placeholder: List[str] = []
    for sid, sname in list(skill_map.items()):
        if sname == PLACEHOLDER_SKILL:
            nd = (namedesign_map.get(sid) or "").strip()
            if nd and nd != PLACEHOLDER_SKILL:
                skill_map[sid] = nd
                replaced_nd.append(sid)
            else:
                still_placeholder.append(sid)
    return replaced_nd, still_placeholder

# Buff placeholder "气刃突刺计数" -> BuffTable.NameDesign
def fix_buff_placeholder_with_namedesign(skill_map: Dict[str, str], buff_namedesign_map: Dict[str, str]):
    replaced_buff_nd: List[str] = []
    still_buff_placeholder: List[str] = []
    for sid, sname in list(skill_map.items()):
        if sname == PLACEHOLDER_BUFF_NAME:
            nd = (buff_namedesign_map.get(sid) or "").strip()
            if nd and nd != PLACEHOLDER_BUFF_NAME:
                skill_map[sid] = nd
                replaced_buff_nd.append(sid)
            else:
                still_buff_placeholder.append(sid)
    return replaced_buff_nd, still_buff_placeholder

# Recount override: always prefer RecountName; append if missing
def apply_recount_override(skill_map: Dict[str, str], recount_map: Dict[str, str]):
    replaced_by_recount: List[str] = []
    appended_by_recount: List[str] = []
    for rid, rname in recount_map.items():
        if rid in skill_map:
            if skill_map[rid] != rname:
                skill_map[rid] = rname
                replaced_by_recount.append(rid)
        else:
            skill_map[rid] = rname
            appended_by_recount.append(rid)
    return replaced_by_recount, appended_by_recount

# ---------- main ----------
def main() -> int:
    ap = argparse.ArgumentParser(description="Generate skill ID→Name mapping (SkillTable + BuffTable + RecountTable + fallbacks).")
    ap.add_argument("--input", help="Explicit path to SkillTable.json")
    ap.add_argument("--buff", help="Explicit path to BuffTable.json")
    ap.add_argument("--recount", help="Explicit path to RecountTable.json")
    ap.add_argument("--from-latest", action="store_true",
                    help="Auto-pick newest Data/RawGameData/<YYYYMMDD_HHMMSS>/Excels/(Skill|Buff|Recount)Table.json")
    ap.add_argument("--raw-root", default=str(RAW_ROOT_DEFAULT),
                    help="Root of RawGameData to search when using --from-latest")
    ap.add_argument("--output", default=str(DEFAULT_OUTPUT), help="Path to skill_name_mapping.json")
    ap.add_argument("--include-empty", action="store_true", help="Include entries with empty names")
    ap.add_argument("--sort", action="store_true", help="Sort mapping by numeric Id before writing")
    args = ap.parse_args()

    raw_root = Path(args.raw_root).resolve()

    # Resolve SkillTable
    if args.input:
        skill_path = Path(args.input).resolve()
        resolved_skill_from = "explicit --input"
    elif args.from_latest:
        cand = find_latest_file(raw_root, "SkillTable.json")
        if not cand:
            raise FileNotFoundError(f"No timestamped SkillTable.json found under: {raw_root}")
        skill_path = cand
        resolved_skill_from = f"--from-latest ({skill_path.parent.parent.name})"
    else:
        skill_path = DEFAULT_INPUT_FALLBACK.resolve()
        resolved_skill_from = "fallback (non-timestamped)"

    # Resolve BuffTable
    if args.buff:
        buff_path = Path(args.buff).resolve()
        resolved_buff_from = "explicit --buff"
    else:
        co = skill_path.parent / "BuffTable.json"
        if co.exists():
            buff_path = co.resolve()
            try:
                bucket = skill_path.parent.parent.name
            except Exception:
                bucket = "Excels"
            resolved_buff_from = f"same dir as SkillTable ({bucket})"
        elif args.from_latest:
            candb = find_latest_file(raw_root, "BuffTable.json")
            buff_path = (candb or DEFAULT_BUFF_FALLBACK).resolve()
            if candb:
                resolved_buff_from = f"--from-latest ({buff_path.parent.parent.name})"
            else:
                resolved_buff_from = "fallback (non-timestamped)"
        else:
            buff_path = DEFAULT_BUFF_FALLBACK.resolve()
            resolved_buff_from = "fallback (non-timestamped)"

    # Resolve RecountTable
    if args.recount:
        recount_path = Path(args.recount).resolve()
        resolved_recount_from = "explicit --recount"
    else:
        co = skill_path.parent / "RecountTable.json"
        if co.exists():
            recount_path = co.resolve()
            try:
                bucket = skill_path.parent.parent.name
            except Exception:
                bucket = "Excels"
            resolved_recount_from = f"same dir as SkillTable ({bucket})"
        elif args.from_latest:
            candr = find_latest_file(raw_root, "RecountTable.json")
            recount_path = (candr or DEFAULT_RECOUNT_FALLBACK).resolve()
            if candr:
                resolved_recount_from = f"--from-latest ({recount_path.parent.parent.name})"
            else:
                resolved_recount_from = "fallback (non-timestamped)"
        else:
            recount_path = DEFAULT_RECOUNT_FALLBACK.resolve()
            resolved_recount_from = "fallback (non-timestamped)"

    output_path = Path(args.output).resolve()
    output_path.parent.mkdir(parents=True, exist_ok=True)

    print(f"[i] SkillTable  : {skill_path}  [{resolved_skill_from}]")
    print(f"[i] BuffTable   : {buff_path}   [{resolved_buff_from}]")
    print(f"[i] RecountTable: {recount_path}   [{resolved_recount_from}]")
    print(f"[i] Output      : {output_path}")

    if not skill_path.exists():
        raise FileNotFoundError(f"SkillTable.json not found: {skill_path}")
    if not buff_path.exists():
        raise FileNotFoundError(f"BuffTable.json not found: {buff_path}")
    if not recount_path.exists():
        raise FileNotFoundError(f"RecountTable.json not found: {recount_path}")

    # Build maps
    skill_data   = load_json(skill_path)
    buff_data    = load_json(buff_path)
    recount_data = load_json(recount_path)

    skill_map, namedesign_map = extract_maps_from_skill(skill_data, include_empty=args.include_empty)
    buff_name_map, buff_namedesign_map = extract_maps_from_buff(buff_data, include_empty=args.include_empty)
    recount_map = extract_map_from_recount(recount_data)

    before_len = len(skill_map)

    # 2) Merge with BuffTable (append + replace 场地标记01 via Buff.Name)
    appended, replaced_from_buff, placeholder_no_match = merge_with_buff(
        skill_map, buff_name_map, include_empty=args.include_empty
    )

    # 3) Fallback 场地标记01 -> SkillTable.NameDesign
    replaced_from_nd, still_placeholder = fallback_placeholder_with_namedesign(skill_map, namedesign_map)

    # 4) Fix Buff placeholder 气刃突刺计数 -> BuffTable.NameDesign
    replaced_buff_nd, still_buff_placeholder = fix_buff_placeholder_with_namedesign(skill_map, buff_namedesign_map)

    # 5) Recount override (last)
    replaced_by_recount, appended_by_recount = apply_recount_override(skill_map, recount_map)

    if args.sort:
        try:
            skill_map = dict(sorted(skill_map.items(), key=lambda kv: int(kv[0])))
        except Exception:
            skill_map = dict(sorted(skill_map.items(), key=lambda kv: kv[0]))

    # Write
    with output_path.open("w", encoding="utf-8") as f:
        json.dump(skill_map, f, ensure_ascii=False, indent=2)

    # Summary
    print(f"[✓] Wrote {len(skill_map):,} entries (was {before_len:,}).")
    if appended:
        print(f"[i] Appended from BuffTable: {len(appended)}")
    if replaced_from_buff:
        print(f"[i] Replaced '{PLACEHOLDER_SKILL}' using Buff.Name: {len(replaced_from_buff)}")
    if replaced_from_nd:
        print(f"[i] Replaced remaining '{PLACEHOLDER_SKILL}' using Skill.NameDesign: {len(replaced_from_nd)}")
    if replaced_buff_nd:
        print(f"[i] Fixed Buff placeholder '{PLACEHOLDER_BUFF_NAME}' using Buff.NameDesign: {len(replaced_buff_nd)}")

    if appended_by_recount:
        print(f"[i] Appended from RecountTable: {len(appended_by_recount)}")
    if replaced_by_recount:
        print(f"[i] Overridden by RecountTable (Name ← RecountName): {len(replaced_by_recount)}")

    # Any leftovers (should be none after Recount pass, but we report anyway)
    if still_placeholder:
        print(f"[!] Still '{PLACEHOLDER_SKILL}' after all passes: {len(still_placeholder)}")
    if still_buff_placeholder:
        print(f"[!] Still Buff placeholder '{PLACEHOLDER_BUFF_NAME}' without NameDesign: {len(still_buff_placeholder)}")

    return 0

if __name__ == "__main__":
    raise SystemExit(main())
