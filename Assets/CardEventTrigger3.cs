using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardEventTrigger3 : CardEventTiming
{
    public override void RoundStart(RoundStartInfo info)
    {
        Debug.Log("子类实现了这个CardEventTrigge3");
    }
}