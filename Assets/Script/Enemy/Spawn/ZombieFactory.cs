using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFactory : BaseEnemyFactory
{
          

    public override BaseEnemy CreateEnemy(Vector2 pos)
    {
        BaseEnemy enemy = _pool.GetEnemy(transform);
        enemy.Init(_pool);
        enemy.SetTarget(_target);
        enemy.transform.position = pos;

        return enemy;
    }
}
