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

    private Dictionary<string, EnemyData_SO> dataDic = new Dictionary<string, EnemyData_SO>();

    void NullCheck()
    {
        if (dataDic != null && dataDic.Count != 0)
        {
            return;
        }

        MakeDic();
    }
    public void MakeDic()
    {
        dataDic.Clear();

        for (int i = 0; i < enemys.Count; i++)
        {
            dataDic.Add(enemys[i].EnemyID, enemys[i]);
        }
    }

    public EnemyData_SO GetEnemyByID(string _ID)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out EnemyData_SO data))
        {
            return data;
        }

        CPrint.Error("EnemyRegistry - Cant Find");
        return null;
    }
}
