using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    protected EnemyPool _pool;
    protected GameObject _target;

    public void Init(EnemyPool pool)
    {
        _pool = pool;
    }
    public abstract void Chase();
    public abstract void Attack();

    public void Die()
    {
        _pool.Return(this);
    }

    public abstract void Damaged();

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
