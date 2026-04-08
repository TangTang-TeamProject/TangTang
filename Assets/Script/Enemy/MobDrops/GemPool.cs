using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GemPool : MonoBehaviour
{
    [SerializeField] private GameObject _gemPrefab;
    [SerializeField] private GemType _type;
    [SerializeField] private FireFence _fireFence;

    private Queue<ExpGem> _gemPool = new Queue<ExpGem>();  

    public GemType GetPoolType()
    {
        return _type;
    }

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
        _gemPool.Enqueue(gem);
    }
}
