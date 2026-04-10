using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour, IAttackables
{    
    [SerializeField] protected EnemyData_SO _projectileSO;
    [SerializeField] protected float _aliveTime = 3f;

    protected Transform _targetPos;    
    protected ProjectilePool _pool;
    protected Vector2 _shootDir;
    protected float _spawnedTime;

    protected float _damage;

    public float Damage => _damage;
    public float Stun => 0;

    protected virtual void Awake()
    {
        _damage = _projectileSO.ContactDamage;        
    }
    public virtual void Init(ProjectilePool pool, Transform targetPos)
    {
        _targetPos = targetPos;
        _pool = pool;

        _shootDir = (targetPos.position - transform.position).normalized;
        float angle = Mathf.Atan2(_shootDir.y, _shootDir.x) * Mathf.Rad2Deg; // 플레이어 바라보는 각도 구하기
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        transform.rotation = rot;
        _spawnedTime = Timer.Instance.RealTime;
    }

    void Update()
    {
        ShootToTarget();
        Destroy();
    }

    protected virtual void ShootToTarget()
    {
        Vector2 pos = transform.position;

        pos += _shootDir * _projectileSO.BulletSpeed * Time.deltaTime;

        transform.position = pos;
    }

    protected virtual void Destroy()
    {
        if (Timer.Instance.RealTime < _spawnedTime + _aliveTime)
        {
            return;
        }

        _targetPos = null;        
        _pool.Return(this);
    }

    public void CutOff()
    {
        _targetPos = null;
        _pool.Return(this);
    }
}
