using UnityEngine;

public class SummonMob_Soldier : BaseEnemy
{
    protected override void Awake()
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
            _offset = circleCollider2D.offset;
            _radius = circleCollider2D.radius;
        }
        else
        {
            _offset = transform.position;
            _radius = 0f;
        }

        if (_radius == 0f)
        {
            CPrint.Log($"{this} -> CircleCollider2D 없음");
        }

    }
    protected override void Start()
    {
        ItemManager.instance.Bomb += Die;
    }

    protected override void Update()
    {
        base.Update();

        if (!CanUpdate())
        {
            return;
        }

        Chase();
        CheckDamaged();
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

        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출
              
        Destroy(gameObject);
    }
}
