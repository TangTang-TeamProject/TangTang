using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameData / EvolutionRegistry", fileName = "EvolutionRegistrySO")]
public class EvolutionRegistry : ScriptableObject
{
    [SerializeField]
    private List<EvolutionData_SO> evolution = new List<EvolutionData_SO>();

    public IReadOnlyList<EvolutionData_SO> Evolution => evolution;

    private Dictionary<string, string> dataDic = new Dictionary<string, string>();
    private Dictionary<string, string> reqDic = new Dictionary<string, string>();

    void NullCheck()
    {
        if (dataDic != null && dataDic.Count != 0 && reqDic != null && reqDic.Count != 0)
        {
            return;
        }

        MakeDic();
    }

    public void MakeDic()
    {
        dataDic.Clear();
        reqDic.Clear();

        for (int i = 0; i < evolution.Count; i++)
        {
            dataDic.Add(evolution[i].BaseSkillID, evolution[i].EvolutionID);
            reqDic.Add(evolution[i].BaseSkillID, evolution[i].RequiredArtifactID);
        }
    }

    public string GetEvolutionRequire(string _ID)
    {
        NullCheck();

        if (reqDic.TryGetValue(_ID, out string data))
        {
            return data;
        }

        CPrint.Error("EvolutionRegistry - Cant Find");
        return null;
    }

    public string GetEvolution(string _ID)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out string data))
        {
            return data;
        }

        CPrint.Error("EvolutionRegistry - Cant Find");
        return null;
    }
}
