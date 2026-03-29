using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMob : BaseEnemy
{
    private EnemySpawner _spawner;
    public void Init(EnemyPool pool, EnemySpawner spawner)
    {
        base.Init(pool, 0);

        _spawner = spawner;

    }

    private void FixedUpdate()
    {
        Chase();
        CheckDamaged();
    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출

        Timer.Instance.IsBossDie();
        Destroy(gameObject);
    }
}
