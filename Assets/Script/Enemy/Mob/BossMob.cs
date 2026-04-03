using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMob : BaseEnemy
{

    public Action FinalBossDie;

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

   

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출

        FinalBossDie?.Invoke();
        Destroy(gameObject);
    }
}
