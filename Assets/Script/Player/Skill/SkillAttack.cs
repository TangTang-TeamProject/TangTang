using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillAttack : MonoBehaviour, IAttackables
{
    protected SkillPool _pool;

    protected string _id;
    [SerializeField] protected float _damage = 1;
    protected float _baseDamage;
    protected float _keepTime;
    protected float _speed = 2f;
    protected float _remainTime;
    protected float _spinZ;
    protected bool _isSpin = false;

    public float Damage => _damage;

    private void Update()
    {
        Move();
        if (_isSpin)
        {
            Rotate();
        }
        
        _remainTime -= Time.deltaTime;
        if (_remainTime <= 0)
        {
            ReturnPool();
        }
        
    }

    public void Init(string id, float damage, float playerAttack, float speed, SkillPool pool, float time = 5.0f)
    {
        _id = id;
        _pool = pool;
        _baseDamage = damage;
        _damage = playerAttack * _baseDamage;
        _speed = speed;
        _keepTime = time;
        _remainTime = _keepTime;
    }

    protected virtual void Move() { }
    protected virtual void Rotate() { }
    public virtual void SetOrbit(float dist) { }
    public virtual void SetComponent(Transform center, Camera cam = null) { }

    // 플레이 도중 플레이어의 attack값이 바뀔경우 아티팩트에서 있을수도 있으니
    public void DamageChange(float playerAttack)
    {
        _damage = playerAttack * _baseDamage;
    }

    protected void ReturnPool()
    {
        _remainTime = _keepTime;
        _pool.ReturnPool(_id, this);
    }


    /*
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] protected float _hitRadius;

    protected readonly Collider2D[] _hits = new Collider2D[150];
    protected HashSet<IDamagables> _hitRecord = new HashSet<IDamagables>(150);
    protected HashSet<IDamagables> _thisFrameRecord = new HashSet<IDamagables>(150);
    protected WaitForSeconds _nextCheck = new WaitForSeconds(0.2f);
    protected Coroutine _checkCo;

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
    */
}
