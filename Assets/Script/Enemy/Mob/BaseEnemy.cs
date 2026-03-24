using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamagables
{
    [Header("EnemyData SO")]
    [SerializeField] protected EnemyData_SO _monsterData;
    [Header("Gizmos 토글")]
    [SerializeField] protected bool _gizmosOn = false;

    protected EnemyPool _pool;
    protected GameObject _target;
    
    protected Vector2 _dir;
    protected float _radius;
    protected Vector2 _offset;

    protected LayerMask _playerLayer;
    protected LayerMask _enemyLayer;
    protected LayerMask _playerBulletLayer;

    private string _playerString = "Player";
    private string _enemyString = "Enemy";
    private string _playerBulletString = "PlayerBullet";

    protected float _id;
    protected float _maxHp;
    protected float _atk;
    protected float _speed;
    protected float _atkCycle;
    protected float _bulletSpeed;
    protected float _damage;
    
    protected float _nextDmg;
    protected float _checkTime = 0.2f;


    private void Awake()
    {
        _playerLayer = LayerMask.GetMask(_playerString);
        _enemyLayer = LayerMask.GetMask(_enemyString);
        _playerBulletLayer = LayerMask.GetMask(_playerBulletString);        
        
        _offset = GetComponent<CircleCollider2D>() != null ? GetComponent<CircleCollider2D>().offset : (Vector2)transform.position;
        _radius = GetComponent<CircleCollider2D>() != null ? GetComponent<CircleCollider2D>().radius : 0f;

        if (_radius == 0f)
        {
            CPrint.Log($"{this} -> CircleCollider2D 없음");
        }
    }

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
    }

    public virtual void Chase()
    {
        if (_target == null) // 타겟 없으면 return
        {
            return;
        }

        _dir = _target.transform.position - transform.position; // 플레이어로의 방향벡터

        float buffer = 0.1f;
        Quaternion rot = transform.rotation;

        if (_dir.x < 0 - buffer)
        {
            rot.y = 0f;
        }
        else if (_dir.x > 0 + buffer)
        {
            rot.y = 180f;
        }

        transform.rotation = rot; // 추적 방향에 따른 방향 전환 적용

        if (_dir.magnitude > 0.001f)
        {
            _dir.Normalize();
        }
        else
        {
            _dir = Vector2.zero;
        }

        Vector2 preventCollision = CheckBoundary().normalized;

        _dir += preventCollision; // 추적 방향에 collision 방지용 방향벡터 합산.
               

        Vector2 nowPos = transform.position;

        nowPos += _dir * _speed * Time.deltaTime;

        transform.position = nowPos;
    }

    // 몬스터 공격 함수
    public virtual void Attack()
    {
        //if (Time.time < _checkTime)
        //{
        //    return;
        //}

        //_checkTime = Time.time + 0.2f; // 함수 진입 0.2초 주기로 설정

        //// 공격 쿨타임(atkCycle) 검사
        //if (Time.time < _nextAtk)
        //{
        //    return;
        //}
        
        //Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _radius, _playerLayer);
        //for (int i = 0; i < hit.Length; i++)
        //{
        //    if (hit[i].CompareTag(_target.tag)) // _target 과 tag 비교
        //    {
        //        if (hit[i].gameObject.TryGetComponent(out IDamagables damagables))
        //        {
        //            damagables.Hit(_damage);
        //            _nextAtk = Time.time + _atkCycle;
        //            return;
        //        }
        //        else
        //        {
        //            CPrint.Log($"{hit[i].gameObject} IDamagables 못 찾음");
        //            return;
        //        }                    
        //    }
        //}
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
        gameObject.SetActive(false); // 몬스터 사망
        _pool.Return(this);
    }
   

    public virtual void SetTarget(GameObject target)
    {
        _target = target;
    }

    protected virtual Vector2 CheckBoundary()
    {
        Vector2 dirToAdd = new Vector2();

        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position + _offset, _radius, _enemyLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            Vector2 newDir = transform.position - hits[i].transform.position; // 대상과 자신이 겹치지 않는 쪽으로의 방향벡터            
            dirToAdd += newDir; 
        }

        return dirToAdd;
    }

    protected virtual void GetDamaged()
    {
        if (Time.time < _nextDmg)
        {
            return;
        }

        _nextDmg = Time.time + _checkTime; // 데미지 판정 검사 _checkTime 주기마다 진입.

        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position + _offset, _radius * 0.9f, _playerBulletLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            
        }
    }

    private void OnDrawGizmos()
    {
        
    }
}
