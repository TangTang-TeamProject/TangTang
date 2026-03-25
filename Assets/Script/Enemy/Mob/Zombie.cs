using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : BaseEnemy
{          

    void Update()
    {        
        Attack();
        CheckDamaged();
    }

    void FixedUpdate()
    {
        Chase();
        
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
