using System.Collections.Generic;
using UnityEngine;


public abstract class BaseEnemyFactory : MonoBehaviour
{
    [SerializeField] protected GameObject _target;
    [SerializeField] protected EnemyPool _pool;

    [Header("보스 프리팹 연결")]
    [SerializeField] protected List<GameObject> _bossPrefab;
    public EnemyPool Pool => _pool;

    protected int _bossIdx = 0;

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
    
    public virtual BaseEnemy CreateBoss(Vector2 pos)
    {
        if (_bossPrefab.Count < 1)
        {
            CPrint.Warn($"{this} : bossPrefab 연결 안됨");
            enabled = false;
            return null;
        }

        if (_bossIdx >=  _bossPrefab.Count)
        {
            CPrint.Log($"{this} : _bossIdx >= bossPrefab.Count");
            _bossIdx = _bossPrefab.Count - 1;
        }

        BaseEnemy enemy = Instantiate(_bossPrefab[_bossIdx], transform).GetComponent<BaseEnemy>();
        _bossIdx++;
        enemy.Init(_pool);
        enemy.SetTarget(_target);

        Vector2 spawnPos = _target.transform.position;
        spawnPos += pos; // 타겟 주변에서 일정 거리만큼 떨어지도록 스폰.

        enemy.transform.position = spawnPos;

        return enemy;
    }
}
