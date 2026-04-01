using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / WaveData", fileName = "WaveDataSO")]
public class WaveData_SO : ScriptableObject
{
    [SerializeField]
    private string stageID = "";
    [SerializeField]
    private int startSec = 0;
    [SerializeField]
    private int endSec = 0;
    [SerializeField]
    private string spawnEnemy = "";
    [SerializeField]
    private int enemyCount = 0;
    [SerializeField]
    private int spawnIntervalSec = 0;

    public string StageID => stageID;
    public int StartSec => startSec;
    public int EndSec => endSec;
    public int EnemyCount => enemyCount;
    public string SpawnEnemy => spawnEnemy;
    public int SpawnIntervalSec => spawnIntervalSec;
}
