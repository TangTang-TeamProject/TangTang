using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameData / ArtifactData", fileName = "ArtifactDataSO")]
public class ArtifactData_SO : ScriptableObject
{
    [SerializeField]
    private int artifactID = 0;
    [SerializeField]
    private string artifactName = "";    
    [SerializeField]
    private float atkCycle = 0f;
    [SerializeField]
    private float bulletSpeed = 0f;
    [SerializeField]
    private float dmg = 0f;
    [SerializeField]
    private GameObject prefab;

    public int ArtifactID => artifactID;
    public string ArtifactName => artifactName;
    public float AtkCycle => atkCycle;
    public float BulletSpeed => bulletSpeed;
    public float Dmg => dmg;
    public GameObject Prefab => prefab;
}
