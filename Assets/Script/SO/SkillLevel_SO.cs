using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / SkillLevel", fileName = "SkillLevelSO")]
public class SkillLevel_SO : ScriptableObject
{
    [SerializeField]
    private string skillID = "";
    [SerializeField]
    private int level = 0;
    [SerializeField]
    private float damage = 0f;
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float appearTime = 0f;
    [SerializeField]
    private float disAppearTime = 0f;



    public string SkillID => skillID;
    public int Level => level;
    public float Damage => damage;
    public float Speed => speed;
    public float AppearTime => appearTime;
    public float DisAppearTime => disAppearTime;
}
