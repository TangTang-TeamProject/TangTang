using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IAttackables
{
    [Header("EnemyData SO")]
    [SerializeField] protected EnemyData_SO _monsterData;

    protected Animator _animator;
    protected EnemyPool _pool;
    protected GameObject _target;
    
    protected Vector2 _dir;
    protected float _radius;
    protected Vector2 _offset;
    protected Collider2D[] _boundaryBuffer = new Collider2D[30];
    protected Collider2D[] _dmgCheckBuffer = new Collider2D[30];

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
    
    protected float _nextDmg;
    

    public float Damage => _atk;


    protected virtual void Awake()
    {
        if (!TryGetComponent(out Animator animator))
        {
            CPrint.Warn($"{this} : Animator 연결 안됨");
            enabled = false;
            return;
        }

        _animator = animator;

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

    public virtual void Init(EnemyPool pool)
    {
        _pool = pool;        

        _id = _monsterData.EmemyID;
        _maxHp = _monsterData.HP;
        _atk = _monsterData.ATK;
        _speed = _monsterData.Speed;
        _atkCycle = _monsterData.ATKCycle;
        _bulletSpeed = _monsterData.BulletSpeed;                         
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
        
        Vector2 preventCollision = CheckBoundary();

        _dir = Vector2.Lerp(_dir, _dir + preventCollision, Time.deltaTime * 5f); // 이동 방향 보간                       

        if (_dir.magnitude > 0.001f)
        {
            _dir.Normalize();
        }
        else
        {
            _dir = Vector2.zero;
        }

        Vector2 nowPos = transform.position;

        nowPos += _dir * _speed * Time.deltaTime;

        transform.position = nowPos;
    }

    // 몬스터 공격 함수
    public virtual void Attack()
    {
        
    }

    // 데미지 받는 함수
    protected virtual void Hit(float damage)
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
        Vector2 sumDir = Vector2.zero;

        //Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position + _offset, _radius, _enemyLayer);

        int count = Physics2D.OverlapCircleNonAlloc((Vector2)transform.position + _offset, _radius, _boundaryBuffer, _enemyLayer);

        for (int i = 0; i < count; i++)
        {            
            Vector2 newDir = transform.position - _boundaryBuffer[i].transform.position; // 대상과 자신이 겹치지 않는 쪽으로의 방향벡터            
            float distance = newDir.magnitude;

            if (distance < 0.001f)
                continue;
            
            float force = 1f / (distance * distance); // 가까울수록 force 가 강해짐
            sumDir += newDir.normalized * force;
            
        }       

        return sumDir;
    }

    protected virtual void CheckDamaged()
    {
        if (Timer.Instance.TickTime == _nextDmg)
        {
            return;
        }

        _nextDmg = Timer.Instance.TickTime; // 데미지 판정 검사 _checkTime 주기마다 진입.

        //Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position + _offset, _radius * 0.9f, _playerBulletLayer);

        int count = Physics2D.OverlapCircleNonAlloc((Vector2)transform.position + _offset, 
            _radius, 
            _dmgCheckBuffer, 
            _playerBulletLayer);

        for (int i = 0; i < count; i++)
        {
            if (_dmgCheckBuffer[i].TryGetComponent(out IAttackables attackables))
            {
                Hit(attackables.Damage);
            }
        }
    }    
  
}
