using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public event Action Bomb;
    public event Action Heal;
    public event Action Money;
    public event Action<GameObject> Magnetic;
    public event Action<float> EXP;
    public event Action LuckyBox;
    public event Action SkillPick;

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
        SoundManager.Instance.PlaySfx(ESfxType.Bomb);
        Bomb?.Invoke();
    }

    public void HealTheWorld()
    {
        SoundManager.Instance.PlaySfx(ESfxType.Meat);
        Heal?.Invoke();
    }

    public void ShowMeTheMoney()
    {
        SoundManager.Instance.PlaySfx(ESfxType.Coin);
        Money?.Invoke();
    }

    public void LikeItsMagnetic(GameObject target)
    {
        SoundManager.Instance.PlaySfx(ESfxType.Magnet);
        Magnetic?.Invoke(target);
    }

    public void OpenTheBox()
    {
        LuckyBox?.Invoke();
    }

    public void PickMeUp()
    {
        SkillPick?.Invoke();
    }
}
