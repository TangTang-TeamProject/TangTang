using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;

    [SerializeField] private int _spawnCount = 100;
    [SerializeField] private int _spawnAtOnce = 10;
    [SerializeField] private float _spawnRadius = 8.0f;
    [SerializeField] private float _spawnTime = 10f;
    
    private EnemyFactory _factory;
    private EnemyPool _pool = new EnemyPool();

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

        for (int i = 0; i < 100; i++)
        {
            GameObject go = Instantiate(_enemy, gameObject.transform);
            Enemy enemy = go.GetComponent<Enemy>();            
            _pool.Add(enemy);            

            go.SetActive(false);
            
        }

        _factory = new EnemyFactory(_pool);
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
                Enemy enemy = _factory.CreateEnemy();

                enemy.gameObject.SetActive(true);
                
                Vector2 _spawnPos = Random.insideUnitCircle;
                enemy.transform.position = _spawnPos.normalized * _spawnRadius;
                enemy.SetTarget(_player);
                
                _aliveList.Add(_enemy);
            }

            yield return new WaitForSeconds(_spawnTime);
        }        
    }

    public void Return(Enemy enemy)
    {
        _aliveList.Remove(enemy.gameObject);
        _factory.ReturnToPool(enemy);
    }
}
