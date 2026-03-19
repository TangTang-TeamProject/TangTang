using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour
{
    // 여기는 확인+전달 하는 스크립트

    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _hitRadius;

    // 데미지를 받아오는 방법이 필요함 Ex : 인터페이스, 클래스참조 등
    private float _damage;
    private HashSet<IDamagables> _hitRecord = new HashSet<IDamagables>();

    private void Start()
    {
        _enemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _hitRadius, _enemyLayer);

        HashSet<IDamagables> thisFrameRecord = new HashSet<IDamagables>();

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamagables target))
            {
                thisFrameRecord.Add(target);
                // 타격을 이미 한 대상인지 확인
                if (_hitRecord.Add(target))
                {
                    target.Hit(_damage);
                }                
            }
        }
        // 교집합만 남긴다
        _hitRecord.IntersectWith(thisFrameRecord);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _hitRadius);
    }
}
