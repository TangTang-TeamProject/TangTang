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
    private ClosedChar unLockChar;
    [SerializeField]
    private int clearReward;
    [SerializeField]
    private int amount;
    [SerializeField]
    private EBgmType bgmType;

    public string StageID => stageID;
    public Scenes SceneEnum => sceneEnum;
    public string StageName => stageName;
    public Sprite IMG => img;
    public bool UnLockStage => unLockStage;
    public ClosedChar UnLockChar => unLockChar;
    public int ClearReward => clearReward;
    public int Amount => amount;
    public EBgmType BgmType => bgmType;
}
