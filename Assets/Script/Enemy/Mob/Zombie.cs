using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : BaseEnemy
{
    [SerializeField] private EnemyData_SO _zombieData;
    void Awake()
    {

    }

    void Update()
    {
        Chase();
    }

    public override void Attack()
    {
        
    }

    public override void Chase()
    {
        Vector2 dir = (_target.transform.position - transform.position).normalized;
        Vector2 nowPos = transform.position;

        nowPos += dir * _zombieData.Speed * Time.deltaTime;

        transform.position = nowPos;
    }

    public override void Damaged()
    {
        
    }    
    
}
