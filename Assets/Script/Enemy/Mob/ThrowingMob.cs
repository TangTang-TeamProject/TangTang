
public class ThrowingMob : BaseEnemy
{    
    private ProjectileFactory _projectileFactory;
    private float _nextShoot = 0f;
    private float _shootDelay = 1f;

    protected override void Update()
    {        
        if (!CanUpdate())
            return;

        CheckDamaged();

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
