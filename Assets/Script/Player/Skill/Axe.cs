using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : SkillAttack
{
    [SerializeField] private Transform _center;
    [SerializeField] private float _radius = 2.0f;
    [SerializeField] private CircleCollider2D _collider;
    [SerializeField] private LayerMask _hitLayer;
    [SerializeField] private string _hitLayerString = "EnemyBullet";

    private float _timer;
    private float _offset;
    private float _hitRadius;
    private readonly Collider2D[] _hits = new Collider2D[20];
    private HashSet<BaseProjectile> _hitRecord = new HashSet<BaseProjectile>(20);
    private WaitForSeconds _nextCheck = new WaitForSeconds(0.1f);
    private Coroutine _checkCo;

    private void Awake()
    {
        _hitLayer = LayerMask.GetMask(_hitLayerString);
        _baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        _isSpin = true;
        _spinZ = 480f;
        _timer = 0;
        _hitRadius = _collider.radius;
        _checkCo = StartCoroutine(Co_CheckTarget());
    }
    public override void SetComponent(Transform center, Camera cam = null)
    {
        if (_center != null)
        {
            return;
        }
        _center = center;
    }

    public override void SetOrbit(float dist)
    {
        _offset = dist * Mathf.Deg2Rad;

        float targetPos = 0 + _offset;

        float x = Mathf.Cos(targetPos) * _radius;
        float y = Mathf.Sin(targetPos) * _radius;

        transform.position = _center.position + new Vector3(x, y, 0);
    }

    protected override void Move()
    {
        _timer += Time.deltaTime * _speed;

        float targetPos = _timer + _offset;

        float x = Mathf.Cos(targetPos) * _radius;
        float y = Mathf.Sin(targetPos) * _radius;

        transform.position = _center.position + new Vector3(x, y, 0);
        if (_id == "AxeEvo")
        {
            _remainTime = _keepTime;
        }
        SoundManager.Instance.PlaySfx(ESfxType.Axe);
    }

    protected override void Rotate()
    {
        transform.Rotate(Vector3.forward * _spinZ * Time.deltaTime);
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
            StopCoroutine(_checkCo);
            _checkCo = null;
        }
        _hitRecord.Clear();
    }
}
