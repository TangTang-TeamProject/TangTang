using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameData / EnemyData", fileName = "EnemyDataSO")]
public class EnemyData_SO : ScriptableObject
{
    [SerializeField]
    private string enemyID = "";
    [SerializeField]
    private string nameKR = "";
    [SerializeField]
    private string nameEN = "";
    [SerializeField]
    private float hp = 0f;
    [SerializeField]
    private float moveSpeed = 0f;
    [SerializeField]
    private float contactDamage = 0f;
    [SerializeField]
    private float expDrop = 0f;
    [SerializeField]
    private float sizeScale = 0f;
    [SerializeField]
    private float atkCycle = 0f;
    [SerializeField]
    private float bulletSpeed = 0f;
    [SerializeField]
    private EnemyType enemytype = EnemyType.Normal;
    [SerializeField]
    private GameObject prefab;

    public string EmemyID => enemyID;
    public string NameKR => nameKR;
    public string NameEN => nameEN;
    public float HP => hp;
    public float MoveSpeed => moveSpeed;
    public float ContactDamage => contactDamage;
    public float ExpDrop => expDrop;
    public float SizeScale => sizeScale;
    public float ATKCycle => atkCycle;
    public float BulletSpeed => bulletSpeed;
    public EnemyType EnemyType => enemytype;
    public GameObject Prefab => prefab;
}
