using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class RealEnemySpawner : MonoBehaviour
{
    [Header("스테이지 정보 입력")]
    [SerializeField] private string _stageId = "STG_001";

    [Header("WaveRegistry SO")]
    [SerializeField] private WaveRegistry _waveRegistry;


    [Header("스폰 범위 최소 ~ 최대")]
    [SerializeField] private float _minSpawnRadius = 8.5f;
    [SerializeField] private float _maxSpawnRadius = 13f;

    [Header("Enemy Factory 연결")]
    [SerializeField] private List<BaseEnemyFactory> _factories = new List<BaseEnemyFactory>();
    [SerializeField] private BossEliteFactory _bossEliteFactory;

    private List<WaveData_SO> _thisStageWave = new List<WaveData_SO>();
    private WaveData_SO _nowWave;    

    private List<BaseEnemy> _aliveList = new List<BaseEnemy>();

    private float _nextSpawnTime = 0f;
    private bool _isBossRound = false;

    private bool _enabled = true;

    private void Awake()
    {
        //if (SceneChanger.instance != null)
        //{
        //    _stageId = SceneChanger.instance.NowScene();
        //}
        GetWaveDatas(); // 스테이지 웨이브 정보 받아오기
    }

    void Start()
    {
        for (int i = 0; i <  _factories.Count; i++)
        {
            _factories[i].Pool.OnEnemyDead += RemoveAliveList;
        }
        
        Timer.Instance.BossSpawn += SpawnBoss;
        Timer.Instance.BossSpawn += ClearAliveList;
        Timer.Instance.BossDie += BossDie;        
    }

    void Update()
    {
        if (Time.frameCount % 3 != 0 || !_enabled)
        {
            return;
        }

        CheckWaveTime();

        SpawnWaves();

    }

    private void CheckWaveTime()
    {
        for (int i = 0; i < _thisStageWave.Count; i++)
        {
            int start = _thisStageWave[i].StartSec;
            int end = _thisStageWave[i].EndSec;

            if (Timer.Instance.GameTime >= start && Timer.Instance.GameTime <= end)
            {
                _nowWave = _thisStageWave[i];
                return;
            }
        }

        CPrint.Warn("웨이브 못찾음");
        _enabled = false;
        return;
    }

    private void SpawnWaves()
    {
        if (_isBossRound)
        {
            return;
        }

        if (Timer.Instance.GameTime < _nextSpawnTime)
        {
            return;
        }

        _nextSpawnTime = Timer.Instance.GameTime + _nowWave.SpawnIntervalSec;

        for (int i = 0; i < _nowWave.EnemyCount; i++)
        {
            SpawnEnemy(_nowWave.SpawnEnemy);
        }
        CPrint.Log($"{Timer.Instance.GameTime} : {_nowWave.EnemyCount} 마리 스폰.");
    }

    private void SpawnEnemy(string enemyId)
    {
        int factoryIdx = GetFactoryIdx(enemyId);

        if (factoryIdx == -1)
        {
            CPrint.Error($"{this} SpawnEnemy() -> {enemyId} 를 찾을 수 없음");
            return;
        }

        Vector2 _randSpawnPos = UnityEngine.Random.insideUnitCircle;
        float randSpawnRadius = UnityEngine.Random.Range(_minSpawnRadius, _maxSpawnRadius);
        _randSpawnPos = _randSpawnPos.normalized * randSpawnRadius;

        BaseEnemy enemy = _factories[factoryIdx].CreateEnemy(_randSpawnPos);

        _aliveList.Add(enemy);

    }

    private int GetFactoryIdx(string enemyId)
    {
        for (int i = 0; i < _factories.Count; i++)
        {
            if (_factories[i].EnemtData.EmemyID ==  enemyId)
            {
                return i;
            }
        }

        return -1;
    }

    private void GetWaveDatas()
    {
        _thisStageWave = _waveRegistry.GetEnemyByStageID(_stageId);
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
