using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / StageRegistry", fileName = "StageRegistrySO")]
public class StageRegistry : ScriptableObject
{
    [SerializeField]
    private List<StageData_SO> stages = new List<StageData_SO>();

    public IReadOnlyList<StageData_SO> Stages => stages;

    private Dictionary<string, StageData_SO> dataDic = new Dictionary<string, StageData_SO>();
    private Dictionary<Scenes, string> sceneDic = new Dictionary<Scenes, string>();

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

        for (int i = 0; i < stages.Count; i++)
        {
            dataDic.Add(stages[i].StageID, stages[i]);
            sceneDic.Add(stages[i].SceneEnum, stages[i].StageID);
        }
    }

    public StageData_SO GetStageDataByID(string _ID)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out StageData_SO data))
        {
            return data;
        }

        CPrint.Error("StageRegistry - Cant Find");
        return null;
    }

    public string GetStageDataByEnum(Scenes _enum)
    {
        NullCheck();

        if (sceneDic.TryGetValue(_enum, out string data))
        {
            return data;
        }

        CPrint.Error("StageRegistry - Cant Find");
        return null;
    }
}
