using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealEnemySpawner : MonoBehaviour
{
    [Header("스테이지 정보 입력")]
    [SerializeField] private string _stageId = "STG_001";

    [Header("WaveRegistry SO")]
    [SerializeField] private WaveRegistry _waveRegistry;

    [Header("스폰 카운트")]
    [SerializeField] private float _spawnCount;

    [Header("스폰 범위 최소 ~ 최대")]
    [SerializeField] private float _minSpawnRadius = 8.5f;
    [SerializeField] private float _maxSpawnRadius = 13f;

    [Header("Enemy Factory 연결")]
    [SerializeField] private List<BaseEnemyFactory> _factories = new List<BaseEnemyFactory>();

    [Header("Boss 3종류 연결")]
    [SerializeField] private List<GameObject> _bossPrefabs = new List<GameObject>();
    [Header("Boss 써클 연결")]
    [SerializeField] private GameObject _bossCircle;
    [SerializeField] private float _circleErase = 3f;

    [Header("타겟")]
    [SerializeField] private GameObject _target;

    [Header("Elite 몬스터 설정")]
    [SerializeField] private float _time = 240f;

    private List<WaveData_SO> _thisStageWave = new List<WaveData_SO>();
    
    private List<WaveData_SO> _nowWave = new List<WaveData_SO>();

    private float _nowSpawnInterval;
    private float _eliteSpawnInterval;

    private List<BaseEnemy> _aliveList = new List<BaseEnemy>();

    private float _nextSpawnTime = 0f;
    private bool _isBossRound = false;    
    
   
    private int _bossIdx = 0;    

    private void Awake()
    {
        //if (SceneChanger.instance != null)
        //{
        //    _stageId = SceneChanger.instance.NowScene();
        //}
        GetWaveDatas(); // 스테이지 웨이브 정보 받아오기
        _eliteSpawnInterval = _time;

        for (int i = 0; i < _bossPrefabs.Count; i++)
        {
            _bossPrefabs[i].SetActive(false);
        }

        _bossCircle.SetActive(false);
    }

    void Start()
    {
        for (int i = 0; i <  _factories.Count; i++)
        {
            _factories[i].Pool.OnEnemyDead += RemoveAliveList;
        }
                       

        Timer.Instance.BossDie += BossDie;        
    }

    void Update()
    {
        if (_isBossRound)
        {                        
            return;
        }

        if (Time.frameCount % 2 != 0)
        {
            return;
        }

        CheckWaveTime();

        if (_nowWave == null || _nowWave.Count == 0)
        {
            return;
        }

        CheckInActiveWave();

        if (_aliveList.Count > _spawnCount)
        {
            return;
        }
       
        if (Timer.Instance.GameTime >= _time) // 엘리트 스폰 시간 체크
        {
            _time = Timer.Instance.GameTime + _eliteSpawnInterval;

            int randIdx = Random.Range(0, _nowWave.Count); // 랜덤 enemyId 중에 고르기

            SpawnEnemy(_nowWave[randIdx].SpawnEnemy, true);            
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
        if (_thisStageWave.Count < 1)
        {
            CPrint.Log("웨이브 모두 완료");
            return;
        }

        for (int i = 0; i < _thisStageWave.Count; i++)
        {            

            int start = _thisStageWave[i].StartSec;     
            int end = _thisStageWave[i].EndSec;

            if (Timer.Instance.GameTime >= start && Timer.Instance.GameTime <= end)
            {                                                
                if (CheckIsBossWave(_thisStageWave[i].SpawnEnemy)) // 해당 웨이브가 보스 웨이브라면
                {                    
                    Timer.Instance.IsBossSpawn(start);                                        
                    _nowWave.Clear();
                    SpawnBoss();
                    ClearAliveList();
                    _thisStageWave.RemoveAt(i);
                    return;
                }
                
                _nowWave.Add(_thisStageWave[i]);
                _thisStageWave.RemoveAt(i);

                _nowSpawnInterval = _thisStageWave[i].SpawnIntervalSec;
                CPrint.Warn("웨이브 변경됨");
                CPrint.Log($"현재 시간: {Timer.Instance.GameTime}");
                CPrint.Log($"현재 웨이브 개수: {_nowWave?.Count}");
            }
        }                    
    }

    private void CheckInActiveWave()
    {       
        for (int i = _nowWave.Count - 1; i > 0; i--)
        {
            if (Timer.Instance.GameTime > _nowWave[i].EndSec)
            {
                _nowWave.RemoveAt(i);
            }
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

    private bool CheckIsBossWave(string enemyId)
    {
        for (int i = 0; i < _bossPrefabs.Count; i++)
        {
            if (enemyId == "ENM_003" || enemyId == "ENM_004" || enemyId == "ENM_008")
            {
                return true;
            }
        }
        
        return false;
    }

    private int GetFactoryIdx(string enemyId)
    {
        for (int i = 0; i < _factories.Count; i++)
        {
            if (_factories[i].EnemyData.EnemyID ==  enemyId)
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

        _bossCircle.SetActive(true);
        StartCoroutine(BossSpawnAfterSeconds());
    }

    IEnumerator BossSpawnAfterSeconds()
    {
        yield return new WaitForSeconds(_circleErase);

        _bossCircle.SetActive(false);
        _bossPrefabs[_bossIdx].SetActive(true);

        BaseEnemy boss = _bossPrefabs[_bossIdx].GetComponent<BaseEnemy>();
        boss.Init(null, 0);
        boss.SetTarget(_target);

        _bossIdx++;
    }

    public void ClearAliveList()
    {
        _aliveList.Clear();
    }

    public void BossDie(bool last)
    {
        if (last)
        {
            return;
        }
        _isBossRound = false;        
    }
}
