using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
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
    protected bool _isStun = false;
    protected float _stunValue = -0.5f; // 스턴 강도 <-------------------- 수치 조정

    // 생성 시 초기화 변수들
    protected Animator _animator;
    protected SpriteRenderer _sr;
    protected CircleCollider2D _col;

    protected EnemyPool _pool;
    protected GameObject _target;

    // 엘리트몹으로 소환되는지
    protected bool _isElite = false;

    protected int _idx; // 그룹으로 나눌 기준이 될 인덱스
    protected float _minX = -15f;
    protected float _maxX = 15f;
    protected float _minY = -10f;
    protected float _maxY = 10f;
       
    protected Vector2 _dir;
    protected float _radius;
    protected Vector2 _offset;
    protected Collider2D[] _boundaryBuffer = new Collider2D[30];
    protected Collider2D[] _dmgCheckBuffer = new Collider2D[30];
    protected HashSet<IAttackables> _thisFrameRecord = new HashSet<IAttackables>(30);
    protected HashSet<IAttackables> _hitRecords = new HashSet<IAttackables>(30);

    protected LayerMask _playerLayer;
    protected LayerMask _enemyLayer;
    protected LayerMask _playerBulletLayer;

    protected string _playerString = "Player";
    protected string _enemyString = "Enemy";
    protected string _playerBulletString = "PlayerBullet";
    protected string _defenceBulletString = "DefenceBullet";

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
    public float Stun => 0;

    public string Id => _id;
    public EnemyType MobType => _mobType;
    public float ExpDrop => _expDrop;
    
    public void IsElite(bool tf)
    {
        _isElite = tf;
    }    
    

    protected virtual void Awake()
    {        
        if (TryGetComponent(out Animator animator))
        {
            _animator = animator;
        }

        _sr = GetComponent<SpriteRenderer>();

        _hitTime = _hitTimer;

        _playerLayer = LayerMask.GetMask(_playerString);
        _enemyLayer = LayerMask.GetMask(_enemyString);
        _playerBulletLayer = LayerMask.GetMask(_playerBulletString, _defenceBulletString);  
        
        
        if (TryGetComponent(out CircleCollider2D circleCollider2D))
        {
            _col = GetComponent<CircleCollider2D>();
        }
        else
        {
            CPrint.Log($"{this} -> CircleCollider2D 없음");
        } 
      
        
    }

    protected virtual void Start()
    {
        if (_monsterData.EnemyType == EnemyType.Boss)
            return;

        Timer.Instance.BossSpawn += RemoveWhenBoss;
        ItemManager.instance.Bomb += Die;
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
            if (_isStun)
            {
                if (_mobType == EnemyType.Boss)
                {
                    _speed = 0f;
                }
                else
                {
                    _speed = _stunValue; // 넉백 ------------------> 넉백 강도 _stunValue 로 조정
                }                   
            }
            else
            {
                _speed = 0f; // 경직
            }
               
            if (_hitTime <= 0f)
            {
                _isHit = false;
                _isStun = false;
                _hitTime = _hitTimer;
                _speed = _monsterData.MoveSpeed; // 스피드 복구
                _sr.color = Color.white;
            }
        }
    }

    public virtual void Init(EnemyPool pool, int idx)
    {
        if (_sr == null)
        {
            _sr = GetComponent<SpriteRenderer>();
        }
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();   
        }

        if (pool != null)
        {
            _pool = pool;
        }        
        _idx = idx; 

        _id = _monsterData.EnemyID;
        _maxHp = _monsterData.HP;
        _contactDamage = _monsterData.ContactDamage;
        _speed = _monsterData.MoveSpeed;
        _atkCycle = _monsterData.ATKCycle;
        _bulletSpeed = _monsterData.BulletSpeed;
        _expDrop = _monsterData.ExpDrop;

        if (_mobType != EnemyType.Boss)
        {
            if (Timer.Instance.GameTime <= 240f)
            {
                _maxHp = _monsterData.HP;
                _expDrop = _monsterData.ExpDrop;
            }
            else if (Timer.Instance.GameTime <= 480f)
            {
                _maxHp = _monsterData.HP * 2f;                
            }
            else
            {
                _maxHp = _monsterData.HP * 4f;                
            }
        }        

        if (_isElite)
        {
            _mobType = EnemyType.Elite;
            transform.localScale = new Vector3(2.5f, 2.5f, 0); // 엘리트몹 크기 변경
            int randExp = UnityEngine.Random.Range(10, 30);
            _expDrop = randExp;
            _maxHp *= 10;
        }
        else
        {
            _mobType = _monsterData.EnemyType;
            transform.localScale = new Vector3(_monsterData.SizeScale, _monsterData.SizeScale, 0);
                   
        }
            
        _isHit = false;
        _isStun = false;
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
    protected virtual void Hit(IAttackables attackables)
    {        
        _maxHp -= attackables.Damage;
        SoundManager.Instance.PlaySfx(ESfxType.EnemyHit);
        if (!_isStun) // _isStun 이 false 일때만 진입
        {
            if (attackables.Stun > 0)
            {
                _isStun = true;
            }
            else
            {
                _isStun = false;
            }
        }        
        _isHit = true;        
        _hitTime = _hitTimer; // 계속 최신 기준 hit 로 변경.
        _sr.color = new Color(1f, 0.5f, 0.5f);

        if (_maxHp <= 0)
        {
            Die();
        }
    }
    
    public virtual void Die()
    {                     
        _isElite = false;        
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        StartCoroutine(DieRoutine());
        //gameObject.SetActive(false); // 몬스터 사망        
        //_pool.Return(this);                   
    }

    IEnumerator DieRoutine()
    {
        _sr.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        _sr.color = Color.white;
        gameObject.SetActive(false); // 몬스터 사망        
        _pool.Return(this);
    }

    // 보스전 시작시 몬스터 정리
    public void RemoveWhenBoss()
    {
        _isElite = false;
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
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
        if (Timer.Instance.RealTime >= _nextDmg)
        {
            _hitRecords.Clear();
            _nextDmg = Timer.Instance.RealTime + 0.5f; // 데미지 판정 검사 _checkTime 주기마다 진입.        
            return;
        }


        Vector2 center = _col.transform.TransformPoint(_col.offset);
        float radius = _col.radius * Mathf.Max(_col.transform.lossyScale.x, _col.transform.lossyScale.y);

        int count = Physics2D.OverlapCircleNonAlloc(center, 
            radius, 
            _dmgCheckBuffer, 
            _playerBulletLayer);

        for (int i = 0; i < count; i++)
        {
            if (_dmgCheckBuffer[i].TryGetComponent(out IAttackables attackables))
            {    
                _thisFrameRecord.Add(attackables);

                if (_hitRecords.Add(attackables))
                {
                    Hit(attackables);
                    attackables.GetDestroy();
                }                
            }
        }

        _hitRecords.IntersectWith(_thisFrameRecord);
        _thisFrameRecord.Clear();
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

    public void GetDestroy()
    {

    }
}
