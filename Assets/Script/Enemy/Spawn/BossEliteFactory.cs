using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEliteFactory : BaseEnemyFactory
{    
    [Header("보스 프리팹 연결")]
    [SerializeField] protected List<GameObject> _bossPrefab;    

    protected int _bossIdx = 0;    

    public BaseEnemy CreateBoss(Vector2 pos, EnemySpawner spawner)
    {
        if (_bossPrefab.Count < 1)
        {
            CPrint.Warn($"{this} : bossPrefab 연결 안됨");
            enabled = false;
            return null;
        }

        if (_bossIdx >= _bossPrefab.Count)
        {
            CPrint.Log($"{this} : _bossIdx >= bossPrefab.Count");
            _bossIdx = _bossPrefab.Count - 1;
        }

        BossMob enemy = Instantiate(_bossPrefab[_bossIdx], transform).GetComponent<BossMob>();
        _bossIdx++; // 보스 리스트 인덱스 증가
        enemy.Init(_pool, spawner);
        enemy.SetTarget(_target);

        Vector2 spawnPos = _target.transform.position;
        spawnPos += pos; // 타겟 주변에서 일정 거리만큼 떨어지도록 스폰.

        enemy.transform.position = spawnPos;

        return enemy;
    }
}
