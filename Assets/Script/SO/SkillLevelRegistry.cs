using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / SkillLevelRegistry", fileName = "SkillLevelRegistrySO")]
public class SkillLevelRegistry : ScriptableObject
{
    [SerializeField]
    private List<SkillLevel_SO> levels = new List<SkillLevel_SO>();

    public IReadOnlyList<SkillLevel_SO> Artifacts => levels;

    private Dictionary<string, Dictionary<int, SkillLevel_SO>> dataDic = new Dictionary<string, Dictionary<int, SkillLevel_SO>>();

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

        for (int i = 0; i < levels.Count; i++)
        {
            if (!dataDic.ContainsKey(levels[i].SkillID))
            {
                dataDic.Add(levels[i].SkillID, new Dictionary<int, SkillLevel_SO>());
            }

            dataDic[levels[i].SkillID].Add(levels[i].Level, levels[i]);
        }
    }

    public SkillLevel_SO GetSkillDataByIDLevel(string _ID, int _level)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out Dictionary<int, SkillLevel_SO> data))
        {
            return data[_level];
        }

        CPrint.Error("SkillRegistry - Cant Find");
        return null;
    }
}
