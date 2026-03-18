using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / EnemyRegistry", fileName = "EnemyRegistrySO")]
public class EnemyRegistry : ScriptableObject
{
    [SerializeField]
    private List<EnemyData_SO> enemys = new List<EnemyData_SO>();

    public IReadOnlyList<EnemyData_SO> Enemys => enemys;

    private Dictionary<int, EnemyData_SO> dataDic = new Dictionary<int, EnemyData_SO>();

    public void ReMake()
    {
        dataDic.Clear();

        for (int i = 0; i < enemys.Count; i++)
        {
            dataDic.Add(enemys[i].EmemyID, enemys[i]);
        }
    }

    public EnemyData_SO GetEnemyByID(int _ID)
    {
        if (dataDic.TryGetValue(_ID, out EnemyData_SO data))
        {
            return data;
        }

        Debug.Log("EnemyRegistry - Error");
        return null;
    }
}
