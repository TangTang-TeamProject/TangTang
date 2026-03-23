using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / PlayerData", fileName = "PlayerDataSO")]
public class PlayerData_SO : ScriptableObject
{
    [SerializeField]
    private int playerID = 0;
    [SerializeField]
    private int maxLevel = 0;
    [SerializeField]
    private float hp = 0;
    [SerializeField]
    private float atk = 0f;
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private GameObject prefab;

    public int PlayerID => playerID;
    public int MaxLevel => maxLevel;
    public float HP => hp;
    public float ATK => atk;
    public float Speed => speed;
    public GameObject Prefab => prefab;
}
