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
                _eliteUX.SetActive(true);
            }
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
                _eliteUX.SetActive(false);
            }
        }

        base.Die();
        
        // Á×¾úÀ»¶§ È¿°ú Ãß°¡ ¿¹Á¤
    }   
    
}
