using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IAttackables
{
    [Header("EnemyData SO")]
    [SerializeField] protected EnemyData_SO _monsterData;
    [Header("피격 설정")]    
    [SerializeField] protected float _hitTimer = 0.1f;

    protected bool _isHit = false;
    protected float _hitTime;

    // 생성 시 초기화 변수들
    protected Animator _animator;
    protected SpriteRenderer _sr;
    

    protected EnemyPool _pool;
    protected GameObject _target;
    protected bool _isElite = false;

    protected int _idx; // 그룹으로 나눌 기준이 될 인덱스
    protected float _minX = -15f;
    protected float _maxX = 15f;
    protected float _minY = -15f;
    protected float _maxY = 15f;
       
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

    protected string _id;
    protected float _maxHp;
    protected float _contactDamage;
    protected float _speed;
    protected float _expDrop;
    protected float _atkCycle;
    protected float _bulletSpeed;
    protected EnemyType _mobType;
    
    protected float _nextDmg;
    
    public float Damage => _contactDamage;
    public EnemyType MobType => _mobType;
    public float ExpDrop => _expDrop;
    
    public void IsElite(bool tf)
    {
        _isElite = tf;
    }
    

    protected virtual void Awake()
    {
        if (!TryGetComponent(out Animator animator))
        {
            CPrint.Warn($"{this} : Animator 연결 안됨");
            enabled = false;
            return;
        }

        _animator = animator;
        _sr = GetComponent<SpriteRenderer>();
        
        _hitTime = _hitTimer;

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

    protected void Start()
    {
        Timer.Instance.BossSpawn += RemoveWhenBoss;
        ItemManager.instance.Bomb += RemoveWhenBoss;
    }

    protected virtual bool CanUpdate()
    {
        return Time.frameCount % 2 == _idx % 2;
    }

    protected virtual void Update()
    {        
        if (_isHit)
        {
            _hitTime -= Time.deltaTime;
            _speed = 0f; // 멈칫하는 모션

            if (_hitTime <= 0f)
            {
                _isHit = false;
                _hitTime = _hitTimer;
                _speed = _monsterData.MoveSpeed; // 스피드 복구
                _sr.color = Color.white;
            }
        }
    }

    public virtual void Init(EnemyPool pool, int idx)
    {
        _pool = pool;
        _idx = idx; 

        _id = _monsterData.EmemyID;
        _maxHp = _monsterData.HP;
        _contactDamage = _monsterData.ContactDamage;
        _speed = _monsterData.MoveSpeed;
        _atkCycle = _monsterData.ATKCycle;
        _bulletSpeed = _monsterData.BulletSpeed;
        _expDrop = _monsterData.ExpDrop;
        if (_isElite)
        {
            _mobType = EnemyType.Elite;
            transform.localScale = new Vector3(1.5f, 1.5f, 0); // 엘리트몹 크기 변경
        }
        else
        {
            _mobType = _monsterData.EnemyType;
            transform.localScale = new Vector3(_monsterData.SizeScale, _monsterData.SizeScale, 0);
        }
            
        _isHit = false;
        _sr.color = Color.white;
        _hitTime = _hitTimer; // 계속 최신 기준 hit 로 변경.
                                 
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
        
        Vector2 preventCollision = CheckBoundary(); // 몹 간 겹침 방지 방향벡터

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
        _isHit = true;
        _hitTime = _hitTimer; // 계속 최신 기준 hit 로 변경.
        _sr.color = Color.red;                      

        if (_maxHp <= 0)
        {
            Die();
        }
    }
    
    public virtual void Die()
    {                     
        _isElite = false;             
        gameObject.SetActive(false); // 몬스터 사망
        _pool.Return(this);                   
    }

    // 보스전 시작시 몬스터 정리
    public void RemoveWhenBoss()
    {
        gameObject.SetActive(false);
        _pool.Add(this);
    }

    public virtual void SetTarget(GameObject target)
    {
        _target = target;
    }
   
    protected virtual Vector2 CheckBoundary()
    {        
        Vector2 sumDir = Vector2.zero;

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

    // 배틀존 안으로 제한.
    protected void MoveIntoBattlezone()
    {
        Vector2 nowPos = transform.position;

        if (nowPos.x > _maxX)
        {
            nowPos.x = _maxX;
        }
        else if (nowPos.x < _minX)
        {
            nowPos.x = _minX;
        }

        if (nowPos.y > _maxY)
        {
            nowPos.y = _maxY;
        }
        else if (nowPos.y < _minY)
        {
            nowPos.y = _minY;
        }

        transform.position = nowPos;
    }
}
