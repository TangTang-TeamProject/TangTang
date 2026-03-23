using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemFactory : MonoBehaviour
{
    [SerializeField] private Transform _enemy; 
    [SerializeField] private GemPool _pool;
    
    public ExpGem CreateGem(Vector2 pos)
    {
        ExpGem gem = _pool.GetGem(transform);
        gem.Init(_pool);

        gem.transform.position = pos;
        return gem;
    }
}
