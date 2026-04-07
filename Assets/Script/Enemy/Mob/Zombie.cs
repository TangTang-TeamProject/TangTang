using UnityEngine;

public class Zombie : BaseEnemy
{
    [Header("¿¤¸®Æ® ¸÷ Ground UX")]
    [SerializeField] private GameObject _eliteMobGE;

    private GameObject _eliteUX;  

    public override void Init(EnemyPool pool, int idx)
    {
        base.Init(pool, idx);
        if (_isElite)
        {
            _eliteUX = Instantiate(_eliteMobGE, transform);
        }        
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

    void FixedUpdate()
    {
       
    }

    public override void Attack()
    {
        base.Attack();
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
        if (_isElite)
        {
            if (_eliteUX != null)
            {
                Destroy(_eliteUX);
            }
        }

        base.Die();
        
        // Á×¾úÀ»¶§ È¿°ú Ãß°¡ ¿¹Á¤
    }   
    
}
