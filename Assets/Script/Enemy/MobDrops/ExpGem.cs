using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : Items
{

    [SerializeField] private GemPool _pool;
    [SerializeField] private GemType _type;

    private float _exp;
    private int _id;

    private float _delayCheckTime = 0f;
    private float _delay = 0.1f; // 뒤로 이동하는 시간

    public Action<float> GetExp;

    public float Exp => _exp;
    public int Id => _id;


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
        _target = null;
        _isAbsorbed = false;
        ItemManager.instance.Magnetic += MagneticAbsorbed;    
    }
    

    // 흡수 되었을 시 -> pool 로 리턴.
    public override void SetActiveFalse()
    {        
        GetExp?.Invoke(Exp);
        GetExp = null;
        base.SetActiveFalse();
        _pool.Return(this);
    }

    private void MagneticAbsorbed(GameObject target)
    {
        base.GetItem(target);       
    }

    public override void MoveToTarget()
    {
        if (_target == null)
        {
            return;
        }                      

        Vector2 dir = _target.transform.position - transform.position;

        if (dir.magnitude > 0.001f)
        {
            dir.Normalize();
        }
        else
        {
            dir = Vector2.zero;
        }

        Vector2 pos = transform.position;

        if (Timer.Instance.RealTime < _delayCheckTime)
        {
            pos += -dir * _itemMoveSpeed * Time.deltaTime;
        }
        else
        {
            pos += dir * _itemMoveSpeed * Time.deltaTime;
        }
     
        transform.position = pos;
    }

    public override void GetItem(GameObject target)
    {
        if (_isAbsorbed || _target == target)
        {
            return;
        }

        _isAbsorbed = true; // 흡수 시작
        _target = target; // 타겟 설정
        _delayCheckTime = Timer.Instance.RealTime + _delay; // 뒤로 이동하는 시간 설정
    }
}
