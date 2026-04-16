using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / EquipLevelData", fileName = "EquipLevelDataSO")]
public class EquipLevel_SO : ScriptableObject
{
    [SerializeField]
    private string equipID = "";
    [SerializeField]
    private int upGradeRequire = 0;
    [SerializeField]
    private int level = 0;
    [SerializeField]
    private float atk = 0f;
    [SerializeField]
    private float hpChange = 0f;
    [SerializeField]
    private float speedChange = 0f;

    public string EquipID => equipID;
    public int UpGradeRequire => upGradeRequire;
    public int Level => level;
    public float ATK => atk;
    public float HPChange => hpChange;
    public float SpeedChange => speedChange;

}
