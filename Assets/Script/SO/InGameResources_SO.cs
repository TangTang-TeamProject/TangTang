using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameData / InGameData", fileName = "IngameDataSO")]
public class InGameResources_SO : ScriptableObject
{
    [SerializeField]
    private float gold;
    [SerializeField]
    private float energy;
    [SerializeField]
    private float rewardTime;


    public float Gold => gold;

    public float Energy => energy;

    public float RewardTime => rewardTime;
}
