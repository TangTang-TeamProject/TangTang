using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameData / EnemyData", fileName = "EnemyDataSO")]
public class EnemyData_SO : ScriptableObject
{
    [SerializeField]
    private int enemyID = 0;
    [SerializeField]
    private string enemyName = "";
    [SerializeField]
    private float atk = 0f;
    [SerializeField]
    private float hp = 0f;
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float atkCycle = 0f;
    [SerializeField]
    private float bulletSpeed = 0f;
    [SerializeField]
    private MobType mobtype = MobType.Basic;
    [SerializeField]
    private GameObject prefab;

    public int EmemyID => enemyID;
    public string EnemyName => enemyName;
    public float ATK => atk;
    public float HP => hp;
    public float Speed => speed;
    public float ATKCycle => atkCycle;
    public float BulletSpeed => bulletSpeed;
    public MobType MobType => mobtype;
    public GameObject Prefab => prefab;
}
