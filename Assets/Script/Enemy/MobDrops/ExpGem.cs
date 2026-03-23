using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : Items
{

    [SerializeField] private GemPool _pool;
    [SerializeField] private GemType _type;

    private float _exp;

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

    }

    // 현재 경험치 반환
    // 추후, id 반환으로 변경 예정
    public override float GetItem(GameObject target)
    {
        _target = target;

        _isAbsorbed = true;

        return _exp; // 경험치 반환
    }

    // 흡수 되었을 시 -> pool 로 리턴.
    public override void SetActiveFalse()
    {       
        _pool.Return(this);
    }
}
