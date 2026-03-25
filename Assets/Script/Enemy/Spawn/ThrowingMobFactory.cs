using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThrowingMobFactory : BaseEnemyFactory
{
    [SerializeField] private ProjectileFactory projectileFactory;
    [SerializeField] private ProjectilePool projectilePool;

    public override BaseEnemy CreateEnemy(Vector2 pos)
    {
        ThrowingMob enemy = (ThrowingMob)_pool.GetEnemy(transform);

        Vector2 spawnPos = _target.transform.position;
        spawnPos += pos; // 타겟 주변에서 일정 거리만큼 떨어지도록 스폰.

        enemy.transform.position = spawnPos;
       
        enemy.SetProjectileFactory(projectileFactory);
        enemy.Init(_pool);       
        enemy.SetTarget(_target);
        
        return enemy;
    }
}
