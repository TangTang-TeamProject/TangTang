using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LotteryBox : Items
{
    public Action GetEaten;

    private void Awake()
    {
        GetEaten += ItemManager.instance.OpenTheBox;
    }

    public override void SetActiveFalse()
    {
        GetEaten?.Invoke();
        GetEaten = null;
        Destroy(this.gameObject);
    }
}
