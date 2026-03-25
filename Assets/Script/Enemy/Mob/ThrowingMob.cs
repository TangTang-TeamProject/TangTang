using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingMob : BaseEnemy
{
    [SerializeField] private GameObject _projectile;

    private float _nextShoot = 0f;

    protected override void Awake()
    {
        base.Awake();
        if (_projectile == null)
        {
            CPrint.Log($"{this} : Projectile 연결 안됨");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        Attack();
        CheckDamaged();
    }

    void FixedUpdate()
    {
        Chase();
    }

    public override void Attack()
    {
        if (Timer.Instance.GameTime < _nextShoot)
        {
            return;
        }

        _nextShoot = Timer.Instance.GameTime + _atkCycle;

        
    }

    public override void Chase()
    {
        base.Chase();
    }

    protected override void Hit(float dmg)
    {
        base.Hit(dmg);

    }

    public override void Die()
    {
        base.Die();

        // 죽었을때 효과 추가 예정
    }
}
