using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.UIElements;

struct OnAddCardInfo : IEvent
{
    public GameObject owner;
    public Card card;
}

public class CardManager : MonoBehaviour
{
    private GameObject _owner;
    private Queue<Card> _cards = new Queue<Card>();
    private Card _currentCard;
    private bool _inRound = false;

    private int _cardCount = 0;

    private void Start()
    {
        AddListener();
    }


    #region 添加监听事件

    private void AddListener()
    {
        EventCenter.Instance.Register<OnAddCardInfo>(AddCardInfo).UnRegisterWhenGameObjectOnDestroy(gameObject);
        EventCenter.Instance.Register<OnAddCardInfo>(AddCardInfo).UnRegisterWhenGameObjectOnDestroy(gameObject);
        EventCenter.Instance.Broadcast<OnAddCardInfo>(new OnAddCardInfo { owner = null, card = null });
    }

    private void AddCardInfo(OnAddCardInfo info)
    {
        Debug.Log($"触发了事件: {_cardCount}");
        _cardCount++;
        if (_owner == null || info.owner == null)
            return;

        if (_owner != info.owner)
        {
            return;
        }

        // if (CheckCondition())
        _cards.Enqueue(info.card);
    }

    #endregion

    #region 出牌逻辑

    /// <summary>
    /// 用于出牌前的条件判断
    /// 这个东西感觉不应该做在这里
    /// 应该关门有个角色管理器，角色管理器里面有这个用来作为出牌前的资源检查
    /// </summary>
    /// <returns></returns>
    private bool CheckCondition()
    {
        return true;
    }

    /// <summary>
    ///检测_currentCard是否为空
    ///如果为空，则需要检测是否是自己的回合
    ///是自己回合就开始出牌
    /// </summary>
    private void CheckCard()
    {
        if (!_inRound)
        {
            return;
        }

        if (_currentCard == null)
        {
            _currentCard = _cards.Dequeue();
        }
    }

    private void UseCard()
    {
    }

    #endregion
}