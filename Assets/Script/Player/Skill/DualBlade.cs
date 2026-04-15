using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualBlade : SkillAttack
{
    [SerializeField] private Transform _spawner;
    [SerializeField] private CircleCollider2D _collider;
    [SerializeField] private LayerMask _hitLayer;
    [SerializeField] private string _hitLayerString = "EnemyBullet";
    [SerializeField] private ParticleSystem[] _particle = new ParticleSystem[2];

    private float _hitRadius;
    private readonly Collider2D[] _hits = new Collider2D[20];
    private HashSet<BaseProjectile> _hitRecord = new HashSet<BaseProjectile>(20);
    private WaitForSeconds _nextCheck = new WaitForSeconds(0.1f);
    private Coroutine _checkCo;

    private void Awake()
    {
        _hitLayer = LayerMask.GetMask(_hitLayerString);
        if (_particle == null)
        {
            CPrint.Warn($"{gameObject.name} 파티클 없음");
            enabled = false;
            return;
        }
        _baseScale = transform.localScale;
    }

    public override void SetComponent(Transform spawner, Camera cam = null)
    {
        if (_spawner != null)
        {
            return;
        }
        _spawner = spawner;
    }

    private void OnEnable()
    {
        _hitRadius = _collider.bounds.extents.x;
        _particle[0].Clear();
        _particle[1].Clear();
        _particle[0].Play();
        _particle[1].Play();
        _checkCo = StartCoroutine(Co_CheckTarget());
    }

    protected override void Move()
    {
        transform.position = _spawner.transform.position;
    }

    IEnumerator Co_CheckTarget()
    {
        while (true)
        {
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, _hitRadius, _hits, _hitLayer);

            for (int i = 0; i < count; i++)
            {
                if (_hits[i] != null && _hits[i].TryGetComponent(out BaseProjectile target))
                {

                    if (_hitRecord.Add(target))
                    {
                        target.CutOff();
                    }
                }
            }
            _hitRecord.Clear();

            yield return _nextCheck;
        }
    }

    private void OnDisable()
    {
        if (_checkCo != null)
        {
            _particle[0].Stop();
            _particle[1].Stop();
            StopCoroutine(_checkCo);
            _checkCo = null;
        }
        _hitRecord.Clear();
    }
}
