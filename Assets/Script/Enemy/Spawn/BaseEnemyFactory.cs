using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public abstract class BaseEnemyFactory : MonoBehaviour
{
    [SerializeField] protected GameObject _target;
    [SerializeField] protected EnemyPool _pool;

    public EnemyPool Pool => _pool;

    public virtual BaseEnemy CreateEnemy(Vector2 pos)
    {
        BaseEnemy enemy = _pool.GetEnemy(transform);
        enemy.Init(_pool);
        enemy.SetTarget(_target);

        Vector2 spawnPos = _target.transform.position;
        spawnPos += pos; // 타겟 주변에서 일정 거리만큼 떨어지도록 스폰.

        enemy.transform.position = spawnPos;

        return enemy;
    }
    
}
