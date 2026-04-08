using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / WaveRegistry", fileName = "WaveRegistrySO")]
public class WaveRegistry : ScriptableObject
{
    [SerializeField]
    private List<WaveData_SO> waves = new List<WaveData_SO>();

    public IReadOnlyList<WaveData_SO> Waves => waves;

    private Dictionary<string, List<WaveData_SO>> dataDic = new Dictionary<string, List<WaveData_SO>>();

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

        for (int i = 0; i < waves.Count; i++)
        {
            if (!dataDic.ContainsKey(waves[i].StageID))
            {
                dataDic.Add(waves[i].StageID, new List<WaveData_SO>());
            }

            dataDic[waves[i].StageID].Add(waves[i]);
        }
    }

    public List<WaveData_SO> GetEnemyByStageID(string _ID)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out List<WaveData_SO> data))
        {
            return data;
        }

        CPrint.Error("WaveRegistry - Cant Find");
        return null;
    }
}
