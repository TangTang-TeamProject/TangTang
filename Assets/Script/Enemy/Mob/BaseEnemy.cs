using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamagables
{
    [SerializeField] protected EnemyData_SO _monsterData;    

    protected EnemyPool _pool;
    protected GameObject _target;

    protected float _id;
    protected float _maxHp;
    protected float _atk;
    protected float _speed;
    protected float _atkCycle;
    protected float _bulletSpeed;
    protected float _damage;

    protected float _radius;
    protected float _nextAtk;
    protected LayerMask _playerLayer;
    
    public abstract void Chase();
     

    public void Init(EnemyPool pool)
    {
        _pool = pool;

        _id = _monsterData.EmemyID;
        _maxHp = _monsterData.HP;
        _atk = _monsterData.ATK;
        _speed = _monsterData.Speed;
        _atkCycle = _monsterData.ATKCycle;
        _bulletSpeed = _monsterData.BulletSpeed;
        _damage = _monsterData.DMG;
        _playerLayer = LayerMask.GetMask("Player");

        _radius = GetComponent<CircleCollider2D>() != null ? GetComponent<CircleCollider2D>().radius : 0f;   
        
        if (_radius == 0f)
        {
            Debug.Log($"{this} -> CircleCollider2D 없음");            
        }
        
    }

    // 몬스터 공격 함수
    public virtual void Attack()
    {
        // 공격 쿨타임(atkCycle) 검사
        if (Time.time < _nextAtk)
        {
            return;
        }

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _radius, _playerLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].CompareTag(_target.tag)) // _target 과 tag 비교
            {
                if (hit[i].gameObject.TryGetComponent(out IDamagables damagables))
                {
                    damagables.Hit(_damage);
                    _nextAtk = Time.time + _atkCycle;
                    return;
                }
                else
                {
                    CPrint.Log($"{hit[i].gameObject} IDamagables 못 찾음");
                    return;
                }                    
            }
        }
    }

    // 데미지 받는 함수
    public virtual void Hit(float damage)
    {
        _maxHp -= damage;

        if (_maxHp <= 0)
        {
            Die();
        }
    }
    
    public virtual void Die()
    {
        gameObject.SetActive(false);
        _pool.Return(this);
    }

    public virtual void SetTarget(GameObject target)
    {
        _target = target;
    }
}
