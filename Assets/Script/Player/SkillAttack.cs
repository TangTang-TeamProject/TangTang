using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour
{
    // 여기는 확인+전달 하는 스크립트
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _hitRadius;

    // 임시 데미지
    private float _damage = 0f;
    private readonly Collider2D[] _hits = new Collider2D[150];
    private HashSet<IDamagables> _hitRecord = new HashSet<IDamagables>(150);
    private HashSet<IDamagables> _thisFrameRecord = new HashSet<IDamagables>(150);

    private WaitForSeconds _nextCheck = new WaitForSeconds(0.2f);
    private Coroutine _checkCo;

    private void Start()
    {
        _enemyLayer = LayerMask.GetMask("Enemy");
        _checkCo = StartCoroutine(Co_CheckTarget());
    }

    IEnumerator Co_CheckTarget()
    {
        while (true)
        {
            // 페이크 널 체크
            _hitRecord.RemoveWhere(target => target == null || (target as MonoBehaviour) == null);

            int count = Physics2D.OverlapCircleNonAlloc(transform.position, _hitRadius, _hits, _enemyLayer);

            for (int i = 0; i < count; i++)
            {
                // 마지막으로 널 체크 한번 더
                if (_hits[i] != null && _hits[i].TryGetComponent(out IDamagables target))
                {
                    _thisFrameRecord.Add(target);

                    if (_hitRecord.Add(target))
                    {
                        CPrint.Log("적 때렸음");
                        target.Hit(_damage);
                    }
                }
            }
            // 교집합만 남긴다
            _hitRecord.IntersectWith(_thisFrameRecord);
            _thisFrameRecord.Clear();

            yield return _nextCheck;
        }
    }

    private void OnDisable()
    {
        if (_checkCo != null)
        {
            StopCoroutine(_checkCo);
            _checkCo = null;
        }
        _hitRecord.Clear();
    }

    /* 방법 바꿔서 사용안함
    private void Update()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, _hitRadius, _hits, _enemyLayer);

        HashSet<IDamagables> thisFrameRecord = new HashSet<IDamagables>();

        for (int i = 0; i < count; i++)
        {
            if (_hits[i].TryGetComponent(out IDamagables target))
            {
                thisFrameRecord.Add(target);

                if (_hitRecord.Add(target))
                {
                    target.Hit(_damage);
                }
            }
        }
        // 교집합만 남긴다
        _hitRecord.IntersectWith(thisFrameRecord);
        
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
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _hitRadius);
    }
}
