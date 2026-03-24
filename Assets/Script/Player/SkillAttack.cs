using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour, IAttackables
{
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] protected float _hitRadius;

    protected readonly Collider2D[] _hits = new Collider2D[150];
    protected HashSet<IDamagables> _hitRecord = new HashSet<IDamagables>(150);
    protected HashSet<IDamagables> _thisFrameRecord = new HashSet<IDamagables>(150);
    protected WaitForSeconds _nextCheck = new WaitForSeconds(0.2f);
    protected Coroutine _checkCo;

    // 임시 데미지
    protected float _damage = 1;


    public float Damage => _damage;
    
    private void OnEnable()
    {
        _enemyLayer = LayerMask.GetMask("Enemy");
        _checkCo = StartCoroutine(Co_CheckTarget());
    }

    public virtual IEnumerator Co_CheckTarget()
    {
        while (true)
        {
            // 페이크 널 체크
            _hitRecord.RemoveWhere(target => target.Equals(null));

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
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _hitRadius);
    }
    
}
