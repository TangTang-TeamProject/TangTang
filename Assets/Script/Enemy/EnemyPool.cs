using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool
{    
    private Queue<Enemy> _enemyPool = new Queue<Enemy>();
    
    public void Add(Enemy enemy)
    {
        _enemyPool.Enqueue(enemy);
    }

    public Enemy GetEnemy()
    {
        if ( _enemyPool.Count > 0 )
        {
            return _enemyPool.Dequeue();
        }
        else
        {
            return null;
        }
    }

    public void ReturnEnemy(Enemy enemy)
    {
        _enemyPool.Enqueue(enemy);
    }
}
