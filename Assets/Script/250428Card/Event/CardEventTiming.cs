using System;
using Framework;
using UnityEngine;

interface IEventTriggerTiming
{
    void RoundStart(RoundStartInfo info);
}

public abstract class CardEventTiming : MonoBehaviour, IEventTriggerTiming
{
    protected virtual void OnEnable()
    {
        Register(); // 自动注册
    }

    private void Register()
    {
        EventCenter.Instance.Register<RoundStartInfo>(RoundStart).UnRegisterWhenGameObjectOnDestroy(gameObject);
    }

    public abstract void RoundStart(RoundStartInfo info);
}

public enum E_TriggerTiming
{
    // 回合阶段
    RoundStart = 101, // 回合开始时
    RoundEnd = 102, // 回合结束时

    // 战斗事件
    BeforeReceiveDamage = 201, // 即将受到伤害时
    AfterReceiveDamage = 202, // 受到伤害后
    BeforeDealDamage = 203, // 造成伤害前
    AfterDealDamage = 204, // 造成伤害后

    // 生命周期
    UnitSpawn = 301, // 单位生成时
    UnitDeath = 302, // 单位死亡时

    // 状态变化
    HpBelowThreshold = 401, // 生命低于阈值时
    ResourceFull = 402, // 资源回满时

    // 特殊时点
    // EveryTurn = 501,      // 每回合固定触发（可与持续时间配合）
    Immediate = 502 // 立即触发（用于瞬发类效果）
}

public struct RoundStartInfo : IEvent
{
}

struct RoundEndInfo : IEvent
{
}

struct BeforeReceiveDamageInfo : IEvent
{
}

struct AfterReceiveDamageInfo : IEvent
{
}

struct BeforeDealDamageInfo : IEvent
{
}

struct AfterDealDamageInfo : IEvent
{
}

struct UnitSpawnInfo : IEvent
{
}

struct UnitDeathInfo : IEvent
{
}

struct HpBelowThresholdInfo : IEvent
{
}

struct ResourceFullInfo : IEvent
{
}

struct ImmediateInfo : IEvent
{
}