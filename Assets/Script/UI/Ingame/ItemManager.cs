using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public Action Bomb;
    public Action Heal;
    public Action Money;
    public Action Magnatic;

    private int gemCount = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public void GetGems(float _exp)
    {
        gemCount++;

        CPrint.Log($"gemCount : {gemCount}");
    }

    public void BoomBoomPow()
    {
        Bomb?.Invoke();
    }

    public void HealTheWorld()
    {
        Heal?.Invoke();
    }

    public void ShowMeTheMoney()
    {
        Money?.Invoke();
    }

    public void LikeItsMagnatic()
    {
        Magnatic?.Invoke();
    }
}
