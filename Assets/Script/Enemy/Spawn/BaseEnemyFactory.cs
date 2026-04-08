using System.Collections.Generic;
using UnityEngine;


public abstract class BaseEnemyFactory : MonoBehaviour
{
    [SerializeField] protected GameObject _target;
    [SerializeField] protected EnemyData_SO _enemyData;
    [SerializeField] protected EnemyPool _pool;
    
    public EnemyPool Pool => _pool;
    public EnemyData_SO EnemyData => _enemyData;
    
    protected int _idx = 0;

    public virtual BaseEnemy CreateEnemy(Vector2 pos)
    {
        BaseEnemy enemy = _pool.GetEnemy(_enemyData, transform);
        enemy.IsElite(false);
        enemy.Init(_pool, _idx); // 인덱스 부여
        _idx++;
        enemy.SetTarget(_target);

        Vector2 spawnPos = _target.transform.position;
        spawnPos += pos; // 타겟 주변에서 일정 거리만큼 떨어지도록 스폰.

        enemy.transform.position = spawnPos;

        return enemy;
    }
        

    public virtual BaseEnemy CreateElite(Vector2 pos)
    {
        BaseEnemy enemy = _pool.GetEnemy(_enemyData, transform);
        enemy.IsElite(true);
        enemy.Init(_pool, 0);        
        enemy.SetTarget(_target);

        Vector2 spawnPos = _target.transform.position;
        spawnPos += pos; // 타겟 주변에서 일정 거리만큼 떨어지도록 스폰.

        enemy.transform.position = spawnPos;

        return enemy;
    }
}
