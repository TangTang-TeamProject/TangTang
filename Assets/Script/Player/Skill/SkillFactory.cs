using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory : MonoBehaviour
{
    private Dictionary<string, SkillAttack> _skillDict = new Dictionary<string, SkillAttack>();
    public SkillAttack CreateWeapon(string id)
    {
        if(!_skillDict.TryGetValue(id, out SkillAttack target))
        {
            CPrint.Log($"{id}의 스킬이 등록되어 있지 않음");
            return null;
        }

        if (target == null)
        {
            CPrint.Log($"{id}의 프리팹이 등록되어 있지 않음");
            return null;
        }

        return Instantiate(target);
    }

    public void FirstGetSkill(string id, SkillAttack prefab)
    {
        if (prefab == null)
        {
            CPrint.Error($"{id}의 프리펩 없음");
            return;
        }
        _skillDict[id] = prefab;
    }

    public void SkillEvol(string id, string evolutionId, SkillAttack prefab)
    {
        if (prefab == null)
        {
            CPrint.Error($"{id}의 프리펩 없음");
            return;
        }
        _skillDict.Remove(id);
        _skillDict[evolutionId] = prefab;
    }
}
