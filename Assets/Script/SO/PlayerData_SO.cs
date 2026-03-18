using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / PlayerData", fileName = "PlayerDataSO")]
public class PlayerData_SO : ScriptableObject
{
    [SerializeField]
    private int maxLevel = 0;
    [SerializeField]
    private List<int> requireEXP = new List<int>();
    [SerializeField]
    private int MaxArtifactCarry = 0;
    [SerializeField]
    private float atk = 0f;
    [SerializeField]
    private float def = 0f;
    [SerializeField]
    private float speed = 0f;

    public int MaxLevel => maxLevel;
    public IReadOnlyList<int> RequireEXP => requireEXP;
    public float ATK => atk;
    public float DEF => def;
    public float Speed => speed;
}
