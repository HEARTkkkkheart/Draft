using System.Collections.Generic;
using UnityEngine;

public class Attack1 : IBuffComponent
{
    private int _name = 0;
    private int _pay = 0;
    private int _attack = 0;


    public void InitData(List<string> items)
    {
        _name = int.Parse(items[0]);
        _pay = int.Parse(items[1]);
        _attack = int.Parse(items[2]);
    }

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