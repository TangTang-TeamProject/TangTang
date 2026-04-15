using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "GameData / EquipData", fileName = "EquipDataSO")]
public class EquipData_SO : ScriptableObject
{
    [SerializeField]
    private string equipID = "";
    [SerializeField]
    private string equipName = "";
    [SerializeField]
    private EquipType type;
    [SerializeField]
    private int maxLevel = 2;
    [SerializeField]
    private string desc = "";
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Sprite img;

    public string EquipID => equipID;
    public string EquipName => equipName;
    public EquipType Type => type;
    public int MaxLevel => maxLevel;
    public string Desc => desc;
    public GameObject Prefab => prefab;
    public Sprite IMG => img;
}
