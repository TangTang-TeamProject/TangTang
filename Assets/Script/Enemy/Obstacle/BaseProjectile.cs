using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected EnemyData_SO _projectileSO;

    protected Transform _targetPos;    
    protected ProjectilePool _pool;
    protected Vector2 _shootDir;
    public virtual void Init(ProjectilePool pool, Transform targetPos)
    {
        _targetPos = targetPos;
        _pool = pool;

        _shootDir = (targetPos.position - transform.position).normalized;
    }

    void Update()
    {
        ShootToTarget();
    }

    protected virtual void ShootToTarget()
    {
        Vector2 pos = transform.position;

        pos += _shootDir * _projectileSO.BulletSpeed * Time.deltaTime;

        transform.position = pos;
    }
}
