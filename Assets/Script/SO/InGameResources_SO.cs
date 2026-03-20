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


    public float Gold => gold;

    public float Energy => energy;
}
