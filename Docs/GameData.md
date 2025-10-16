# Data Extraction

1. Run `bspr_data_extraction_pipline.py`

> Note: "GameAssembly.dll" and "global-metadata.dat" might need to be specified separately via `--dll` and `--metadata` pkg. Backuped files can be found in `Data/UnityFiles`

2. This will detect the Steam Install of BPSR and Extract the Data and output it into `Data/RawGameData/`


# Data Processing

1. To generate a ID to English Names mapping run `py skill_table_generator.py`

> Note: As data understanding gets better the translations will improve, as currently some skills in the `SkillTable` are not translated and can be rather found in other tables. E.g Boss or Monster Skill.

# About "GameAssembly" and "MetaData"

- Before 14.10.2025 BPSR Steam Version had no anticheat therefore data extraction was quite easy. Currently extracting from the Steam Version is harder as it now has ACE and MetaData was moved.

> **Legal & Ethical Disclaimer**  
> This project is provided **for educational and research purposes only**.  
> It must **not** be used to violate any game's Terms of Service, enable cheating, bypass security/anti-cheat, or infringe on intellectual property.  
> The authors and contributors **do not condone or support harmful use** and disclaim all responsibility for misuse.  
> Respect local laws and the rights of the game publishers and developers at all times.