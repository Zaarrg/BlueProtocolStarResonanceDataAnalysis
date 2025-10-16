
# BlueProtocolStarResonanceDataAnalysis

Toolkit for **extracting and processing game data** from *Blue Protocol: Star Resonance*.

## Current Scope
- ✅ **Implemented:** **Skill ID → English name** mapping — **complete and ready to use**
- 🧩 **WIP (minor):** A **small subset of low-priority/rare entries** (e.g., edge-case boss/monster skills) may still be missing or placeholdered and are tracked as WIP. Core gameplay skills and descriptions are covered.

## Roadmap / TODO
- Dungeon & world-boss datasets
- Drop tables / drop-rate parsing
- Cross-referencing boss/monster skill sources
- Cleaner exports (JSON/CSV) for dashboards

## Quick Start
1. Extract raw game data:

   ```bash
   py Tools/DataTools/Scripts/bspr_data_extraction_pipline.py
   ```

   If needed, specify paths explicitly:

   ```bash
   py Tools/DataTools/Scripts/bspr_data_extraction_pipline.py --dll "path/to/GameAssembly.dll" --metadata "path/to/global-metadata.dat"
   ```
2. Generate **Skill ID → English** mapping:

   ```bash
   py Tools/GameTools/Skills/skill_table_generator.py
   ```

## Documentation

See **[GameData.md](GameData.md)** for detailed extraction and processing notes.

---

### Legal & Ethical Disclaimer

This repository is provided **for educational and research purposes only**. Do **not** use it to violate Terms of Service, enable cheating, bypass anti-cheat/security, or infringe on intellectual property. The authors and contributors **do not condone or support harmful use** and **disclaim responsibility for misuse**. Always respect applicable laws and the rights of the game publisher/developer.