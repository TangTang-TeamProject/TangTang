using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{   
    [SerializeField] private GameObject _enemy;

    [SerializeField] private int _spawnCount = 500;
    [SerializeField] private int _spawnAtOnce = 20;
    [SerializeField] private float _spawnRadius = 8.5f;
    [SerializeField] private float _spawnTime = 10f;
    
    [SerializeField] private BaseEnemyFactory _factory;
  

    private List<GameObject> _aliveList = new List<GameObject>();

    public static EnemySpawner Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Instance = null;
            return;
        }
        Instance = this;

        if (_enemy == null)
        {
            Debug.Log("Enemy 리스트 없음");
            enabled = false;
            return;
        }
        
        if (_factory == null)
        {
            Debug.Log("EnemySpawner -> _factory 연결 안됨");
            enabled = false;
            return;
        }    
        
    }


    void Start()
    {
        StartCoroutine(Spawn());
    }


    IEnumerator Spawn()
    {
        while(true)
        {
            if (_aliveList.Count >= _spawnCount)
            {
                yield return null;
            }

            for (int i = 0; i < _spawnAtOnce; i++)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(_spawnTime);
        }        
    }

    private void SpawnEnemy()
    {
        Vector2 _randSpawnPos = Random.insideUnitCircle;
        _randSpawnPos = _randSpawnPos.normalized * _spawnRadius;

        BaseEnemy enemy = _factory.CreateEnemy(_randSpawnPos);

        _aliveList.Add(_enemy);

    }
  
}
