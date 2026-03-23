using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : Items
{

    [SerializeField] private GemPool _pool;
    [SerializeField] private GemType _type;

    private float _exp;
    private float _id;

    public float Exp => _exp;
    public float Id => _id;

    public void Init(GemPool pool)
    {
        if (_itemData == null)
        {
            CPrint.Log($"{this} -> SO 연결 안됨");
            enabled = false;
            return;
        }

        _pool = pool;

        _exp = _itemData.EXP;
        _id = _itemData.ItemID;

    }
    

    // 흡수 되었을 시 -> pool 로 리턴.
    public override void SetActiveFalse()
    {       
        _pool.Return(this);
    }
}
