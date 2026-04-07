using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMob : BaseEnemy
{
    [Header("무기 연결")]
    [SerializeField] private GameObject _leftWeaponSlot;
    [SerializeField] private GameObject _rightWeaponSlot;
    [SerializeField] private GameObject _weaponPrefab;
    

    private void FixedUpdate()
    {
        base.Update();

        if (_target == null) // 타겟 없으면 return
        {
            return;
        }

       
        if (!CanUpdate())
        {
            return;
        }

        Chase();
        CheckDamaged();
    }

    private void Pattern_ThrowWeapon()
    {

    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출

        Timer.Instance.IsBossDie(true);
        _pool.Return(this);
    }
}
