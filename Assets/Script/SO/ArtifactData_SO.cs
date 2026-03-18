using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameData / ArtifactData", fileName = "ArtifactDataSO")]
public class ArtifactData_SO : ScriptableObject
{
    [SerializeField]
    private int artifactID = 0;
    [SerializeField]
    private float atkCycle = 0f;
    [SerializeField]
    private float bulletSpeed = 0f;
    [SerializeField]
    private float dmg = 0f;
    /*
    이후 팩토리와 조율
    [SerializeField]
    private GameObject prefab;
    */

    public int ArtifactID => artifactID;
    public float AtkCycle => atkCycle;
    public float BulletSpeed => bulletSpeed;
    public float Dmg => dmg;
}
