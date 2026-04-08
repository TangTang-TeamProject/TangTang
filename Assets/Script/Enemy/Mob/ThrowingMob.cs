
using UnityEngine;

public class ThrowingMob : BaseEnemy
{
    [Header("엘리트 몹 Ground UX")]
    [SerializeField] private GameObject _eliteMobGE;

    private ProjectileFactory _projectileFactory;
    private float _nextShoot = 0f;
   
    private bool _isAttacking = false;
    private float _attackDuration = 1f;

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

            if (_attackDuration <= 0f)
            {
                _isAttacking = false;
                _attackDuration = 1f;
                _animator.SetBool(animParam, true);
            }

            return;
        }

        if (Timer.Instance.GameTime >= _nextShoot)
        {
            _nextShoot = Timer.Instance.GameTime + _atkCycle;
            Attack();            
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
        _projectileFactory.CreateProjectile(transform.position);
    }
   

    public override void Chase()
    {
        base.Chase();

    }

    protected override void Hit(float dmg)
    {
        base.Hit(dmg);

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
