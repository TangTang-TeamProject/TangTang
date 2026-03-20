using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / ItemData", fileName = "ItemDataSO")]
public class ItemData_SO : ScriptableObject
{
    [SerializeField]
    private int itemID = 0;
    [SerializeField]
    private string itemName = "";
    [SerializeField]
    private float exp = 0f;
    [SerializeField]
    private float gold = 0f;
    [SerializeField]
    private float dmg = 0f;
    [SerializeField]
    private GameObject prefab;

    public int ItemID => itemID;
    public string ItemName => itemName;
    public float EXP => exp;
    public float Gold => gold;
    public float Dmg => dmg;
    public GameObject Prefab => prefab;

}
