using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "GameData / EquipData", fileName = "EquipDataSO")]
public class EquipData_SO : ScriptableObject
{
    [SerializeField]
    private int equipID = 0;
    [SerializeField]
    private string equipName = "";
    [SerializeField]
    private EquipType type;
    [SerializeField]
    private float atk = 0f;
    [SerializeField]
    private float hpChange = 0f;
    [SerializeField]
    private float speedChange = 0f;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Sprite img;

    public int EquipID => equipID;
    public string EquipName => equipName;
    public EquipType Type => type;
    public float ATK => atk;
    public float HPChange => hpChange;
    public float SpeedChange => speedChange;
    public GameObject Prefab => prefab;
    public Sprite IMG => img;
}
