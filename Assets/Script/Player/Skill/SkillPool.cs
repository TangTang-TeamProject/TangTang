using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPool : MonoBehaviour
{
    [SerializeField] private SkillFactory _skillFactory;

    private readonly Dictionary<string, Queue<SkillAttack>> _skillDict = new Dictionary<string, Queue<SkillAttack>>();
    public void CreateWeapon(int type)
    {
        // Use에서 사용할게 부족하면 팩토리에게 생산해달라고 한다
    }

    public void Use(int type)
    {

    }

    public void ReturnPool(string tag, SkillAttack target)
    {
        if (!_skillDict.TryGetValue(tag, out Queue<SkillAttack> queue))
        {
            queue = new Queue<SkillAttack>();
            _skillDict.Add(tag, queue);
            _skillDict[tag] = queue;
        }
        target.gameObject.SetActive(false);
    }
}
