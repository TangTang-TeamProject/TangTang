using UnityEngine;

public class SummonMob_Wizard : BaseEnemy
{
    [SerializeField] private GameObject _projectile;

    private float _nextShoot = 0f;

    private bool _isAttacking = false;

    private float _attackDuration = 0.8f;
    private float _attackDelay = 0.2f;
    private bool _isDelay = false;

    private string animParam = "IsMoving";
   

    protected override void Start()
    {
        ItemManager.instance.Bomb += Die;
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
        _hitTime = _hitTimer; // 계속 최신 기준 hit 로 변경.

        if (_sr == null)
        {
            _sr = GetComponent<SpriteRenderer>();
        }
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        _nextShoot = Timer.Instance.RealTime + _atkCycle;
    }

    protected override void Update()
    {
        base.Update();

        if (_target == null) // 타겟 없으면 return
        {
            return;
        }

        if (!CanUpdate())
            return;

        CheckDamaged();

        if (_isAttacking)
        {
            _attackDuration -= Time.deltaTime;

            if (_isDelay)
            {
                _attackDelay -= Time.deltaTime;
                if (_attackDelay <= 0f)
                {
                    Projectile_SummonMob proj = Instantiate(_projectile, transform.position, Quaternion.identity).GetComponent<Projectile_SummonMob>();
                    proj.Init(null, _target.transform);
                    _attackDelay = 0.2f;
                    _isDelay = false;
                }
            }

            if (_attackDuration <= 0f)
            {
                _isAttacking = false;
                _attackDuration = 0.8f;
                _animator.SetBool(animParam, true);
            }

            return;
        }

        if (Timer.Instance.RealTime >= _nextShoot)
        {
            _nextShoot = Timer.Instance.RealTime + _atkCycle;
            Attack();
        }


        Chase();
    }


    public override void Attack()
    {
        _isAttacking = true;
        _animator.SetBool(animParam, false);
        _isDelay = true;
    }

    public override void Chase()
    {
        base.Chase();
        _animator.SetBool(animParam, true);
    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출

        Destroy(gameObject);
    }
}
