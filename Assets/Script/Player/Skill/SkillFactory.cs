using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory : MonoBehaviour
{
    [SerializeField] private SkillAttack[] _skillPrefab;
    public SkillAttack CreateWeapon(string tag)
    {
        SkillAttack target = null;
        foreach (SkillAttack skillP in _skillPrefab)
        {
            if(skillP.tag == tag)
            {
                target = skillP;
                break;
            }
        }
        if (target == null)
        {
            CPrint.Log($"{tag}의 프리팹이 등록되어 있지 않음");
            return null;
        }

        return Instantiate(target);
    }
}
