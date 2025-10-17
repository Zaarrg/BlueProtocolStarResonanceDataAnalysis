namespace StarResonanceDpsAnalysis.Core
{
    // Auto-generated from skill_config.json (v2.0.1)
    public enum SkillType
    {
        Damage,
        Heal,
        Unknown
    }

    public enum ElementType
    {
        Dark,
        Earth,
        Fire,
        Ice,
        Light,
        Thunder,
        Wind,
        Physics,   // ← 新增
        Unknown
    }

    public sealed class SkillDefinition
    {
        public string Name { get; set; } = "";
        public SkillType Type { get; set; } = SkillType.Unknown;
        public ElementType Element { get; set; } = ElementType.Unknown;
        public string Description { get; set; } = "";
    }

    public sealed class ElementInfo
    {
        public string Color { get; set; } = "#FFFFFF";
        public string Icon { get; set; } = "";
    }

    public static class EmbeddedSkillConfig
    {
        public static readonly string Version = "2.0.1";      // ← 更新
        public static readonly string LastUpdated = "2025-01-20"; // ← 更新

        // 与 skill_config.json 的 elements 完全一致
        public static readonly Dictionary<ElementType, ElementInfo> Elements = new()
        {
            [ElementType.Fire] = new ElementInfo { Color = "#ff6b6b", Icon = "🔥" },
            [ElementType.Ice] = new ElementInfo { Color = "#74c0fc", Icon = "❄️" },
            [ElementType.Thunder] = new ElementInfo { Color = "#ffd43b", Icon = "⚡" },
            [ElementType.Earth] = new ElementInfo { Color = "#8ce99a", Icon = "🍀" }, // ← 图标从🌍改为🍀
            [ElementType.Wind] = new ElementInfo { Color = "#91a7ff", Icon = "💨" },
            [ElementType.Light] = new ElementInfo { Color = "#fff3bf", Icon = "✨" },
            [ElementType.Dark] = new ElementInfo { Color = "#9775fa", Icon = "🌙" },
            [ElementType.Physics] = new ElementInfo { Color = "#91a7ff", Icon = "⚔️" }  // ← 新增
        };

        // 与 skill_config.json 的 skills 完全一致（覆盖之前的列表）
        public static readonly Dictionary<string, SkillDefinition> SkillsByString = new()
        {
            ["1401"] = new SkillDefinition { Name = "Windborne Grace", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风华翔舞" },
            ["1402"] = new SkillDefinition { Name = "Normal Attack", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风华翔舞" },
            ["1403"] = new SkillDefinition { Name = "Normal Attack", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风华翔舞" },
            ["1404"] = new SkillDefinition { Name = "Normal Attack", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风华翔舞" },
            ["1409"] = new SkillDefinition { Name = "Typhoon Cleave", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风神·破阵之风" },
            ["1420"] = new SkillDefinition { Name = "Galeform", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风姿卓绝" },
            ["2031104"] = new SkillDefinition { Name = "Lance - Luck", Type = SkillType.Damage, Element = ElementType.Light, Description = "幸运一击(长枪)" },
            ["1418"] = new SkillDefinition { Name = "Gale Thrust", Type = SkillType.Damage, Element = ElementType.Wind, Description = "疾风刺" },
            ["1421"] = new SkillDefinition { Name = "Spiral Thrust", Type = SkillType.Damage, Element = ElementType.Wind, Description = "螺旋击刺" },
            ["1434"] = new SkillDefinition { Name = "Vortex Strike", Type = SkillType.Damage, Element = ElementType.Wind, Description = "神影螺旋" },
            ["140301"] = new SkillDefinition { Name = "Divine Shadow Spiral (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "神影螺旋" },
            ["1422"] = new SkillDefinition { Name = "Breach Pursuit", Type = SkillType.Damage, Element = ElementType.Wind, Description = "破追" },
            ["1427"] = new SkillDefinition { Name = "Breach Pursuit", Type = SkillType.Damage, Element = ElementType.Wind, Description = "破追" },
            ["31901"] = new SkillDefinition { Name = "Rock Projectile", Type = SkillType.Damage, Element = ElementType.Wind, Description = "勇气风环" },
            ["2205450"] = new SkillDefinition { Name = "Fortitude (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "勇气风环吸血" },
            ["1411"] = new SkillDefinition { Name = "Swift Blade", Type = SkillType.Damage, Element = ElementType.Wind, Description = "疾驰锋刃" },
            ["1435"] = new SkillDefinition { Name = "Drake Cannon", Type = SkillType.Damage, Element = ElementType.Wind, Description = "龙击炮" },
            ["140401"] = new SkillDefinition { Name = "Gravity Impact (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "龙击炮" },
            ["2205071"] = new SkillDefinition { Name = "EX Skill: Instant Restore – Rend (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "撕裂" },
            ["149901"] = new SkillDefinition { Name = "Wind Spiral", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风螺旋/螺旋引爆" },
            ["1419"] = new SkillDefinition { Name = "Skyfall", Type = SkillType.Damage, Element = ElementType.Wind, Description = "翔返" },
            ["1424"] = new SkillDefinition { Name = "Instant Edge", Type = SkillType.Damage, Element = ElementType.Wind, Description = "刹那" },
            ["1425"] = new SkillDefinition { Name = "Falcon Toss", Type = SkillType.Damage, Element = ElementType.Wind, Description = "飞鸟投" },
            ["149905"] = new SkillDefinition { Name = "Tina – Flying Bird Throw (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "飞鸟投" },
            ["1433"] = new SkillDefinition { Name = "Azure Sever", Type = SkillType.Damage, Element = ElementType.Wind, Description = "极·岚切" },
            ["149906"] = new SkillDefinition { Name = "Tina – Empowered Flying Bird Throw (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "极·岚切" },
            ["149907"] = new SkillDefinition { Name = "Tina – Sharp Impact I (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "锐利冲击(风神)" },
            ["1431"] = new SkillDefinition { Name = "Sharp Impact", Type = SkillType.Damage, Element = ElementType.Wind, Description = "锐利冲击(风神)" },
            ["149902"] = new SkillDefinition { Name = "Tina – Lance Formation (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "长矛贯穿" },
            ["140501"] = new SkillDefinition { Name = "Tornado (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "龙卷风" },

            ["1701"] = new SkillDefinition { Name = "Judgment Cut", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "我流刀法·诛恶" },
            ["1702"] = new SkillDefinition { Name = "Olvera ATK_02 (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "我流刀法·诛恶" },
            ["1703"] = new SkillDefinition { Name = "Olvera ATK_03 (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "我流刀法·诛恶" },
            ["1704"] = new SkillDefinition { Name = "Olvera ATK_04 (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "我流刀法·诛恶" },
            ["1713"] = new SkillDefinition { Name = "Oblivion Combo", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "极诣·大破灭连斩" },
            ["1728"] = new SkillDefinition { Name = "Oblivion Combo", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "极诣·大破灭连斩(天赋)" },
            ["1714"] = new SkillDefinition { Name = "Iaido Slash", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "居合" },
            ["1717"] = new SkillDefinition { Name = "Flash strike", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "一闪" },
            ["1718"] = new SkillDefinition { Name = "Raijin Dash", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "飞雷神" },
            ["1735"] = new SkillDefinition { Name = "Dracoflash", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "坠龙闪" },
            ["1736"] = new SkillDefinition { Name = "Phantom Slash", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "神影斩" },
            ["155101"] = new SkillDefinition { Name = "Lightning Sever (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "雷切" },
            ["1715"] = new SkillDefinition { Name = "Moonstrike", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "月影" },
            ["1719"] = new SkillDefinition { Name = "Scythe Wheel", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "镰车" },
            ["1724"] = new SkillDefinition { Name = "Thundercut", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "霹雳连斩" },
            ["1705"] = new SkillDefinition { Name = "Overdrive", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "超高出力" },
            ["1732"] = new SkillDefinition { Name = "Olvera Triggers Stormflash", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "千雷闪影之意" },
            ["1737"] = new SkillDefinition { Name = "Divine Sickle", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "神罚之镰" },
            ["1738"] = new SkillDefinition { Name = "Chaos Breaker", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "缭乱兜割" },
            ["1739"] = new SkillDefinition { Name = "Olvera – Mind’s Eye Strike (Trigger) (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "看破斩" },
            ["1740"] = new SkillDefinition { Name = "Storm Scythe", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "雷霆之镰(触发霹雳升龙斩时)" },
            ["1741"] = new SkillDefinition { Name = "Storm Scythe", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "雷霆之镰" },
            ["1742"] = new SkillDefinition { Name = "Thundercleave", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "霹雳升龙斩" },
            ["44701"] = new SkillDefinition { Name = "Scythe Cart Damage (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "月刃" },
            ["179908"] = new SkillDefinition { Name = "Thunderstrike", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "雷击" },
            ["179906"] = new SkillDefinition { Name = "Moonstrike Whirl", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "月刃回旋" },
            ["2031101"] = new SkillDefinition { Name = "Blade - Luck", Type = SkillType.Damage, Element = ElementType.Light, Description = "幸运一击(太刀)" },

            ["2330"] = new SkillDefinition { Name = "Soul Musician – Flame Dancer Empowerment (T)", Type = SkillType.Damage, Element = ElementType.Fire, Description = "火柱冲击" },
            ["55314"] = new SkillDefinition { Name = "Torrent", Type = SkillType.Heal, Element = ElementType.Fire, Description = "安可治疗" },
            ["230101"] = new SkillDefinition { Name = "Concerto – Single Hit (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "聚合乐章/安可治疗相关" },
            ["230401"] = new SkillDefinition { Name = "Encore Damage (T)", Type = SkillType.Damage, Element = ElementType.Fire, Description = "安可伤害" },
            ["230501"] = new SkillDefinition { Name = "Endless Performance – Encore Damage (T)", Type = SkillType.Damage, Element = ElementType.Fire, Description = "无限连奏安可伤害" },
            ["2031111"] = new SkillDefinition { Name = "Guitar - Luck", Type = SkillType.Damage, Element = ElementType.Light, Description = "幸运一击(灵魂乐手)" },
            ["2306"] = new SkillDefinition { Name = "Amplified Beat", Type = SkillType.Damage, Element = ElementType.Fire, Description = "增幅节拍" },
            ["2317"] = new SkillDefinition { Name = "Fierce Strike", Type = SkillType.Damage, Element = ElementType.Fire, Description = "猛烈挥击" },
            ["2321"] = new SkillDefinition { Name = "String Strike", Type = SkillType.Damage, Element = ElementType.Fire, Description = "琴弦叩击" },
            ["2322"] = new SkillDefinition { Name = "Beat Performer ATK_02a", Type = SkillType.Damage, Element = ElementType.Fire, Description = "琴弦叩击" },
            ["2323"] = new SkillDefinition { Name = "Beat Performer ATK_03a", Type = SkillType.Damage, Element = ElementType.Fire, Description = "琴弦叩击" },
            ["2324"] = new SkillDefinition { Name = "Beat Performer ATK_04a", Type = SkillType.Damage, Element = ElementType.Fire, Description = "琴弦叩击" },
            ["2331"] = new SkillDefinition { Name = "Scorching Impact", Type = SkillType.Damage, Element = ElementType.Fire, Description = "音浪" },
            ["2335"] = new SkillDefinition { Name = "Infinite Rhapsody", Type = SkillType.Damage, Element = ElementType.Fire, Description = "无限狂想伤害" },
            ["230102"] = new SkillDefinition { Name = "Concerto – 2nd Hit (T)", Type = SkillType.Damage, Element = ElementType.Fire, Description = "聚合乐章" },
            ["230103"] = new SkillDefinition { Name = "Concerto – 3rd Hit (T)", Type = SkillType.Damage, Element = ElementType.Fire, Description = "聚合乐章" },
            ["230104"] = new SkillDefinition { Name = "Concerto – 4th Hit (T)", Type = SkillType.Damage, Element = ElementType.Fire, Description = "聚合乐章" },
            ["230105"] = new SkillDefinition { Name = "Flame's Rampage", Type = SkillType.Damage, Element = ElementType.Fire, Description = "炎律狂踏伤害" },
            ["230106"] = new SkillDefinition { Name = "Flame Note", Type = SkillType.Damage, Element = ElementType.Fire, Description = "烈焰音符伤害" },
            ["231001"] = new SkillDefinition { Name = "Blazing Rhapsody Damage (T)", Type = SkillType.Damage, Element = ElementType.Fire, Description = "烈焰狂想伤害" },
            ["55301"] = new SkillDefinition { Name = "Flame Fantasia Healing (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "烈焰狂想治疗" },
            ["55311"] = new SkillDefinition { Name = "Soul Musician – Innate Resource Recovery Buff (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "安可曲转化" },
            ["55341"] = new SkillDefinition { Name = "Wild Sonata Healing (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "英勇乐章治疗" },
            ["55346"] = new SkillDefinition { Name = "Ultimate – Max Heal (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "无限狂想治疗" },
            ["55355"] = new SkillDefinition { Name = "Basic Attack Hits 1–2 Heal (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "休止的治愈" },
            ["2207141"] = new SkillDefinition { Name = "Dual Melody Healing (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "音符" },
            ["2207151"] = new SkillDefinition { Name = "Blazing Heal", Type = SkillType.Heal, Element = ElementType.Fire, Description = "炽焰治愈" },
            ["2207431"] = new SkillDefinition { Name = "Amplified Beat Effect (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "炎律狂踏治疗" },
            ["2301"] = new SkillDefinition { Name = "Resonant Strings", Type = SkillType.Damage, Element = ElementType.Fire, Description = "琴弦撩拨" },
            ["2302"] = new SkillDefinition { Name = "Beat Performer ATK_01", Type = SkillType.Damage, Element = ElementType.Fire, Description = "琴弦撩拨" },
            ["2303"] = new SkillDefinition { Name = "Beat Performer ATK_03", Type = SkillType.Damage, Element = ElementType.Fire, Description = "琴弦撩拨" },
            ["2304"] = new SkillDefinition { Name = "Beat Performer ATK_04", Type = SkillType.Damage, Element = ElementType.Fire, Description = "琴弦撩拨" },
            ["2312"] = new SkillDefinition { Name = "Fivefold Crescendo", Type = SkillType.Damage, Element = ElementType.Fire, Description = "激涌五重奏伤害" },
            ["2313"] = new SkillDefinition { Name = "Passion Burst", Type = SkillType.Damage, Element = ElementType.Fire, Description = "热情挥洒" },
            ["2332"] = new SkillDefinition { Name = "Passion Fury", Type = SkillType.Damage, Element = ElementType.Fire, Description = "强化热情挥洒" },
            ["2336"] = new SkillDefinition { Name = "Concert Circuit", Type = SkillType.Damage, Element = ElementType.Fire, Description = "巡演曲伤害" },
            ["2366"] = new SkillDefinition { Name = "Concert Circuit copy", Type = SkillType.Damage, Element = ElementType.Fire, Description = "巡演曲伤害(音箱复读的)" },
            ["55302"] = new SkillDefinition { Name = "Special Attack B Healing (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "愈合节拍" },
            ["55304"] = new SkillDefinition { Name = "Surging Quintet Healing (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "激涌五重奏治疗" },
            ["55339"] = new SkillDefinition { Name = "Ultimate B Healing (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "巡演曲治疗" },
            ["55342"] = new SkillDefinition { Name = "Serene Melody Healing (T)", Type = SkillType.Heal, Element = ElementType.Fire, Description = "愈合乐章治疗" },
            ["2207620"] = new SkillDefinition { Name = "Rhythm", Type = SkillType.Heal, Element = ElementType.Fire, Description = "活力解放" },

            ["1501"] = new SkillDefinition { Name = "Vines' Embrace", Type = SkillType.Damage, Element = ElementType.Earth, Description = "掌控藤曼" },
            ["1502"] = new SkillDefinition { Name = "Airona ATK_02 (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "掌控藤曼" },
            ["1503"] = new SkillDefinition { Name = "Airona ATK_03 (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "掌控藤曼" },
            ["1504"] = new SkillDefinition { Name = "Airona ATK_04 (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "掌控藤曼" },
            ["1509"] = new SkillDefinition { Name = "Divine Circle Bloom", Type = SkillType.Damage, Element = ElementType.Earth, Description = "希望结界伤害" },
            ["1518"] = new SkillDefinition { Name = "Wild Bloom", Type = SkillType.Damage, Element = ElementType.Earth, Description = "狂野绽放" },
            ["1529"] = new SkillDefinition { Name = "Blossom Charge", Type = SkillType.Damage, Element = ElementType.Earth, Description = "盛放注能(包含伤害和治疗)" },
            ["1550"] = new SkillDefinition { Name = "Seed Meteor", Type = SkillType.Damage, Element = ElementType.Earth, Description = "不羁之种" },
            ["1551"] = new SkillDefinition { Name = "Talent Trigger – Seed Meteor (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "狂野之种" },
            ["1560"] = new SkillDefinition { Name = "Healing Ring (Spreads once, heals once)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "再生脉冲" },
            ["20301"] = new SkillDefinition { Name = "Healing Effect (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "生命绽放" },
            ["21402"] = new SkillDefinition { Name = "Special Attack Healing (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "狂野绽放治疗" },
            ["21404"] = new SkillDefinition { Name = "HP Regen", Type = SkillType.Heal, Element = ElementType.Earth, Description = "滋养" },
            ["21406"] = new SkillDefinition { Name = "Recovery (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "森之祈愿" },
            ["21414"] = new SkillDefinition { Name = "Sanctuary Field (Nature) (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "希望结界持续(包含伤害和治疗)" },
            ["21427"] = new SkillDefinition { Name = "Basic Attack Hit 1 Heal (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "休止的治愈" },
            ["21428"] = new SkillDefinition { Name = "Basic Attack Hit 2 Heal (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "休止的治愈" },
            ["21429"] = new SkillDefinition { Name = "Basic Attack Hit 3 Heal (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "休止的治愈" },
            ["21430"] = new SkillDefinition { Name = "Basic Attack Hit 4 Heal (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "休止的治愈" },
            ["150103"] = new SkillDefinition { Name = "Untamed Seed (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "不羁之种" },
            ["150104"] = new SkillDefinition { Name = "Untamed Seed (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "不羁之种" },
            ["150106"] = new SkillDefinition { Name = "Infusion (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "灌注" },
            ["150107"] = new SkillDefinition { Name = "Infusion (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "灌注" },
            ["2031005"] = new SkillDefinition { Name = "Spell Ring – Intelligence (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "幸运一击(森语者)" },
            ["2202091"] = new SkillDefinition { Name = "Healing Chain Buff (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "治疗链接" },
            ["2202311"] = new SkillDefinition { Name = "Wrath of Nature’s Spirit – Healing Sub-Buff (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "滋养之种" },
            ["1541"] = new SkillDefinition { Name = "Wild Bloom", Type = SkillType.Damage, Element = ElementType.Earth, Description = "狂野绽放" },
            ["1561"] = new SkillDefinition { Name = "Healing Ring (Damages and heals back and forth)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "再生脉冲(包含伤害和治疗)" },
            ["21423"] = new SkillDefinition { Name = "Symbiotic Mark", Type = SkillType.Heal, Element = ElementType.Earth, Description = "共生印记" },
            ["21424"] = new SkillDefinition { Name = "Thorn", Type = SkillType.Damage, Element = ElementType.Earth, Description = "荆棘" },
            ["150101"] = new SkillDefinition { Name = "Airona Summon ATK_01 (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "鹿之奔袭" },
            ["150110"] = new SkillDefinition { Name = "Infusion (T)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "灌注" },

            ["2031105"] = new SkillDefinition { Name = "Rune Ring - Luck", Type = SkillType.Damage, Element = ElementType.Light, Description = "幸运一击(惩戒)" },
            ["220101"] = new SkillDefinition { Name = "Every Shot Hits (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "弹无虚发" },
            ["220103"] = new SkillDefinition { Name = "Every Shot Hits (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "弹无虚发" },
            ["220104"] = new SkillDefinition { Name = "Storm Arrows (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "暴风箭矢" },
            ["2295"] = new SkillDefinition { Name = "Ultimate - Gravity Gathering Orb", Type = SkillType.Damage, Element = ElementType.Light, Description = "锐眼·光能巨箭" },
            ["2291"] = new SkillDefinition { Name = "Photon Explosion", Type = SkillType.Damage, Element = ElementType.Light, Description = "锐眼·光能巨箭(天赋)" },
            ["2289"] = new SkillDefinition { Name = "Arrow Rain Proxy (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "箭雨" },
            ["2233"] = new SkillDefinition { Name = "Powerdraw", Type = SkillType.Damage, Element = ElementType.Light, Description = "聚能射击" },
            ["2288"] = new SkillDefinition { Name = "Charged Shot Proxy (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "光能轰炸" },
            ["220102"] = new SkillDefinition { Name = "Torrent Shot (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "怒涛射击" },
            ["220108"] = new SkillDefinition { Name = "Explosive Arrows (T)", Type = SkillType.Damage, Element = ElementType.Fire, Description = "爆炸箭矢" },
            ["220109"] = new SkillDefinition { Name = "Deter Shot (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "威慑射击" },
            ["1700820"] = new SkillDefinition { Name = "Wild Wolf - Assist", Type = SkillType.Damage, Element = ElementType.Earth, Description = "狼协同攻击" },
            ["1700827"] = new SkillDefinition { Name = "Wild Wolf - Basic Attack", Type = SkillType.Damage, Element = ElementType.Earth, Description = "狼普攻" },
            ["2292"] = new SkillDefinition { Name = "Phantom Direwolves", Type = SkillType.Damage, Element = ElementType.Earth, Description = "扑咬" },
            ["2203512"] = new SkillDefinition { Name = "Wild Call – Trample Sub-Buff 2 Damage (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "践踏" },
            ["55231"] = new SkillDefinition { Name = "Explosive Arrow BUFF", Type = SkillType.Damage, Element = ElementType.Fire, Description = "爆炸射击" },
            ["220110"] = new SkillDefinition { Name = "Explosive Arrow (T)", Type = SkillType.Damage, Element = ElementType.Fire, Description = "爆炸箭引爆" },
            ["2203291"] = new SkillDefinition { Name = "Skyfall – Sub-Buff (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "猎鹰出击" },
            ["220113"] = new SkillDefinition { Name = "Phantom Falcon (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "幻影猎鹰" },
            ["2203621"] = new SkillDefinition { Name = "Chain Lightning – Main Buff (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "光棱" },
            ["2203622"] = new SkillDefinition { Name = "Thunderlight Derivative Buff – Test (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "光棱溅射" },
            ["220112"] = new SkillDefinition { Name = "Photon Rift (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "光能裂隙" },
            ["220106"] = new SkillDefinition { Name = "Aerial Shot (T)", Type = SkillType.Damage, Element = ElementType.Earth, Description = "二连矢" },
            ["2203521"] = new SkillDefinition { Name = "Steel Beak – Sub-Buff Stacks (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "内爆(钢制喙触发)" },
            ["2203181"] = new SkillDefinition { Name = "Hawkeye Lightning Break – Sub-Buff (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "闪电冲击" },
            ["2294"] = new SkillDefinition { Name = "Lethal Shot", Type = SkillType.Damage, Element = ElementType.Light, Description = "光意·四连矢" },
            ["220111"] = new SkillDefinition { Name = "Luminance: Quad Arrows (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "光意·四连矢" },
            ["220203"] = new SkillDefinition { Name = "Luminance: Quad Arrows (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "光意·四连矢" },
            ["2031109"] = new SkillDefinition { Name = "Bow - Luck", Type = SkillType.Damage, Element = ElementType.Light, Description = "幸运一击(弓箭手)" },
            ["220301"] = new SkillDefinition { Name = "Hawkeye: Photon Great Arrow (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "锐眼·光能巨箭" },
            ["2352"] = new SkillDefinition { Name = "Celestial Eagle - Eagle", Type = SkillType.Damage, Element = ElementType.Light, Description = "天界雄鹰" },

            ["120401"] = new SkillDefinition { Name = "Rain-Lashed Tide (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "雨打潮生" },
            ["1203"] = new SkillDefinition { Name = "Bois ATK_03 (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "雨打潮生" },
            ["120501"] = new SkillDefinition { Name = "Rain-Lashed Tide (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "雨打潮生" },
            ["120201"] = new SkillDefinition { Name = "Rain-Lashed Tide (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "雨打潮生" },
            ["120301"] = new SkillDefinition { Name = "Rain-Lashed Tide (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "雨打潮生" },
            ["2031102"] = new SkillDefinition { Name = "Staff - Luck", Type = SkillType.Damage, Element = ElementType.Light, Description = "幸运一击(冰法)" },
            ["1248"] = new SkillDefinition { Name = "Glacier Hymn", Type = SkillType.Damage, Element = ElementType.Ice, Description = "极寒·冰雪颂歌" },
            ["1263"] = new SkillDefinition { Name = "Staff Ultimate: Powerful Ice Meteor", Type = SkillType.Damage, Element = ElementType.Ice, Description = "极寒·冰雪颂歌" },
            ["120902"] = new SkillDefinition { Name = "Proxy Ice Explosion (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "冰矛" },
            ["1262"] = new SkillDefinition { Name = "Ice Meteor Hologram", Type = SkillType.Damage, Element = ElementType.Ice, Description = "陨星风暴" },
            ["121501"] = new SkillDefinition { Name = "Proxy Dragon Claw Thunderfall (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "清淹绕珠" },
            ["1216"] = new SkillDefinition { Name = "Proxy Ice Orb Explosion (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "强化清淹绕珠" },
            ["1257"] = new SkillDefinition { Name = "Blizzard Proxy (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "寒冰风暴" },
            ["1250"] = new SkillDefinition { Name = "Bois SKILL01_Proxy (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "水之涡流" },
            ["2204081"] = new SkillDefinition { Name = "Frostburst AOE Sub-Buff (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "冰箭爆炸" },
            ["121302"] = new SkillDefinition { Name = "Ice Arrow (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "冰箭" },
            ["1259"] = new SkillDefinition { Name = "Talent – Frost Comet (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "冰霜彗星" },
            ["120901"] = new SkillDefinition { Name = "Proxy Trap Detection (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "贯穿冰矛" },
            ["2204241"] = new SkillDefinition { Name = "Frost Impact – Sub-Buff (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "冰霜冲击" },
            ["1261"] = new SkillDefinition { Name = "Blizzard Proxy – Talent Variant (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "瞬发寒冰风暴" },

            ["2401"] = new SkillDefinition { Name = "Blade of Justice", Type = SkillType.Damage, Element = ElementType.Light, Description = "公正之剑" },
            ["2402"] = new SkillDefinition { Name = "Sword & Shield Basic Attack 02 (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "公正之剑" },
            ["2403"] = new SkillDefinition { Name = "Sword & Shield Basic Attack 03 (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "公正之剑" },
            ["2404"] = new SkillDefinition { Name = "Sword & Shield Basic Attack 04 (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "公正之剑" },
            ["2416"] = new SkillDefinition { Name = "Condemn", Type = SkillType.Damage, Element = ElementType.Light, Description = "断罪" },
            ["2417"] = new SkillDefinition { Name = "Enhanced Condemn", Type = SkillType.Damage, Element = ElementType.Light, Description = "断罪" },
            ["2407"] = new SkillDefinition { Name = "Radiant Infusion", Type = SkillType.Damage, Element = ElementType.Light, Description = "凛威·圣光灌注" },
            ["2031110"] = new SkillDefinition { Name = "Sword and Shield - Luck", Type = SkillType.Damage, Element = ElementType.Light, Description = "幸运一击(剑盾)" },
            ["2405"] = new SkillDefinition { Name = "Valor Bash", Type = SkillType.Damage, Element = ElementType.Light, Description = "英勇盾击" },
            ["2450"] = new SkillDefinition { Name = "Radiant Impact", Type = SkillType.Damage, Element = ElementType.Light, Description = "光明冲击" },
            ["2410"] = new SkillDefinition { Name = "Judgment", Type = SkillType.Damage, Element = ElementType.Light, Description = "裁决" },
            ["2451"] = new SkillDefinition { Name = "Auto Judgment", Type = SkillType.Damage, Element = ElementType.Light, Description = "裁决(神圣触发)" },
            ["2452"] = new SkillDefinition { Name = "Scorching Judgment Sacred Blade", Type = SkillType.Damage, Element = ElementType.Fire, Description = "灼热裁决" },
            ["2412"] = new SkillDefinition { Name = "Reckoning", Type = SkillType.Damage, Element = ElementType.Light, Description = "清算" },
            ["2413"] = new SkillDefinition { Name = "Inferno Reckon", Type = SkillType.Damage, Element = ElementType.Fire, Description = "炽热清算" },
            ["240101"] = new SkillDefinition { Name = "Shield Throw (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "投掷盾牌" },
            ["2206401"] = new SkillDefinition { Name = "Blinding Strike", Type = SkillType.Damage, Element = ElementType.Light, Description = "神圣之击" },
            ["55421"] = new SkillDefinition { Name = "Judgment Healing", Type = SkillType.Heal, Element = ElementType.Light, Description = "裁决治疗" },
            ["55404"] = new SkillDefinition { Name = "Holy Ring (T)", Type = SkillType.Heal, Element = ElementType.Light, Description = "圣环伤害/治疗(相同编号)" },
            ["2406"] = new SkillDefinition { Name = "Vanguard Strike", Type = SkillType.Damage, Element = ElementType.Light, Description = "先锋打击/先锋追击" },
            ["2421"] = new SkillDefinition { Name = "Sacred Blade", Type = SkillType.Damage, Element = ElementType.Light, Description = "圣剑" },
            ["240102"] = new SkillDefinition { Name = "Radiant Resolve (T)", Type = SkillType.Damage, Element = ElementType.Light, Description = "光明决心" },
            ["55412"] = new SkillDefinition { Name = "Zeal Crusade", Type = SkillType.Damage, Element = ElementType.Light, Description = "冷酷征伐" },
            ["2206241"] = new SkillDefinition { Name = "Holy Mark", Type = SkillType.Damage, Element = ElementType.Light, Description = "神圣印记" },
            ["2206552"] = new SkillDefinition { Name = "Radiant Core Explosion", Type = SkillType.Damage, Element = ElementType.Light, Description = "光明核心" },

            ["1005240"] = new SkillDefinition { Name = "Stunt! Predator Slash", Type = SkillType.Damage, Element = ElementType.Dark, Description = "绝技! 追猎猛斩(尖兵)" },
            ["1006940"] = new SkillDefinition { Name = "Arcane! Cocoon Tech", Type = SkillType.Damage, Element = ElementType.Dark, Description = "奥义! 茧房术(蜘蛛)" },
            ["391006"] = new SkillDefinition { Name = "Finisher! Chaotic Missiles (Void Ogre) (T)", Type = SkillType.Damage, Element = ElementType.Dark, Description = "绝技! 纷乱飞弹(虚食人魔)" },
            ["1008440"] = new SkillDefinition { Name = "Arcane! Azure Gale", Type = SkillType.Damage, Element = ElementType.Wind, Description = "奥义! 沧澜风啸(飞鱼)" },
            ["391301"] = new SkillDefinition { Name = "Finisher! Electromagnetic Bomb (Gunner) (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "绝技! 电磁爆弹(枪手)" },
            ["3913001"] = new SkillDefinition { Name = "Resonance – Shadow Legion Rifle – Proxy Explosion (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "绝技! 电磁爆弹(枪手)" },
            ["1008641"] = new SkillDefinition { Name = "Stunt! Whirlwind Strike", Type = SkillType.Damage, Element = ElementType.Wind, Description = "飓风哥布林战士" },
            ["3210081"] = new SkillDefinition { Name = "Murloc Warrior – Passive Trigger (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "蜥蜴人猎手（被动）" },
            ["391007"] = new SkillDefinition { Name = "Goblin Crossbowman (Passive) (T)", Type = SkillType.Damage, Element = ElementType.Physics, Description = "哥布林弩手（被动）" },
            ["391008"] = new SkillDefinition { Name = "Mutant Bee – Passive (T)", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "变异峰弩手（被动）" },
            ["1700440"] = new SkillDefinition { Name = "Arcane! Furious Hammer", Type = SkillType.Damage, Element = ElementType.Dark, Description = "慕课头目" },

            ["1222"] = new SkillDefinition { Name = "Phantom Dash", Type = SkillType.Damage, Element = ElementType.Light, Description = "幻影冲锋" },
            ["1241"] = new SkillDefinition { Name = "Frostbeam", Type = SkillType.Damage, Element = ElementType.Ice, Description = "射线" },
            ["199902"] = new SkillDefinition { Name = "Terra Sunder", Type = SkillType.Damage, Element = ElementType.Earth, Description = "岩盾" },


            // Ice 系
            ["1240"] = new SkillDefinition { Name = "Frozen Gale", Type = SkillType.Damage, Element = ElementType.Ice, Description = "冻结寒风" }, // ← 新增
            ["1242"] = new SkillDefinition { Name = "Frost Lance", Type = SkillType.Damage, Element = ElementType.Ice, Description = "冰霜之矛" }, // ← 新增
            ["1243"] = new SkillDefinition { Name = "Permafrost", Type = SkillType.Damage, Element = ElementType.Ice, Description = "冰之灌注" }, // ← 新增
            ["1245"] = new SkillDefinition { Name = "Frost Shelter", Type = SkillType.Heal, Element = ElementType.Ice, Description = "寒冰庇护" }, // ← 新增
            ["1246"] = new SkillDefinition { Name = "Tidepool", Type = SkillType.Damage, Element = ElementType.Ice, Description = "浪潮汇聚-水龙卷" }, // ← 新增
            ["1247"] = new SkillDefinition { Name = "Frost Comet", Type = SkillType.Damage, Element = ElementType.Ice, Description = "天赋触发彗星" }, // ← 新增
            ["1249"] = new SkillDefinition { Name = "Staff Support – Synergy Ice Crystal (T)", Type = SkillType.Damage, Element = ElementType.Ice, Description = "协同冰晶" }, // ← 新增

            // Wind 系
            ["1405"] = new SkillDefinition { Name = "Gale Thrust", Type = SkillType.Damage, Element = ElementType.Wind, Description = "疾风刺" }, // ← 新增
            ["1406"] = new SkillDefinition { Name = "Tina AIRATK_04 (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风华翔舞" }, // ← 新增
            ["1407"] = new SkillDefinition { Name = "Typhoon", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风神" }, // ← 新增
            ["1410"] = new SkillDefinition { Name = "Typhoon Cleave", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风神·破阵之风" }, // ← 新增
            ["1426"] = new SkillDefinition { Name = "Typhoon Cleave", Type = SkillType.Damage, Element = ElementType.Wind, Description = "风神·破阵之风" }, // ← 新增
            ["1430"] = new SkillDefinition { Name = "Valor Cyclone", Type = SkillType.Damage, Element = ElementType.Wind, Description = "翔返(额外版本)" }, // ← 新增

            // Earth 系
            ["1517"] = new SkillDefinition { Name = "Red light counter", Type = SkillType.Damage, Element = ElementType.Earth, Description = "掌控藤蔓-红光反制" }, // ← 新增
            ["1527"] = new SkillDefinition { Name = "Bloomheal", Type = SkillType.Heal, Element = ElementType.Earth, Description = "花语回升" }, // ← 新增
            ["1556"] = new SkillDefinition { Name = "Flourish Seed", Type = SkillType.Heal, Element = ElementType.Earth, Description = "花语回升" }, // ← 新增
            ["1562"] = new SkillDefinition { Name = "Healing Ring (Spreads once, heals once)", Type = SkillType.Heal, Element = ElementType.Earth, Description = "再生脉冲-扩散治疗" }, // ← 新增

            // Thunder 系
            ["1711"] = new SkillDefinition { Name = "Blade Technique", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "我流秘刀法" }, // ← 新增
            ["1712"] = new SkillDefinition { Name = "Blade Technique", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "我流秘刀法2" }, // ← 新增
            ["1716"] = new SkillDefinition { Name = "Overdrive", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "超高出力" }, // ← 新增
            ["1720"] = new SkillDefinition { Name = "True Sight", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "心眼" }, // ← 新增
            ["1721"] = new SkillDefinition { Name = "Judgment Cut I", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "我流秘刀法·壹之型·改" }, // ← 新增
            ["1722"] = new SkillDefinition { Name = "Judgment Cut II", Type = SkillType.Damage, Element = ElementType.Thunder, Description = "我流秘刀法·贰之型·改" }, // ← 新增

            // Rock / Shield 系
            ["1905"] = new SkillDefinition { Name = "Dragon Slash", Type = SkillType.Damage, Element = ElementType.Earth, Description = "斩龙式" }, // ← 新增
            ["1906"] = new SkillDefinition { Name = "Backstep Sweep", Type = SkillType.Damage, Element = ElementType.Earth, Description = "后撤斩" }, // ← 新增
            ["1907"] = new SkillDefinition { Name = "Tectonic Ring", Type = SkillType.Damage, Element = ElementType.Earth, Description = "岩御·崩裂回环" }, // ← 新增
            ["1917"] = new SkillDefinition { Name = "Titan Charge", Type = SkillType.Damage, Element = ElementType.Earth, Description = "巨岩撞击" }, // ← 新增
            ["1922"] = new SkillDefinition { Name = "Shield Bash", Type = SkillType.Damage, Element = ElementType.Earth, Description = "护盾猛击" }, // ← 新增
            ["1925"] = new SkillDefinition { Name = "Rage Burst", Type = SkillType.Damage, Element = ElementType.Earth, Description = "怒爆" }, // ← 新增
            ["1926"] = new SkillDefinition { Name = "Sandgrip", Type = SkillType.Damage, Element = ElementType.Earth, Description = "砂岩之握" }, // ← 新增
            ["1928"] = new SkillDefinition { Name = "Jerrad Earh Guard", Type = SkillType.Heal, Element = ElementType.Earth, Description = "岩之守护" }, // ← 新增
            ["1929"] = new SkillDefinition { Name = "Unyielding Defense", Type = SkillType.Heal, Element = ElementType.Earth, Description = "不动如山" }, // ← 新增
            ["1936"] = new SkillDefinition { Name = "Stoneform", Type = SkillType.Heal, Element = ElementType.Earth, Description = "巨岩躯体" }, // ← 新增
            ["1938"] = new SkillDefinition { Name = "Brave Bastion", Type = SkillType.Heal, Element = ElementType.Earth, Description = "勇者壁垒" }, // ← 新增
            ["1941"] = new SkillDefinition { Name = "Starfall", Type = SkillType.Damage, Element = ElementType.Earth, Description = "碎星崩裂" }, // ← 新增
            ["1943"] = new SkillDefinition { Name = "Stone Fist", Type = SkillType.Damage, Element = ElementType.Earth, Description = "巨岩轰击" }, // ← 新增

            // 其他 (选摘)
            ["2220"] = new SkillDefinition { Name = "Storm Arrow", Type = SkillType.Damage, Element = ElementType.Wind, Description = "暴风箭矢" }, // ← 新增
            ["2221"] = new SkillDefinition { Name = "Storm Arrow", Type = SkillType.Damage, Element = ElementType.Wind, Description = "暴风箭矢SP" }, // ← 新增
            ["2230"] = new SkillDefinition { Name = "Torrent Volley", Type = SkillType.Damage, Element = ElementType.Earth, Description = "怒涛射击" }, // ← 新增
            ["2231"] = new SkillDefinition { Name = "Focus", Type = SkillType.Damage, Element = ElementType.Light, Description = "精神凝聚" }, // ← 新增
            ["2232"] = new SkillDefinition { Name = "Arrow Rain", Type = SkillType.Damage, Element = ElementType.Wind, Description = "箭雨" }, // ← 新增
            ["2234"] = new SkillDefinition { Name = "Radiance Barrage", Type = SkillType.Damage, Element = ElementType.Light, Description = "光能轰炸" }, // ← 新增
            ["2237"] = new SkillDefinition { Name = "Wildcall", Type = SkillType.Damage, Element = ElementType.Wind, Description = "狂野呼唤" }, // ← 新增
            ["2238"] = new SkillDefinition { Name = "Blast Shot", Type = SkillType.Damage, Element = ElementType.Fire, Description = "爆炸射击" }, // ← 新增
            ["1256"] = new SkillDefinition { Name = "Water Tornado Proxy (T)", Type = SkillType.Damage, Element = ElementType.Wind, Description = "浪潮" }, // ← 新增
                                                                                                                                     // ===== 2025-08-19 批量补齐：从 skill_names.json 导入的占位条目 =====
            ["1201"] = new SkillDefinition { Name = "Raincall Surge", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "雨打潮生" }, // ← 2025-08-19 新增（占位）
            ["1202"] = new SkillDefinition { Name = "Bois ATK_02 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "雨打潮生-转弯子弹" }, // ← 2025-08-19 新增（占位）
            ["1204"] = new SkillDefinition { Name = "Bois ATK_04 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "雨打潮生-普攻1段" }, // ← 2025-08-19 新增（占位）
            ["1210"] = new SkillDefinition { Name = "Maelstrom", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "水之涡流" }, // ← 2025-08-19 新增（占位）
            ["1211"] = new SkillDefinition { Name = "Crystal Veil", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "清滝绕珠" }, // ← 2025-08-19 新增（占位）
            ["1219"] = new SkillDefinition { Name = "Proxy Ice Tornado (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "冰龙卷" }, // ← 2025-08-19 新增（占位）
            ["1223"] = new SkillDefinition { Name = "Phantom Dash", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "幻影冲刺" }, // ← 2025-08-19 新增（占位）
            ["1238"] = new SkillDefinition { Name = "Tidal Vortex - Passive Skill", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "水龙卷-被动版" }, // ← 2025-08-19 新增（占位）
            ["1239"] = new SkillDefinition { Name = "Meteor Storm", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "陨星风暴" }, // ← 2025-08-19 新增（占位）
            ["1244"] = new SkillDefinition { Name = "Blizzard", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "寒冰风暴" }, // ← 2025-08-19 新增（占位）
            ["1251"] = new SkillDefinition { Name = "Bois_SKILL01_Proxy (Ice) (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "水之涡流" }, // ← 2025-08-19 新增（占位）
            ["1258"] = new SkillDefinition { Name = "Icy Bolt", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "天赋-冰晶落" }, // ← 2025-08-19 新增（占位）
            ["1725"] = new SkillDefinition { Name = "Thundercut", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "霹雳连斩2" }, // ← 2025-08-19 新增（占位）
            ["1726"] = new SkillDefinition { Name = "Thundercut", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "霹雳连斩3" }, // ← 2025-08-19 新增（占位）
            ["1730"] = new SkillDefinition { Name = "Volt Surge", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "无穷雷霆之力" }, // ← 2025-08-19 新增（占位）
            ["1731"] = new SkillDefinition { Name = "Stormflash", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "千雷闪影之意" }, // ← 2025-08-19 新增（占位）
            ["1733"] = new SkillDefinition { Name = "Storm Scythe", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "雷霆之镰" }, // ← 2025-08-19 新增（占位）
            ["1734"] = new SkillDefinition { Name = "Thunder Cut", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "雷霆居合斩" }, // ← 2025-08-19 新增（占位）
            ["1901"] = new SkillDefinition { Name = "Halberd's Edge", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "止战之锋1/岩弹" }, // ← 2025-08-19 新增（占位）
            ["1902"] = new SkillDefinition { Name = "Gerard ATK_02 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "止战之锋2/岩弹" }, // ← 2025-08-19 新增（占位）
            ["1903"] = new SkillDefinition { Name = "Gerard ATK_03 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "止战之锋3" }, // ← 2025-08-19 新增（占位）
            ["1904"] = new SkillDefinition { Name = "Gerard ATK_04 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "止战之锋4" }, // ← 2025-08-19 新增（占位）
            ["1909"] = new SkillDefinition { Name = "Gerard EX AIRATK (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "止战之锋（跳跃）" }, // ← 2025-08-19 新增（占位）
            ["1912"] = new SkillDefinition { Name = "Red light counter", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "止战之锋-红光反制" }, // ← 2025-08-19 新增（占位）
            ["1924"] = new SkillDefinition { Name = "Star Shatter", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "碎星冲" }, // ← 2025-08-19 新增（占位）
            ["1927"] = new SkillDefinition { Name = "Sandshroud", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "砂石斗篷（初始）" }, // ← 2025-08-19 新增（占位）
            ["1930"] = new SkillDefinition { Name = "Countercrush", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "格挡冲击" }, // ← 2025-08-19 新增（占位）
            ["1931"] = new SkillDefinition { Name = "Countercrush", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "格挡冲击" }, // ← 2025-08-19 新增（占位）
            ["1932"] = new SkillDefinition { Name = "Shield Combo", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "护盾猛击" }, // ← 2025-08-19 新增（占位）
            ["1933"] = new SkillDefinition { Name = "Halberd's Edge", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "止战之锋" }, // ← 2025-08-19 新增（占位）
            ["1934"] = new SkillDefinition { Name = "Countercrush", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "格挡冲击" }, // ← 2025-08-19 新增（占位）
            ["1935"] = new SkillDefinition { Name = "Rageblow", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "格挡冲击-怒击" }, // ← 2025-08-19 新增（占位）
            ["1937"] = new SkillDefinition { Name = "Granite Fury", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "岩怒之击" }, // ← 2025-08-19 新增（占位）
            ["1939"] = new SkillDefinition { Name = "Granite Fury", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "岩怒之击" }, // ← 2025-08-19 新增（占位）
            ["1940"] = new SkillDefinition { Name = "Rage Burst", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "怒爆" }, // ← 2025-08-19 新增（占位）
            ["1942"] = new SkillDefinition { Name = "Rupture", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "崩裂" }, // ← 2025-08-19 新增（占位）
            ["1944"] = new SkillDefinition { Name = "Rage Burst", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "怒爆" }, // ← 2025-08-19 新增（占位）
            ["1999"] = new SkillDefinition { Name = "Power of Rock", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "岩之力" }, // ← 2025-08-19 新增（占位）
            ["2201"] = new SkillDefinition { Name = "Bullseye", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "弹无虚发" }, // ← 2025-08-19 新增（占位）
            ["2209"] = new SkillDefinition { Name = "Luminary Bolt", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "锐眼·光能巨箭" }, // ← 2025-08-19 新增（占位）
            ["2222"] = new SkillDefinition { Name = "Double Arrow", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "二连矢" }, // ← 2025-08-19 新增（占位）
            ["2224"] = new SkillDefinition { Name = "Quadraflare", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "夺命射击-四连矢" }, // ← 2025-08-19 新增（占位）
            ["2235"] = new SkillDefinition { Name = "Deter Shot", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "威慑射击" }, // ← 2025-08-19 新增（占位）
            ["2239"] = new SkillDefinition { Name = "Explosive Arrow II", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "爆炸箭二段" }, // ← 2025-08-19 新增（占位）
            ["2240"] = new SkillDefinition { Name = "Lumi Torrent", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "天翔贯星击-强化怒涛射击" }, // ← 2025-08-19 新增（占位）
            ["2241"] = new SkillDefinition { Name = "Celestial Eagle", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "天界雄鹰" }, // ← 2025-08-19 新增（占位）
            ["2242"] = new SkillDefinition { Name = "Beast Stampede", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "群兽践踏" }, // ← 2025-08-19 新增（占位）
            ["2290"] = new SkillDefinition { Name = "Rotroiani – Bow – Gather Proxy (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "锐眼·光能巨箭-聚怪(天赋)" }, // ← 2025-08-19 新增（占位）
            ["2293"] = new SkillDefinition { Name = "Talent: Photon Crack", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "天赋-光能裂痕" }, // ← 2025-08-19 新增（占位）
            ["2296"] = new SkillDefinition { Name = "Phantom Falcon", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "幻影雄鹰" }, // ← 2025-08-19 新增（占位）
            ["2307"] = new SkillDefinition { Name = "Healing Beat", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "愈合节拍" }, // ← 2025-08-19 新增（占位）
            ["2308"] = new SkillDefinition { Name = "Harmonic Anthem", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "聚合乐章" }, // ← 2025-08-19 新增（占位）
            ["2309"] = new SkillDefinition { Name = "Rhapsody of Flame", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "烈焰狂想" }, // ← 2025-08-19 新增（占位）
            ["2310"] = new SkillDefinition { Name = "Heroic Melody", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "鸣奏·英勇乐章" }, // ← 2025-08-19 新增（占位）
            ["2311"] = new SkillDefinition { Name = "Healing Melody", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "鸣奏·愈合乐章" }, // ← 2025-08-19 新增（占位）
            ["2314"] = new SkillDefinition { Name = "Rock the Stage/Infinite Rhapsody", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "升格·劲爆全场" }, // ← 2025-08-19 新增（占位）
            ["2315"] = new SkillDefinition { Name = "Encore", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "安可" }, // ← 2025-08-19 新增（占位）
            ["2316"] = new SkillDefinition { Name = "Center Stage", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "万众瞩目" }, // ← 2025-08-19 新增（占位）
            ["2318"] = new SkillDefinition { Name = "Finale! Healing Movement", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "完结！愈合乐章" }, // ← 2025-08-19 新增（占位）
            ["2319"] = new SkillDefinition { Name = "Sound Wave Surge", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "音浪潮涌" }, // ← 2025-08-19 新增（占位）
            ["2320"] = new SkillDefinition { Name = "Flame Shock", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "音塔爆炎冲击" }, // ← 2025-08-19 新增（占位）
            ["2329"] = new SkillDefinition { Name = "Pillar of Flame", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "炎舞奏者" }, // ← 2025-08-19 新增（占位）
            ["2330"] = new SkillDefinition { Name = "Soul Musician – Flame Dancer Empowerment (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "火柱冲击-炎舞奏者强化" }, // ← 2025-08-19 新增（占位）
            ["2331"] = new SkillDefinition { Name = "Scorching Impact", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "音浪烈焰" }, // ← 2025-08-19 新增（占位）
            ["2332"] = new SkillDefinition { Name = "Passion Fury", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "激昂·热情挥洒" }, // ← 2025-08-19 新增（占位）
            ["2335"] = new SkillDefinition { Name = "Infinite Rhapsody", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "升格·无限狂想" }, // ← 2025-08-19 新增（占位）
            ["2352"] = new SkillDefinition { Name = "Celestial Eagle - Eagle", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "天界雄鹰" }, // ← 2025-08-19 新增（占位）
            ["2361"] = new SkillDefinition { Name = "Healing Beat copy", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "愈合节拍copy" }, // ← 2025-08-19 新增（占位）
            ["2362"] = new SkillDefinition { Name = "Healing Beat copy", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "音波裁决" }, // ← 2025-08-19 新增（占位）
            ["2363"] = new SkillDefinition { Name = "Healing Beat copy", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "激涌五重奏copy" }, // ← 2025-08-19 新增（占位）
            ["2364"] = new SkillDefinition { Name = "Rock the Stage copy", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "升格·劲爆全场copy" }, // ← 2025-08-19 新增（占位）
            ["2365"] = new SkillDefinition { Name = "Infinite Rhapsody copy", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "升格·无限狂想copy" }, // ← 2025-08-19 新增（占位）
            ["2399"] = new SkillDefinition { Name = "Sound Healing Staff", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "音响奶棍" }, // ← 2025-08-19 新增（占位）
            ["2408"] = new SkillDefinition { Name = "Shield Toss", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "投掷盾牌" }, // ← 2025-08-19 新增（占位）
            ["2409"] = new SkillDefinition { Name = "Divine Circle", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "圣环" }, // ← 2025-08-19 新增（占位）
            ["2411"] = new SkillDefinition { Name = "Scorching Judgment", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "灼热裁决" }, // ← 2025-08-19 新增（占位）
            ["2414"] = new SkillDefinition { Name = "Holy Barrier", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "神圣壁垒" }, // ← 2025-08-19 新增（占位）
            ["2415"] = new SkillDefinition { Name = "Aegis Ward", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "圣光守卫" }, // ← 2025-08-19 新增（占位）
            ["2417"] = new SkillDefinition { Name = "Enhanced Condemn", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "强化断罪" }, // ← 2025-08-19 新增（占位）
            ["2419"] = new SkillDefinition { Name = "Zeal Crusade", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "冷酷征伐" }, // ← 2025-08-19 新增（占位）
            ["2420"] = new SkillDefinition { Name = "Radiance", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "光明决心" }, // ← 2025-08-19 新增（占位）
            ["2425"] = new SkillDefinition { Name = "Shield Toss", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "投掷盾牌" }, // ← 2025-08-19 新增（占位）
            ["2452"] = new SkillDefinition { Name = "Scorching Judgment Sacred Blade", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "灼热裁决圣剑" }, // ← 2025-08-19 新增（占位）
            ["3698"] = new SkillDefinition { Name = "Lucky Meteor Proxy – Active (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "风哥布林王(被动)" }, // ← 2025-08-19 新增（占位）
            ["3901"] = new SkillDefinition { Name = "Arcane! Flame Roar", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！琉火咆哮(火魔)" }, // ← 2025-08-19 新增（占位）
            ["3925"] = new SkillDefinition { Name = "Stunt! Invisible Impact", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技！无形冲击(巨魔)" }, // ← 2025-08-19 新增（占位）
            ["21418"] = new SkillDefinition { Name = "Healing Effect (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "鹿之奔袭" }, // ← 2025-08-19 新增（占位）
            ["21427"] = new SkillDefinition { Name = "Basic Attack Hit 1 Heal (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "惩击奶空A1段" }, // ← 2025-08-19 新增（占位）
            ["21428"] = new SkillDefinition { Name = "Basic Attack Hit 2 Heal (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "惩击奶空A2段" }, // ← 2025-08-19 新增（占位）
            ["21429"] = new SkillDefinition { Name = "Basic Attack Hit 3 Heal (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "惩击奶空A3段" }, // ← 2025-08-19 新增（占位）
            ["21430"] = new SkillDefinition { Name = "Basic Attack Hit 4 Heal (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "惩击奶空A4段" }, // ← 2025-08-19 新增（占位）
            ["27009"] = new SkillDefinition { Name = "Icebox Buff (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "冰箱BUFF" }, // ← 2025-08-19 新增（占位）
            ["50036"] = new SkillDefinition { Name = "Weak Point Strike (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "弱点打击" }, // ← 2025-08-19 新增（占位）
            ["50037"] = new SkillDefinition { Name = "Block Counter (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "格挡反击" }, // ← 2025-08-19 新增（占位）
            ["50049"] = new SkillDefinition { Name = "Sand Cloak (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "砂石斗篷（持续）" }, // ← 2025-08-19 新增（占位）
            ["50068"] = new SkillDefinition { Name = "Block Counter — Strong (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "格挡反击--强" }, // ← 2025-08-19 新增（占位）
            ["55231"] = new SkillDefinition { Name = "Explosive Arrow BUFF", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "爆炸箭BUFF引爆" }, // ← 2025-08-19 新增（占位）
            ["55235"] = new SkillDefinition { Name = "Special attack enhanced: small arrow AOE.", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "光意·四连矢（溅射伤害）" }, // ← 2025-08-19 新增（占位）
            ["55236"] = new SkillDefinition { Name = "Enhanced special attack final hit: virtual body", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "强化特攻最后一击虚拟体" }, // ← 2025-08-19 新增（占位）
            ["55238"] = new SkillDefinition { Name = "Bow – Ultimate Gravity (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "弓-大招引力" }, // ← 2025-08-19 新增（占位）
            ["55239"] = new SkillDefinition { Name = "Light Energy Stasis", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "光能凝滞定身BUFF" }, // ← 2025-08-19 新增（占位）
            ["55240"] = new SkillDefinition { Name = "Photon Bombardment Damage Area (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "光能轰炸伤害区域BUFF" }, // ← 2025-08-19 新增（占位）
            ["55328"] = new SkillDefinition { Name = "Center Stage – Surging Quintet Play Count Doubled (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "万众瞩目激涌五重奏弹奏翻倍" }, // ← 2025-08-19 新增（占位）
            ["55335"] = new SkillDefinition { Name = "Center Stage – Passionate Flourish Stage 3 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "万众瞩目热情挥洒3阶段" }, // ← 2025-08-19 新增（占位）
            ["55344"] = new SkillDefinition { Name = "Ultimate – Base Healing (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "升格·劲爆全场" }, // ← 2025-08-19 新增（占位）
            ["55417"] = new SkillDefinition { Name = "Zeal Crusade", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "冷酷征伐" }, // ← 2025-08-19 新增（占位）
            ["55431"] = new SkillDefinition { Name = "Zeal Crusade DMG", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "冷酷征伐伤害buff" }, // ← 2025-08-19 新增（占位）
            ["55432"] = new SkillDefinition { Name = "Zeal Crusade DMG Boost", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "冷冷酷征伐伤害buff" }, // ← 2025-08-19 新增（占位）
            ["100730"] = new SkillDefinition { Name = "Resonance – Frenzied Shooting (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "哥布林弩手主动" }, // ← 2025-08-19 新增（占位）
            ["102640"] = new SkillDefinition { Name = "Stunt! Boarrier Rush", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技！猪突猛进(威猛野猪)" }, // ← 2025-08-19 新增（占位）
            ["101112"] = new SkillDefinition { Name = "Falcon Dive", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "宠物雄鹰快速回旋" }, // ← 2025-08-19 新增（占位）
            ["141104"] = new SkillDefinition { Name = "Wasp Melee Stab (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "变异蜂主动a1" }, // ← 2025-08-19 新增（占位）
            ["149904"] = new SkillDefinition { Name = "Tina – Instant Tornado (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "蒂娜龙卷" }, // ← 2025-08-19 新增（占位）
            ["179904"] = new SkillDefinition { Name = "Olvera – Flying Thunder God (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "神影斩-最后一击" }, // ← 2025-08-19 新增（占位）
            ["199903"] = new SkillDefinition { Name = "Stone Fist", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "巨岩轰击" }, // ← 2025-08-19 新增（占位）
            ["220105"] = new SkillDefinition { Name = "Light-Chasing Arrow (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "光追箭" }, // ← 2025-08-19 新增（占位）
            ["220106"] = new SkillDefinition { Name = "Aerial Shot (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "空中射击" }, // ← 2025-08-19 新增（占位）
            ["220107"] = new SkillDefinition { Name = "Magic Arrows (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "魔法箭矢" }, // ← 2025-08-19 新增（占位）
            ["220110"] = new SkillDefinition { Name = "Explosive Arrow (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "爆炸箭" }, // ← 2025-08-19 新增（占位）
            ["221101"] = new SkillDefinition { Name = "Every Shot Hits – Red Light Counter (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "弹无虚发-红光反制" }, // ← 2025-08-19 新增（占位）
            ["230106"] = new SkillDefinition { Name = "Flame Note", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "烈焰音符伤害" }, // ← 2025-08-19 新增（占位）
            ["391001"] = new SkillDefinition { Name = "Finisher! Chaotic Missiles (Void-Erosion Ogre) 1 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技! 纷乱飞弹(虚蚀食人魔)1" }, // ← 2025-08-19 新增（占位）
            ["391002"] = new SkillDefinition { Name = "Finisher! Chaotic Missiles (Void-Erosion Ogre) 2 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技! 纷乱飞弹(虚蚀食人魔)2" }, // ← 2025-08-19 新增（占位）
            ["391003"] = new SkillDefinition { Name = "Finisher! Chaotic Missiles (Void-Erosion Ogre) 3 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技! 纷乱飞弹(虚蚀食人魔)3" }, // ← 2025-08-19 新增（占位）
            ["391004"] = new SkillDefinition { Name = "Finisher! Chaotic Missiles (Void-Erosion Ogre) 4 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技! 纷乱飞弹(虚蚀食人魔)4" }, // ← 2025-08-19 新增（占位）
            ["391005"] = new SkillDefinition { Name = "Finisher! Chaotic Missiles (Void-Erosion Ogre) 5 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技! 纷乱飞弹(虚蚀食人魔)5" }, // ← 2025-08-19 新增（占位）
            ["391008"] = new SkillDefinition { Name = "Mutant Bee – Passive (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "变异蜂被动" }, // ← 2025-08-19 新增（占位）
            ["391401"] = new SkillDefinition { Name = "Void-Erosion Pulse (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "虚蚀脉冲" }, // ← 2025-08-19 新增（占位）
            ["701001"] = new SkillDefinition { Name = "Device Spawn – Innate Buff (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "虚蚀之影爆炸" }, // ← 2025-08-19 新增（占位）
            ["701002"] = new SkillDefinition { Name = "Device Void-Erosion Explosion Show 01 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "虚蚀波动爆炸" }, // ← 2025-08-19 新增（占位）
            ["1002440"] = new SkillDefinition { Name = "Stunt! Super Critical", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技！超会心(姆克兵长)" }, // ← 2025-08-19 新增（占位）
            ["1002830"] = new SkillDefinition { Name = "Arcane! Frostquake", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！冰霜震荡(冰魔)" }, // ← 2025-08-19 新增（占位）
            ["1005940"] = new SkillDefinition { Name = "Muku Vanguard - Whirlwind Charge", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "姆克狂战士-旋风冲锋" }, // ← 2025-08-19 新增（占位）
            ["1007601"] = new SkillDefinition { Name = "Void Bzzar Basic Attack", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "变异蜂技能12" }, // ← 2025-08-19 新增（占位）
            ["1007602"] = new SkillDefinition { Name = "Void Bzzar - Big Explosion", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "变异蜂技能3" }, // ← 2025-08-19 新增（占位）
            ["1007741"] = new SkillDefinition { Name = "Arcane! Poison Explosion Avatar", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "剧毒蜂巢（初始）" }, // ← 2025-08-19 新增（占位）
            ["1008040"] = new SkillDefinition { Name = "Stunt! Thunder Ball", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技! 雷光球(雷光野猪)" }, // ← 2025-08-19 新增（占位）
            ["1008140"] = new SkillDefinition { Name = "Ultimate - Hell Stab", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！地狱突刺(铁牙)" }, // ← 2025-08-19 新增（占位）
            ["1008540"] = new SkillDefinition { Name = "Arcane! Silent Tide", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！静默潮汐(蜥蜴人王)" }, // ← 2025-08-19 新增（占位）
            ["1010440"] = new SkillDefinition { Name = "Stunt! Thunder Suppress", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技！强压之雷(蜥蜴人猎手)(主动)" }, // ← 2025-08-19 新增（占位）
            ["1700440"] = new SkillDefinition { Name = "Arcane! Furious Hammer", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！重锤狂袭（姆头）" }, // ← 2025-08-19 新增（占位）
            ["1700824"] = new SkillDefinition { Name = "Wild Wolf - Tail Sweep", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "甩尾" }, // ← 2025-08-19 新增（占位）
            ["1700825"] = new SkillDefinition { Name = "Companion - Foxen Pounce", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "狼突击" }, // ← 2025-08-19 新增（占位）
            ["1700826"] = new SkillDefinition { Name = "Wild Wolf Pounce", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "狂野召唤" }, // ← 2025-08-19 新增（占位）
            ["2001740"] = new SkillDefinition { Name = "Stunt! Blink Ambush", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技！瞬步奇袭（山贼斥候）" }, // ← 2025-08-19 新增（占位）
            ["2002853"] = new SkillDefinition { Name = "Meteor (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技！碎星陨落（火哥）" }, // ← 2025-08-19 新增（占位）
            ["2031106"] = new SkillDefinition { Name = "Hand Cannon - Luck", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "幸运一击(手炮)" }, // ← 2025-08-19 新增（占位）
            ["2031107"] = new SkillDefinition { Name = "Guardblade - Luck", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "幸运一击(巨刃)" }, // ← 2025-08-19 新增（占位）
            ["2031108"] = new SkillDefinition { Name = "Ceremonial Blade - Luck", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "幸运一击(仪刀)" }, // ← 2025-08-19 新增（占位）
            ["2110012"] = new SkillDefinition { Name = "hot", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！脉冲祝祷（姆克王）" }, // ← 2025-08-19 新增（占位）
            ["2110066"] = new SkillDefinition { Name = "Earth Shield", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技！大地之盾(山贼护卫队长)" }, // ← 2025-08-19 新增（占位）
            ["2110083"] = new SkillDefinition { Name = "Glazefire’s Roar – Healing (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "火魔治疗" }, // ← 2025-08-19 新增（占位）
            ["2110085"] = new SkillDefinition { Name = "Seed of Thunder (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "雷电之种(奥尔维拉)" }, // ← 2025-08-19 新增（占位）
            ["2110090"] = new SkillDefinition { Name = "Void Corruption Decay", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "虚蚀食人魔" }, // ← 2025-08-19 新增（占位）
            ["2110091"] = new SkillDefinition { Name = "Convert damage received into healing", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "虚蚀伤害" }, // ← 2025-08-19 新增（占位）
            ["2110096"] = new SkillDefinition { Name = "Electro Shield", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！雷霆咆哮(金牙)(雷击)" }, // ← 2025-08-19 新增（占位）
            ["2110099"] = new SkillDefinition { Name = "Toxic Hive – DoT Vulnerability (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "剧毒蜂巢（持续）" }, // ← 2025-08-19 新增（占位）
            ["2201070"] = new SkillDefinition { Name = "Block Damage Reduction (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "格挡减伤" }, // ← 2025-08-19 新增（占位）
            ["2201080"] = new SkillDefinition { Name = "Block Recovery (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "格挡回复" }, // ← 2025-08-19 新增（占位）
            ["2201172"] = new SkillDefinition { Name = "Shatter All Shields & Deal Damage", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "坚毅之息" }, // ← 2025-08-19 新增（占位）
            ["2201240"] = new SkillDefinition { Name = "Shield Echo (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "护盾回声" }, // ← 2025-08-19 新增（占位）
            ["2201362"] = new SkillDefinition { Name = "Sand Crystal Shock", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "沙晶石震荡" }, // ← 2025-08-19 新增（占位）
            ["2201410"] = new SkillDefinition { Name = "Sandstone Resurgence (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "砂石复苏" }, // ← 2025-08-19 新增（占位）
            ["2201493"] = new SkillDefinition { Name = "Recovery", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "回复（岩盾）" }, // ← 2025-08-19 新增（占位）
            ["2201570"] = new SkillDefinition { Name = "Rockguard (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "岩护" }, // ← 2025-08-19 新增（占位）
            ["2202120"] = new SkillDefinition { Name = "Nature Shield", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "艾露娜护盾" }, // ← 2025-08-19 新增（占位）
            ["2202211"] = new SkillDefinition { Name = "Arcane of Green (Healing)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绿意之爆发（治疗）" }, // ← 2025-08-19 新增（占位）
            ["2202262"] = new SkillDefinition { Name = "Wings of Revival", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "复苏光环回血BUFF" }, // ← 2025-08-19 新增（占位）
            ["2202271"] = new SkillDefinition { Name = "Heavenly Light – Applied Buff (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "天降圣光生效BUFF" }, // ← 2025-08-19 新增（占位）
            ["2202291"] = new SkillDefinition { Name = "Life Gift - Healing Sub-buff", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "生命馈赠-治疗子buff" }, // ← 2025-08-19 新增（占位）
            ["2002440"] = new SkillDefinition { Name = "Arcane! Thunderfall Grasp", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！雷霆天牢引（雷魔）" }, // ← 2025-08-19 新增（占位）
            ["2202581"] = new SkillDefinition { Name = "Collapse! boom~", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "坍缩！boom~" }, // ← 2025-08-19 新增（占位）
            ["2202582"] = new SkillDefinition { Name = "Collapse! boom~", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "坍缩！boom~" }, // ← 2025-08-19 新增（占位）
            ["2203091"] = new SkillDefinition { Name = "Bleed", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "生命流失（扑咬引爆）" }, // ← 2025-08-19 新增（占位）
            ["2203101"] = new SkillDefinition { Name = "Demon Wolf Bite – Life Loss (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "生命流失" }, // ← 2025-08-19 新增（占位）
            ["2203102"] = new SkillDefinition { Name = "Bleed", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "生命流失（协同攻击引爆）" }, // ← 2025-08-19 新增（占位）
            ["2203141"] = new SkillDefinition { Name = "Bleed", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "生命流失（光追箭引爆）" }, // ← 2025-08-19 新增（占位）
            ["2203302"] = new SkillDefinition { Name = "Life Loss – Sub-Buff 2 (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "生命流失（扫尾引爆）" }, // ← 2025-08-19 新增（占位）
            ["2203311"] = new SkillDefinition { Name = "Explosive Arrow – Sub-Buff (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "爆炸箭矢（溅射）" }, // ← 2025-08-19 新增（占位）
            ["2204320"] = new SkillDefinition { Name = "Icy Pulse (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "冰冷脉冲" }, // ← 2025-08-19 新增（占位）
            ["2406140"] = new SkillDefinition { Name = "(Unique) [Set 4A] Triggers AoE DMG when gaining a shield", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "获得护盾时造成aoe（套装）" }, // ← 2025-08-19 新增（占位）
            ["2206240"] = new SkillDefinition { Name = "Holy Radiance", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "神圣光辉" }, // ← 2025-08-19 新增（占位）
            ["2207500"] = new SkillDefinition { Name = "Essence: Center Stage (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "极意万众瞩目" }, // ← 2025-08-19 新增（占位）
            ["2207660"] = new SkillDefinition { Name = "Essence: Center Stage (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "极意万众瞩目" }, // ← 2025-08-19 新增（占位）
            ["2207681"] = new SkillDefinition { Name = "Spiral Performance", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "螺旋演奏" }, // ← 2025-08-19 新增（占位）
            ["2900540"] = new SkillDefinition { Name = "Arcane! Flash Execution", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！瞬即斩(奥尔维拉)" }, // ← 2025-08-19 新增（占位）
            ["3001031"] = new SkillDefinition { Name = "Void Aura", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "虚蚀光环" }, // ← 2025-08-19 新增（占位）
            ["3001170"] = new SkillDefinition { Name = "Void-Erosion Wave (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "虚蚀波动回血" }, // ← 2025-08-19 新增（占位）
            ["3081023"] = new SkillDefinition { Name = "Stunt! Heavy Slash", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技! 超重斩(黯影剑士队长)" }, // ← 2025-08-19 新增（占位）
            ["3210021"] = new SkillDefinition { Name = "Wind Goblin King – Active Trigger (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "奥义！流星陨落(风哥)(主动)" }, // ← 2025-08-19 新增（占位）
            ["3210031"] = new SkillDefinition { Name = "Thunder Ogre – Passive Trigger (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "雷魔被动" }, // ← 2025-08-19 新增（占位）
            ["3210051"] = new SkillDefinition { Name = "Bandit Chief – Passive – Sub-Buff DoT (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "山贼被动" }, // ← 2025-08-19 新增（占位）
            ["3210061"] = new SkillDefinition { Name = "Muk Captain – Passive Trigger (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "姆克兵长被动" }, // ← 2025-08-19 新增（占位）
            ["3210092"] = new SkillDefinition { Name = "Lizardman King – Tidal Power Trigger (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "蜥蜴人王被动" }, // ← 2025-08-19 新增（占位）
            ["3210101"] = new SkillDefinition { Name = "Muku's Protection", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "姆克王-护盾" }, // ← 2025-08-19 新增（占位）
            ["3936001"] = new SkillDefinition { Name = "Murloc Mage Silence Pool", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "绝技！静默之水(蜥蜴人法师)" }, // ← 2025-08-19 新增（占位）
            ["10040102"] = new SkillDefinition { Name = "Sword & Shield Goblin – Wind Blade Slash Resonance (T)", Type = SkillType.Unknown, Element = ElementType.Unknown, Description = "剑盾哥布林-风刃斩共鸣" }, // ← 2025-08-19 新增（占位）



        };

        public static readonly Dictionary<int, SkillDefinition> SkillsByInt = new();

        static EmbeddedSkillConfig()
        {
            foreach (var kv in SkillsByString)
            {
                if (int.TryParse(kv.Key, out var id))
                    SkillsByInt[id] = kv.Value;
            }
        }

        public static bool TryGet(string id, out SkillDefinition def) => SkillsByString.TryGetValue(id, out def!);
        public static bool TryGet(int id, out SkillDefinition def) => SkillsByInt.TryGetValue(id, out def!);

        public static string GetName(string id) => TryGet(id, out var d) ? d.Name : id;
        public static string GetName(int id) => TryGet(id, out var d) ? d.Name : id.ToString();

        public static SkillType GetTypeOf(string id) => TryGet(id, out var d) ? d.Type : SkillType.Unknown;
        public static SkillType GetTypeOf(int id) => TryGet(id, out var d) ? d.Type : SkillType.Unknown;

        public static ElementType GetElementOf(string id) => TryGet(id, out var d) ? d.Element : ElementType.Unknown;
        public static ElementType GetElementOf(int id) => TryGet(id, out var d) ? d.Element : ElementType.Unknown;

        public static IReadOnlyDictionary<string, SkillDefinition> AllByString => SkillsByString;
        public static IReadOnlyDictionary<int, SkillDefinition> AllByInt => SkillsByInt;
    }
}
