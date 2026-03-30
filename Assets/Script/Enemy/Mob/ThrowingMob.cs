
public class ThrowingMob : BaseEnemy, IAttackables
{    
    private ProjectileFactory _projectileFactory;
    private float _nextShoot = 0f;

    protected override void Update()
    {
        if (!CanUpdate())
            return;

        Attack();
        Chase();
        CheckDamaged();
    }

    void FixedUpdate()
    {
        
    }
    

    public override void Attack()
    {
        if (Timer.Instance.GameTime < _nextShoot)
        {
            return;
        }

        _nextShoot = Timer.Instance.GameTime + _atkCycle;

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

    public override void Die()
    {
        base.Die();

        // 죽었을때 효과 추가 예정
    }
    
    public void SetProjectileFactory(ProjectileFactory projectileFactory)
    {
        _projectileFactory = projectileFactory;
    }
}
