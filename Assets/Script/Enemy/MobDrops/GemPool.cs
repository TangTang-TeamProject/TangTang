using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPool : MonoBehaviour
{
    [SerializeField] private GameObject _gemPrefab;

    private Queue<ExpGem> _gemPool = new Queue<ExpGem>();
    
    public void Add(ExpGem gem)
    {
        _gemPool.Enqueue(gem);
    }

    public ExpGem GetGem(Transform parent)
    {
        if (_gemPool.Count > 0)
        {
            ExpGem gem = _gemPool.Dequeue();
            gem.gameObject.SetActive(true);
            return gem;
        }

        GameObject go = Instantiate(_gemPrefab, parent.transform);
        return go.GetComponent<ExpGem>();
        
    }

    public void Return(ExpGem gem)
    {
        gem.gameObject.SetActive(false);
        _gemPool.Enqueue(gem);
    }
}
