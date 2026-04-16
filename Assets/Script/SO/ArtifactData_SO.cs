using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameData / ArtifactData", fileName = "ArtifactDataSO")]
public class ArtifactData_SO : ScriptableObject
{
    [SerializeField]
    private string artifactID = "";
    [TextArea(3, 10)]
    [SerializeField]
    private string nameKR = "";
    [SerializeField]
    private string nameEN = "";
    [SerializeField]
    private StatKey statKey = StatKey.Damage;
    [SerializeField]
    private float valuePerLevel = 0f;
    [SerializeField]
    private int maxLevel = 0;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Sprite img;

    public string ArtifactID => artifactID;
    public string NameKR => nameKR;
    public string NameEN => nameEN;
    public StatKey StatKey => statKey;
    public float ValuePerLevel => valuePerLevel;
    public int MaxLevel => maxLevel;
    public GameObject Prefab => prefab;
    public Sprite IMG => img;
}
