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
    public Action<GameObject> Magnetic;
    public Action<float> EXP;
    public Action ItemBox;

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
        EXP?.Invoke(_exp);
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

    public void LikeItsMagnetic(GameObject target)
    {
        Magnetic?.Invoke(target);
    }

    public void OpenTheBox()
    {
        ItemBox?.Invoke();
    }
}
