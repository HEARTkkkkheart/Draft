using System.Collections.Generic;

/// <summary>
/// 配置表路径
/// </summary>
public class GameConfigFiles
{
    public static readonly Dictionary<string, string> GameConfigFilesDic = new Dictionary<string, string>()
    {
        ["cfg_Card"] = "cfg_Card.csv",
        ["cfg_Buff"] = "cfg_Buff.csv",
    };
}

/// <summary>
/// 类型效果
/// 1-伤害
/// 2-治疗
/// 3-属性修改（攻击/防御等）
/// 4-资源相关
/// 5-特殊状态（眩晕、沉默）
/// </summary>
public enum E_BuffType
{
    Damage = 1,
    Heal = 2,
    StatModifier = 3,
    ResourceChange = 4,
    StatusEffect = 5
}

/// <summary>
/// 作用机制
/// 1-立即生效
/// 2-持续生效
/// 3-条件触发
/// 4-全局规则修改
/// 5-影响其他Buff
/// </summary>
public enum E_MechanismType
{
    Instant = 1,
    Duration = 2,
    Conditional = 3,
    Global = 4,
    Meta = 5
}

/// <summary>
/// buff的基础数据类型
/// </summary>
public class BuffData
{
    public int Id;
    public string Name;
    public E_BuffType Type;
    public E_MechanismType Mechanism;
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