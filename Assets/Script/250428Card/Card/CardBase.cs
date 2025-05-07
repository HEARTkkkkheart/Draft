using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBuffComponent
{
    // 当Effect被附加到目标时触发（初始化）
    void OnApply(GameObject target, GameObject user);

    // 当Effect触发时调用（根据机制类型在不同时机触发）
    void OnTrigger(GameObject target, GameObject user);

    // 当Effect被移除时触发（清理资源）
    void OnRemove(GameObject target, GameObject user);
}

public class Buff
{
    public int Id;
    public string Name;
    public E_BuffType BuffType;
    public E_TriggerTiming MechanismType;
    public int Value;

    public void OnApply(GameObject target, GameObject user)
    {
    }

    public void OnTrigger(GameObject target, GameObject user)
    {
    }

    public void OnRemove(GameObject target, GameObject user)
    {
    }
}

public class Card
{
    // 基础信息
    public string Name;
    public int Cost;

    public GameObject Target;
    public GameObject Caster;

    // 组件化效果（核心解耦点）
    public List<Buff> Buffs;
}