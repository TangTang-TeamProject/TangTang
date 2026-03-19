using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : BaseEnemy
{          

    void Update()
    {
        Chase();
        Attack();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Chase()
    {
        Vector2 dir = (_target.transform.position - transform.position).normalized;
        Vector2 nowPos = transform.position;

        nowPos += dir * _speed * Time.deltaTime;

        transform.position = nowPos;
    }

    public override void Hit(float dmg)
    {
        base.Hit(dmg);

        // 데미지 받는 효과 추가 예정
        // 현재 프로토타입
        StartCoroutine(DamagedMotion());
    }  
    
    public override void Die()
    {
        base.Die();

        // 죽었을때 효과 추가 예정
    }

    IEnumerator DamagedMotion()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;

            yield return new WaitForSeconds(0.3f);

            sr.color = Color.white;
        }
    }
    
}
