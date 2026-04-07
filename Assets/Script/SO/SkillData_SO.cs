using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameData / SkillData", fileName = "SkillDataSO")]
public class SkillData_SO : ScriptableObject
{
    [SerializeField]
    private string skillID = "";
    [SerializeField]
    private string nameKR = "";
    [SerializeField]
    private string nameEN = "";
    [SerializeField]
    private string maxLevel = "";
    [SerializeField]
    private Sprite img;
    [SerializeField]
    private SkillAttack prefab;

    public string SkillID => skillID;
    public string NameKR => nameKR;
    public string NameEN => nameEN;
    public string MaxLevel => maxLevel;
    public Sprite IMG => img;
    public SkillAttack Prefab => prefab;
}
