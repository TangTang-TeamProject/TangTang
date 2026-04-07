using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOLDCOIN : Items
{
    public Action GetEaten;

    private void Awake()
    {
        GetEaten += ItemManager.instance.ShowMeTheMoney;
    }


    public override void SetActiveFalse()
    {
        GetEaten?.Invoke();
        GetEaten = null;
        Destroy(this.gameObject);
    }
}
