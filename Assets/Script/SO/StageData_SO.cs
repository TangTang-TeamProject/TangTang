using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / StageData", fileName = "StageDataSO")]
public class StageData_SO : MonoBehaviour
{
    [SerializeField]
    private string stageID;
    [SerializeField]
    private string stageName;
    [SerializeField]
    private string img;

    public string StageID => stageID;
    public string StageName => stageName;
    public string IMG => img;
}
