using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class STG2_FinalBoss : BaseEnemy
{
    [Header("HP Bar 연결")]
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private Image _HPBarImage;

    [Header("랜덤 박스")]
    [SerializeField] private GameObject _itemParent;
    [SerializeField] private GameObject _randomBox;

    [Header("투사체 연결")]
    [SerializeField] private GameObject _guidedProjectile;
    [SerializeField] private GameObject _allDirProjectile;
    [SerializeField] private float _shootCount = 8;

    [Header("소환할 몬스터")]
    [SerializeField] private GameObject _enemyType1;
    [SerializeField] private GameObject _enemyType2;
    [SerializeField] private int _summonCount = 5;

    private Vector2 _shootDir;
    private Vector2 _shootOrigin;
    private float _nextShoot;

    private SpriteRenderer[] _spriteRenderers;
    private List<SpriteRenderer> _activeList = new List<SpriteRenderer>();
    private Dictionary<SpriteRenderer, Color> _colorMap = new Dictionary<SpriteRenderer, Color>();

    private string _animString_Move = "1_Move";
    private string _animString_Atk = "2_Attack";

    private Vector2[] _allDirList = {Vector2.up, Vector2.down, Vector2.right, Vector2.left};


    protected override void Awake()
    {
        base.Awake();

        _HPBar.SetActive(true);
        _HPBarImage.fillAmount = 1f;


        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _activeList.Add(_spriteRenderers[i]);
            _colorMap.Add(_spriteRenderers[i], _spriteRenderers[i].color);
        }

        _animator = GetComponentInChildren<Animator>();

        _nextShoot = Timer.Instance.RealTime + _atkCycle;

        if (TryGetComponent(out CircleCollider2D circleCollider2D))
        {
            _col = GetComponent<CircleCollider2D>();
        }
        else
        {
            CPrint.Log($"{this} -> CircleCollider2D 없음");
        }
    }

    public override void Init(EnemyPool pool, int idx)
    {
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
        _mobType = _monsterData.EnemyType;

        _nextShoot = Timer.Instance.RealTime + _atkCycle;
    }


    protected override void Update()
    {
        if (_target == null) // 타겟 없으면 return
        {
            return;
        }


        if (_isHit)
        {
            _hitTime -= Time.deltaTime;
            _speed = 0f; // 멈칫하는 모션

            if (_hitTime <= 0f)
            {
                _isHit = false;
                _hitTime = _hitTimer;
                _speed = _monsterData.MoveSpeed; // 스피드 복구
                for (int i = 0; i < _activeList.Count; i++)
                {
                    _activeList[i].color = _colorMap[_activeList[i]];
                }
            }
        }

        if (!CanUpdate())
        {
            return;
        }

        if (Timer.Instance.RealTime >= _nextShoot)
        {
            int rand = UnityEngine.Random.Range(0, 3);
            _animator.SetTrigger(_animString_Atk);
            if (rand == 0)
            {
                StartCoroutine(ExecutePattern(0));
            }
            else if (rand == 1)
            {
                StartCoroutine(ExecutePattern(1));
            }
            else
            {
                StartCoroutine(ExecutePattern(2));
            }

            _nextShoot = Timer.Instance.RealTime + _atkCycle;
        }

        Chase();
        CheckDamaged();
    }

    void LateUpdate()
    {
        MoveIntoBattlezone();
    }

    protected override void Hit(IAttackables attackables)
    {
        _maxHp -= attackables.Damage;
        _isHit = true;
        _hitTime = _hitTimer; // 계속 최신 기준 hit 로 변경.        
        //_animator.SetBool(_animString_Move, false);
        //_animator.SetTrigger(_animString_Damaged);

        float ratio = _maxHp / _monsterData.HP;
        _HPBarImage.fillAmount = ratio;

        for (int i = 0; i < _activeList.Count; i++)
        {
            _activeList[i].color = Color.red;
        }

        if (_maxHp <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출

        Timer.Instance.IsBossDie(false);
        _HPBar.SetActive(false);
        Instantiate(_randomBox, transform.position, Quaternion.identity, _itemParent.transform);        
        Destroy(gameObject);
    }

    public override void Chase()
    {
        base.Chase();
        _animator.SetBool(_animString_Move, true);
    }

    private void ShootGuided()
    {
        _shootOrigin = transform.position;
        _shootOrigin.y += 0.5f;

        _shootDir = _target.transform.position - transform.position;
        _shootDir.Normalize();

        _shootOrigin += _shootDir;

        Projectile_Guided projectile = Instantiate(_guidedProjectile, _shootOrigin, Quaternion.identity).GetComponent<Projectile_Guided>();
        projectile.Init(null, _target.transform);
    }

    private void SummonMobs()
    {        
        int randType = UnityEngine.Random.Range(0, 2);

        if (randType == 0)
        {
            SummonMob_Soldier enemy = Instantiate(_enemyType1, RandSpawnPoint(), Quaternion.identity).GetComponent<SummonMob_Soldier>();
            enemy.Init(null, 0);
            enemy.SetTarget(_target);
        }
        else
        {
            SummonMob_Wizard enemy = Instantiate(_enemyType2, RandSpawnPoint(), Quaternion.identity).GetComponent<SummonMob_Wizard>();
            enemy.Init(null, 0);
            enemy.SetTarget(_target);
        }
        
    }

    private void ShootAllDir()
    {
        int randDir = UnityEngine.Random.Range(0, 4);

        _shootOrigin = transform.position;
        _shootOrigin.y += 0.5f;

        _shootOrigin += _allDirList[randDir];

        Projectile_AllDir projectile = Instantiate(_allDirProjectile, _shootOrigin, Quaternion.identity).GetComponent<Projectile_AllDir>();
        projectile.Init(null, _target.transform);
        projectile.SetShootDir(_allDirList[randDir]);
    }

    private Vector2 RandSpawnPoint()
    {
        float maxX = 14f;
        float maxY = 9f;

        float randX = UnityEngine.Random.Range(-maxX, maxX);
        float randY = UnityEngine.Random.Range(-maxY, maxY);

        return new Vector2(randX, randY);
    }

    IEnumerator ExecutePattern(int i)
    {        
        if (i == 0)
        {
            for (int j = 0; j < 3; j++)
            {
                ShootGuided();
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if (i == 1)
        {
            for (int j = 0; j < _summonCount; j++)
            {
                SummonMobs();
                yield return new WaitForSeconds(0.1f);
            }           
        }
        else
        {
            for (int j = 0; j < _shootCount; j++)
            {
                ShootAllDir();
                yield return new WaitForSeconds(0.2f);  
            }
        }
    }
}
