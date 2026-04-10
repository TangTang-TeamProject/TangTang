using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / PlayerData", fileName = "PlayerDataSO")]
public class PlayerData_SO : ScriptableObject
{
    [SerializeField]
    private string characterID = "";
    [SerializeField]
    private string nameKR = "";
    [SerializeField]
    private string nameEN = "";
    [SerializeField]
    private float baseHP = 0;
    [SerializeField]
    private float baseATK = 0f;
    [SerializeField]
    private float baseMoveSpeed = 0f;
    [SerializeField]
    private float critRate = 0f;
    [SerializeField]
    private float pickUpRange = 0f;

    [SerializeField]
    private bool isClosed = true;
    [SerializeField]
    private ClosedChar openRequire = ClosedChar.Erin;
    [SerializeField]
    private string desc;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private GameObject prefab;

    public string CharacterID => characterID;
    public string NameKR => nameKR;
    public string NameEN => nameEN;
    public float BaseHP => baseHP;
    public float BaseATK => baseATK;
    public float BaseMoveSpeed => baseMoveSpeed;
    public float CritRate => critRate;
    public float PickUpRange => pickUpRange;

    public bool IsClosed => isClosed;
    public ClosedChar OpenRequire => openRequire;
    public string Desc => desc;
    public Sprite Icon => icon;
    public GameObject Prefab => prefab;
}
