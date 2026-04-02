using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMob : BaseEnemy
{
    
    

    private void FixedUpdate()
    {
        base.Update();

        Chase();
        CheckDamaged();
    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출

        base.Die();
    }
}
