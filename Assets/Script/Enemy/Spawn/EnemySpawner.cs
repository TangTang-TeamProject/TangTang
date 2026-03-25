using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [SerializeField] private int _spawnCount = 500;
    [SerializeField] private int _spawnAtOnce = 20;
    [SerializeField] private float _spawnRadius = 8.5f;
    [SerializeField] private float _spawnTime = 10f;    
    [SerializeField] private BaseEnemyFactory _factory;
  

    private List<BaseEnemy> _aliveList = new List<BaseEnemy>();
    private float _nextSpawn = 0f;

    void Awake()
    {
                      
        if (_factory == null)
        {
            CPrint.Log("EnemySpawner -> _factory 연결 안됨");
            enabled = false;
            return;
        }
        _factory.Pool.OnEnemyDead += RemoveAliveList;
    }
    void Start()
    {
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
        if (Time.time <  _nextSpawn)
        {
            return;
        }

        _nextSpawn = Time.time + _spawnTime; // 다음 스폰 시간 재설정


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
        _randSpawnPos = _randSpawnPos.normalized * _spawnRadius;

        BaseEnemy enemy = _factory.CreateEnemy(_randSpawnPos);

        _aliveList.Add(enemy);

    }  

    private void RemoveAliveList(BaseEnemy enemy)
    {
        _aliveList.Remove(enemy);        
    }
}
