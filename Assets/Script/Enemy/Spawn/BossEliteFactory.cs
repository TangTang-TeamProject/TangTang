using System.Collections.Generic;
using UnityEngine;

public class BossEliteFactory : BaseEnemyFactory
{
    [Header("보스 SO 연결")]
    [SerializeField] private List<EnemyData_SO> _bossDatas;  

    protected int _bossIdx = 0;

    public int BossIdx => _bossIdx;

    public List<EnemyData_SO> BossDatas => _bossDatas;

    public BaseEnemy CreateBoss(Vector2 pos)
    {
        if (_bossDatas.Count < 1)
        {
            CPrint.Warn($"{this} : boss 연결 안됨");
            enabled = false;
            return null;
        }

        if (_bossIdx >= _bossDatas.Count)
        {
            CPrint.Log($"{this} : _bossIdx >= _bossDatas.Count");
            _bossIdx = _bossDatas.Count - 1;
        }

        BaseEnemy enemy = Instantiate(_bossDatas[_bossIdx].Prefab, transform).GetComponent<BaseEnemy>();
        _bossIdx++; // 보스 리스트 인덱스 증가        
        enemy.Init(_pool, 0);
        enemy.SetTarget(_target);       

        Vector2 spawnPos = _target.transform.position;
        spawnPos += pos; // 타겟 주변에서 일정 거리만큼 떨어지도록 스폰.

        enemy.transform.position = spawnPos;

        return enemy;
    }
}
