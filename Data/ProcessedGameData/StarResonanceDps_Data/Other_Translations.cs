// Common.cs
public static string GetSubProfessionBySkillId(ulong skillId) =>
    skillId switch
    {
        1241 => "Frostbeam Spec",

        2307 or 2361 or 55302 => "Concerto Spec",

        20301 => "Lifebind Spec",

        1518 or 1541 or 21402 => "Smite Spec",

        2306 => "Dissonance Spec",

        120901 or 120902 => "Icicle Spec",

        1714 or 1734 => "Iaido Slash Spec",

        44701 or 179906 => "Moonstrike Spec",

        220112 or 2203622 or 220106  => "Falconry Spec",

        2292 or 1700820 or 1700825 or 1700827 => "Wildpack Spec",

        1419 => "Skyward Spec",

        1405 or 1418 => "Vanguard Spec",

        2405 => "Shield Spec",
        2406 => "Recovery Spec",
        199902 => "Earthfort Spec",

        1930 or 1931 or 1934 or 1935 => "Block Spec",

        _ => string.Empty
    };