using UnityEngine;

public class Zombie : BaseEnemy
{          

    protected override void Update()
    {
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
        base.Die();

        // 죽었을때 효과 추가 예정
    }   
    
}
