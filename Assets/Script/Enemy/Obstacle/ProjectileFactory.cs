using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private ProjectilePool _pool;

    public void CreateProjectile(Vector2 nowPos)
    {
        BaseProjectile projectile = _pool.GetProjectile(transform);
        projectile.transform.position = nowPos;
        projectile.Init(_pool, _target.transform);        
        
    }
}
