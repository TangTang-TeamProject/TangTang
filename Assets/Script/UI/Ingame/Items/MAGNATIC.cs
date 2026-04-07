using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAGNATIC : Items
{
    public Action<GameObject> GetEaten;

    private void Awake()
    {
        GetEaten += ItemManager.instance.LikeItsMagnetic;
    }

    public override void SetActiveFalse()
    {
        GetEaten?.Invoke(_target);
        GetEaten = null;
        Destroy(this.gameObject);
    }
}
