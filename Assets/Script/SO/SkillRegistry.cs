using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameData / SkillRegistry", fileName = "SkillRegistrySO")]
public class SkillRegistry : ScriptableObject
{
    [SerializeField]
    private List<SkillData_SO> skills = new List<SkillData_SO>();

    public IReadOnlyList<SkillData_SO> Skills => skills;

    private Dictionary<string, SkillData_SO> dataDic = new Dictionary<string, SkillData_SO>();

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

        for (int i = 0; i < skills.Count; i++)
        {
            dataDic.Add(skills[i].SkillID, skills[i]);
        }
    }

    public SkillData_SO GetSkillByID(string _ID)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out SkillData_SO data))
        {
            return data;
        }

        CPrint.Error("SkillRegistry - Cant Find");
        return null;
    }

    public SkillData_SO GetRandomSkill()
    {
        int maxLoop = 0;

        while (maxLoop < 10)
        {
            maxLoop++;

            int a = Random.Range(0, skills.Count);

            if (skills[a].IsEvo == false)
            {
                return skills[a];
            }
        }

        return skills[0];
    }
}
