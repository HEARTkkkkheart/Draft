using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IBuffComponent
{
    // 当Effect被附加到目标时触发（初始化）
    void OnApply(GameObject target,GameObject user);

    // 当Effect触发时调用（根据机制类型在不同时机触发）
    void OnTrigger(GameObject target,GameObject user);

    // 当Effect被移除时触发（清理资源）
    void OnRemove(GameObject target,GameObject user);
}

public class Buff : IBuffComponent
{
    public E_BuffType BuffType;
    public E_MechanismType MechanismType;
    private List<string> _infoList = new List<string>();

    public void OnApply(GameObject target,GameObject user)
    {
    }

    public void OnTrigger(GameObject target,GameObject user)
    {
    }

    public void OnRemove(GameObject target,GameObject user)
    {
    }
}

public class Card
{
    // 基础信息
    public string Name;

    /// <summary>
    /// 用二进制表示这张卡的功能  比如100010说明拥有两种效果
    /// </summary>
    public int Type = 0;

    public GameObject Target;
    public GameObject Caster;

    // 组件化效果（核心解耦点）
    public List<Buff> Components;
}