using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private ProjectilePool _pool;

    public void CreateProjectile(Vector2 nowPos)
    {
        BaseProjectile projectile = _pool.GetProjectile(transform);

        nowPos.y += 0.2f; // 몬스터 피봇 위치에서 조금 위로 지정

        Vector2 targetPos = _target.transform.position;
        Vector2 dir = (targetPos - nowPos).normalized;        

        projectile.transform.position = nowPos + dir * 0.5f;
        projectile.Init(_pool, _target.transform);        
        
    }
}
