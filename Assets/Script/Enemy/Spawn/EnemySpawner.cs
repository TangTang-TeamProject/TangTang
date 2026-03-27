using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [SerializeField] private int _spawnCount = 500;
    [SerializeField] private int _spawnAtOnce = 30;
    [SerializeField] private float _minSpawnRadius = 8.5f;
    [SerializeField] private float _maxSpawnRadius = 13f;    
    [SerializeField] private float _spawnTime = 5f;    
    [SerializeField] private BaseEnemyFactory _factory;
  

    private List<BaseEnemy> _aliveList = new List<BaseEnemy>();
    private float _nextSpawn = 0f;
    
    private bool _isBossRound = false;   

    void Awake()
    {
                      
        if (_factory == null)
        {
            CPrint.Log("EnemySpawner -> _factory 연결 안됨");
            enabled = false;
            return;
        }
        
        
    }
    void Start()
    {
        _factory.Pool.OnEnemyDead += RemoveAliveList;
        Timer.Instance.BossSpawn += SpawnBoss;
        Timer.Instance.BossSpawn += ClearAliveList;
        Timer.Instance.BossDie += BossDie;
        //StartCoroutine(Spawn());
    }

    private void Update()
    {
        Spawn2();
    }


    IEnumerator Spawn()
    {
        while(true)
        {
            if (_aliveList.Count >= _spawnCount)
            {                
                yield return null;
                continue;
            }

            for (int i = 0; i < _spawnAtOnce; i++)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(_spawnTime);
        }        
    }

    private void Spawn2()
    {
        if (_isBossRound)
        {
            return;
        }

        if (Timer.Instance.GameTime <  _nextSpawn)
        {
            return;
        }

        _nextSpawn = Timer.Instance.GameTime + _spawnTime; // 다음 스폰 시간 재설정


        if (_aliveList.Count >= _spawnCount)
        {
            return;
        }

        for (int i = 0; i < _spawnAtOnce; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector2 _randSpawnPos = UnityEngine.Random.insideUnitCircle;
        float randSpawnRadius = UnityEngine.Random.Range(_minSpawnRadius, _maxSpawnRadius);
        _randSpawnPos = _randSpawnPos.normalized * randSpawnRadius;

        BaseEnemy enemy = _factory.CreateEnemy(_randSpawnPos);

        _aliveList.Add(enemy);

    }  

    private void RemoveAliveList(BaseEnemy enemy)
    {
        _aliveList.Remove(enemy);        
    }

    public void SpawnBoss()
    {
        _isBossRound = true;
        Vector2 spawnPos = new Vector2(3f, 3f);

        BaseEnemy boss = _factory.CreateBoss(spawnPos, this);        
        if (boss == null)
        {
            CPrint.Warn("Boss 없음");
            _isBossRound = false;
            Timer.Instance.IsBossDie();
        }
    }

    public void ClearAliveList()
    {
        _aliveList.Clear();
    }

    public void BossDie()
    {
        _isBossRound = false;
        
    }
}
