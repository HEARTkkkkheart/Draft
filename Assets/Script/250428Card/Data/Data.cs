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

// public sealed class GameData
// {
//     private static readonly GameData _instance = new GameData();
//     public static GameData Instance => _instance;
//
//     public List<BuffData> BuffDataList = new List<BuffData>();
//
//     public List<CardData> CardDataList = new List<CardData>();
// }

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

// /// <summary>
// /// 作用机制
// /// 1-立即生效
// /// 2-持续生效
// /// 3-条件触发
// /// 4-全局规则修改
// /// 5-影响其他Buff
// /// </summary>
// public enum E_MechanismType
// {
//     Instant = 1,
//     Duration = 2,
//     Conditional = 3,
//     Global = 4,
//     Meta = 5
// }

public enum E_TriggerTiming
{
    // 回合阶段
    RoundStart = 101,     // 回合开始时
    RoundEnd = 102,       // 回合结束时
    
    // 战斗事件
    BeforeReceiveDamage = 201,  // 即将受到伤害时
    AfterReceiveDamage = 202,   // 受到伤害后
    BeforeDealDamage = 203,     // 造成伤害前
    AfterDealDamage = 204,      // 造成伤害后
    
    // 生命周期
    UnitSpawn = 301,      // 单位生成时
    UnitDeath = 302,      // 单位死亡时
    
    // 状态变化
    HpBelowThreshold = 401,     // 生命低于阈值时
    ResourceFull = 402,         // 资源回满时
    
    // 特殊时点
    EveryTurn = 501,      // 每回合固定触发（可与持续时间配合）
    Immediate = 502       // 立即触发（用于瞬发类效果）
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