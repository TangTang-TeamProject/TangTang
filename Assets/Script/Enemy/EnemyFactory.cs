using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour 
{
    private EnemyPool pool;
  
    public EnemyFactory(EnemyPool pool)
    {
        this.pool = pool;
    }

    public Enemy CreateEnemy()
    {
        return pool.GetEnemy();
    }

    public void ReturnToPool(Enemy enemy)
    {
        pool.ReturnEnemy(enemy);
    }
}
