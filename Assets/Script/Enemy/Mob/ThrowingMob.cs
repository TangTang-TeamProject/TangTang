
using UnityEngine;

public class ThrowingMob : BaseEnemy
{
    [Header("엘리트 몹 Ground UX")]
    [SerializeField] private GameObject _eliteMobGE;

    private ProjectileFactory _projectileFactory;
    private float _nextShoot = 0f;
   
    private bool _isAttacking = false;

    private float _attackDuration = 1f;
    private float _attackDelay = 0.2f;
    private bool _isDelay = false;

    private string animParam = "IsMoving";

    private GameObject _eliteUX;
    public override void Init(EnemyPool pool, int idx)
    {
        base.Init(pool, idx);
        if (_isElite)
        {
            if (_eliteUX != null)
            {
                _eliteUX.SetActive(true);
            }
            else
            {
                _eliteUX = Instantiate(_eliteMobGE, transform);
            }
        }
        else
        {
            if (_eliteUX != null)
            {
                _eliteUX.SetActive(false);
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_isDead)
        {
            return;
        }

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
                    _projectileFactory.CreateProjectile(transform.position);
                    _attackDelay = 0.2f;
                    _isDelay = false;
                }
            }
                        
            if (_attackDuration <= 0f)
            {
                _isAttacking = false;
                _attackDuration = 1f;
                _animator.SetBool(animParam, true);
                _speed = _monsterData.MoveSpeed;
            }

            return;
        }

        if (Timer.Instance.GameTime >= _nextShoot)
        {
            _nextShoot = Timer.Instance.GameTime + _atkCycle;
            Attack();
            return;
        }
    
                       
        Chase();
    }

    void FixedUpdate()
    {
        
    }
    

    public override void Attack()
    {
        _isAttacking = true;
        _animator.SetBool(animParam, false);
        _speed = 0f;
        _isDelay = true;        
    }
   

    public override void Chase()
    {
        base.Chase();

    }

   
    public void SetProjectileFactory(ProjectileFactory projectileFactory)
    {
        _projectileFactory = projectileFactory;
    }

    public override void Die()
    {
        if (_isElite)
        {
            if (_eliteUX != null)
            {
                Destroy(_eliteUX);
            }
        }

        base.Die();

        // 죽었을때 효과 추가 예정
    }
}
