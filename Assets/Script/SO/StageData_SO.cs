using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / StageData", fileName = "StageDataSO")]
public class StageData_SO : ScriptableObject
{
    [SerializeField]
    private string stageID;
    [SerializeField]
    private Scenes sceneEnum;
    [SerializeField]
    private string stageName;
    [SerializeField]
    private Sprite img;
    [SerializeField]
    private bool unLockStage = false;
    [SerializeField]
    private string unLockChar = "";

    public string StageID => stageID;
    public Scenes SceneEnum => sceneEnum;
    public string StageName => stageName;
    public Sprite IMG => img;
    public bool UnLockStage => unLockStage;
    public string UnLockChar => unLockChar;
}
