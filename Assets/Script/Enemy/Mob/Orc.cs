using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : BaseEnemy
{ 
    public override void Chase()
    {
        Vector2 dir = (_target.transform.position - transform.position).normalized;
        Vector2 nowPos = transform.position;

        nowPos += dir * _speed * Time.deltaTime;

        transform.position = nowPos;
    }

    public override void Attack()
    {
        
    }

    public override void Hit(float dmg)
    {
        
    }

}
