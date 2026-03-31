using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("최대 스폰 마리수 (생존 기준)")]
    [SerializeField] private int _spawnCount = 500;
    
    [Header("1 스폰당 스폰 마리수")]
    [SerializeField] private int _spawnAtOnce = 30;
    
    [Header("스폰 범위 최소 ~ 최대")]
    [SerializeField] private float _minSpawnRadius = 8.5f;
    [SerializeField] private float _maxSpawnRadius = 13f;
    
    [Header("스폰 시간 간격")]
    [SerializeField] private float _spawnTimeInterval = 10f;

    [Header("웨이브 설정")]
    [SerializeField] private List<float> _waveTime = new List<float>();
    [SerializeField] private int _waveSpawnAtOnce = 50;
    [SerializeField] private float _waveSpawnTimeInterval = 3f;
    [SerializeField] private float _waveDuration = 30f;

    [Header("엘리트 몹 스폰 설정")]    
    [SerializeField] private float _eliteSpawnTimeInterval = 60f;

    [Header("스폰할 몬스터의 팩토리")]
    [SerializeField] private BaseEnemyFactory _factory;
    [SerializeField] private BossEliteFactory _bossEliteFactory;
  

    private List<BaseEnemy> _aliveList = new List<BaseEnemy>();
    private float _nextSpawn = 0f;
    
    private float _nextEliteSpawn;
    
    private bool _isBossRound = false;
    private bool _isWaveTime = false;
    private int _waveCnt = 0; // 웨이브 인덱스     

    void Awake()
    {
                      
        if (_factory == null)
        {
            CPrint.Log("EnemySpawner -> _factory 연결 안됨");
            enabled = false;
            return;
        }    

        if (_waveTime == null)
        {
            CPrint.Log($"{this} _waveTime 설정 안됨");
            enabled = false;
            return;
        }

        _nextEliteSpawn = _eliteSpawnTimeInterval;
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
        GatheredSpawnLogic();
    }

    private void CommonSpawn()
    {        
        if (Timer.Instance.GameTime <  _nextSpawn)
        {
            return;
        }

        _nextSpawn = Timer.Instance.GameTime + _spawnTimeInterval; // 다음 스폰 시간 재설정

        if (_aliveList.Count >= _spawnCount)
        {
            return;
        }
        
        // 일반 스폰
        for (int i = 0; i < _spawnAtOnce; i++)
        {
            SpawnEnemy();
        }
    }

    private void GatheredSpawnLogic()
    {
        if (_isBossRound)
        {
            return;
        }

        EliteSpawn();

        if (!_isWaveTime)
        {
            CheckIsWaveTime();
        }

        if (_isWaveTime)
        {
            WaveSpawn();
        }
        else
        {
            CommonSpawn();
        }
        
    }

    private void CheckIsWaveTime()
    {
        if (Timer.Instance.GameTime >= _waveTime[_waveCnt] && Timer.Instance.GameTime <= (_waveTime[_waveCnt] + _waveDuration))
        {
            _isWaveTime = true;
        }
        else if (Timer.Instance.GameTime > (_waveTime[_waveCnt] + _waveDuration))
        {
            _isWaveTime = false;
            _waveCnt++;
        }
        else
        {
            _isWaveTime = false;
        }
    }

    private void EliteSpawn()
    {
        // 스폰 주기마다 엘리트몹 스폰
        if (Timer.Instance.GameTime >= _nextEliteSpawn)
        {
            _nextEliteSpawn = _nextEliteSpawn + _eliteSpawnTimeInterval;

            Vector2 _randSpawnPos = UnityEngine.Random.insideUnitCircle;
            float randSpawnRadius = UnityEngine.Random.Range(_minSpawnRadius, _maxSpawnRadius);
            _randSpawnPos = _randSpawnPos.normalized * randSpawnRadius;

            _bossEliteFactory.CreateEnemy(_randSpawnPos);
        }
    }

    private void WaveSpawn()
    {
        if (_isBossRound)
        {
            return;
        }

        if (_aliveList.Count >= _spawnCount)
        {
            return;
        }

        if (Timer.Instance.GameTime < _nextSpawn)
        {
            return;
        }

        _nextSpawn = Timer.Instance.GameTime + _waveSpawnTimeInterval;

        for (int i = 0; i < _waveSpawnAtOnce; i++)
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

    public void RemoveAliveList(BaseEnemy enemy)
    {
        _aliveList.Remove(enemy);        
    }

    public void SpawnBoss()
    {
        _isBossRound = true;
        Vector2 spawnPos = new Vector2(3f, 3f);

        BaseEnemy boss = _bossEliteFactory.CreateBoss(spawnPos, this);        
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
