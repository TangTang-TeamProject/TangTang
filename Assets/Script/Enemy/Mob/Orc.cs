using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : BaseEnemy
{
    [SerializeField] private EnemyData_SO _orcData;

    public override void Chase()
    {
        Vector2 dir = (_target.transform.position - transform.position).normalized;
        Vector2 nowPos = transform.position;

        nowPos += dir * _orcData.Speed * Time.deltaTime;

        transform.position = nowPos;
    }

    public override void Attack()
    {
        
    }

    public override void Damaged()
    {
        
    }

}
