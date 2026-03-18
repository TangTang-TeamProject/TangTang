using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

// List 번호 == ID로 맞출 것
[CreateAssetMenu(menuName = "GameData / EnemyRegistry", fileName = "EnemyRegistrySO")]
public class EnemyRegistry : ScriptableObject
{
    [SerializeField]
    private List<EnemyData_SO> enemys = new List<EnemyData_SO>();

    public IReadOnlyList<EnemyData_SO> Enemys => enemys;

    public bool GetEnemyByID(int _ID, out EnemyData_SO enemy)
    {
        enemy = null;

        if (_ID >= enemys.Count || enemys[_ID] == null)
            return false;

        enemy = enemys[_ID];

        return true;
    }
}
