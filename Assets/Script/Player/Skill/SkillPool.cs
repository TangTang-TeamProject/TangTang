using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPool : MonoBehaviour
{
    [SerializeField] private SkillFactory _skillFactory;
    [SerializeField] private Transform _godObject;

    private Dictionary<string, Queue<SkillAttack>> _skillDict = new Dictionary<string, Queue<SkillAttack>>();
    
    public void InitCreateSkill(string tag, int num)
    {
        if (!_skillDict.TryGetValue(tag, out Queue<SkillAttack> queue))
        {
            queue = new Queue<SkillAttack>();
            _skillDict[tag] = queue;
        }
        for (int i = 0; i < num; i++)
        {
            SkillAttack target = _skillFactory.CreateWeapon(tag);
            if (target == null)
            {
                CPrint.Error($"{gameObject.name}에 null반환됨 인스펙터 태그 확인");
                break;
            }
            target.transform.SetParent(transform);
            target.gameObject.SetActive(false);

            queue.Enqueue(target);
        }
    }

    public SkillAttack UseSkill(string tag)
    {
        SkillAttack target;
        if (_skillDict[tag].Count == 0)
        {
            target = _skillFactory.CreateWeapon(tag);
        }
        else
        {
            target = _skillDict[tag].Dequeue();
        }        
        target.transform.SetParent(_godObject);
        return target;
    }

    public void ReturnPool(string tag, SkillAttack target)
    {
        if (!_skillDict.TryGetValue(tag, out Queue<SkillAttack> queue))
        {
            queue = new Queue<SkillAttack>();
            _skillDict[tag] = queue;
        }
        target.transform.SetParent(transform);
        target.gameObject.SetActive(false);
        queue.Enqueue(target);
    }
}
