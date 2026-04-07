using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOMB : Items
{
    public Action GetEaten;

    private void Awake()
    {
        GetEaten += ItemManager.instance.BoomBoomPow;
    }

    public override void SetActiveFalse()
    {
        GetEaten?.Invoke();
        GetEaten = null;
        Destroy(this.gameObject);
    }
}
