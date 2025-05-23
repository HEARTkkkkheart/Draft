using System.Collections.Generic;

public class GameConfigFiles
{
    public static readonly Dictionary<string, string> GameConfigFilesDic = new Dictionary<string, string>()
    {
        ["cfg_Card"] = "cfg_Card.csv",
        ["cfg_Buff"] = "cfg_Buff.csv",
    };
}

public enum E_BuffType
{
    Damage = 1,
    Heal = 2,
    StatModifier = 3,
    ResourceChange = 4,
    StatusEffect = 5
}

/// <summary>
/// buff的基础数据类型
/// </summary>
public class BuffData
{
    public int Id;
    public string Name;
    public E_BuffType Type;
    public E_TriggerTiming TriggerTiming;
}

/// <summary>
/// 卡牌的基础数据类型
/// </summary>
public class CardData
{
    public string Name;
    public int Cost;
    public List<int> ExecutionSeq = new List<int>();
    public List<int> ValueList = new List<int>();
}