using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

    [Header("Elite 몬스터 설정")]
    [SerializeField] private float _time = 240f;

    private List<WaveData_SO> _thisStageWave = new List<WaveData_SO>();

    
    private List<WaveData_SO> _nowWave = new List<WaveData_SO>();

    private float _nowSpawnInterval;
    private float _eliteSpawnInterval;

    private List<BaseEnemy> _aliveList = new List<BaseEnemy>();

    private float _nextSpawnTime = 0f;
    private bool _isBossRound = false;    
    

    private void Awake()
    {
        //if (SceneChanger.instance != null)
        //{
        //    _stageId = SceneChanger.instance.NowScene();
        //}
        GetWaveDatas(); // 스테이지 웨이브 정보 받아오기
        _eliteSpawnInterval = _time;
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
        if (_isBossRound)
        {
            return;
        }

        if (Time.frameCount % 3 != 0)
        {
            return;
        }

        CheckWaveTime();

        if (Timer.Instance.GameTime >= _time) // 엘리트 스폰 시간 체크
        {
            if (_nowWave == null || _nowWave.Count == 0)
                return;

            int randIdx = Random.Range(0, _nowWave.Count); // 랜덤 enemyId 중에 고르기

            SpawnEnemy(_nowWave[randIdx].SpawnEnemy, true);

            _time += _eliteSpawnInterval;
        }

        if (Timer.Instance.GameTime < _nextSpawnTime)
        {
            return;
        }

        _nextSpawnTime = Timer.Instance.GameTime + _nowSpawnInterval;

        for (int i = 0; i < _nowWave.Count; i++)
        {
            SpawnWaves(_nowWave[i]);
        }
        
       
    }

    private void CheckWaveTime()
    {
        List<WaveData_SO> _curWave = new List<WaveData_SO>();

        for (int i = 0; i < _thisStageWave.Count; i++)
        {
            int start = _thisStageWave[i].StartSec;
            int end = _thisStageWave[i].EndSec;

            if (Timer.Instance.GameTime >= start && Timer.Instance.GameTime <= end)
            {                
                _curWave.Add(_thisStageWave[i]);                
            }
        }

        if (_curWave.Count == 0)
        {
            CPrint.Warn("웨이브 못찾음");          
            return;
        }

        if (_nowWave.Count != _curWave.Count || _nowWave[0] != _curWave[0])
        {
            _nowWave = _curWave;
            CPrint.Log($"현재 시간: {Timer.Instance.GameTime}");
            CPrint.Log($"현재 웨이브 개수: {_nowWave?.Count}");
            _nowSpawnInterval = _nowWave[0].SpawnIntervalSec;
        }
      
    }

    private void SpawnWaves(WaveData_SO nowWave)
    {                      
        for (int j = 0; j < nowWave.EnemyCount; j++)
        {
            SpawnEnemy(nowWave.SpawnEnemy, false);
        }
        CPrint.Log($"{Timer.Instance.GameTime} : {nowWave.EnemyCount} 마리 스폰.");
               
    }

    private void SpawnEnemy(string enemyId, bool isElite)
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

        BaseEnemy enemy;

        if (isElite)
        {
            enemy = _factories[factoryIdx].CreateElite(_randSpawnPos);
        }
        else
        {
            enemy = _factories[factoryIdx].CreateEnemy(_randSpawnPos);
        }
            

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

        BaseEnemy boss = _bossEliteFactory.CreateBoss(spawnPos);
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
