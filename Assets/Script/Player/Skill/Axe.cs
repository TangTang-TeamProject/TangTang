using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : SkillAttack
{
    [SerializeField] private Transform _spawner;
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
    }

    private void OnEnable()
    {
        // 임시 실험용 나중에 구조 고치면서 스포너에서 넘겨줄거임
        GameObject spawner = GameObject.Find("SkillSpawner");
        _spawner = spawner.transform;
        //
        _isSpin = true;
        _spinZ = 360f;
        _timer = 0;
        _hitRadius = _collider.radius;
        _checkCo = StartCoroutine(Co_CheckTarget());
    }

    public override void SetOrbit(float dist)
    {
        _offset = dist * Mathf.Deg2Rad;
    }

    protected override void Move()
    {
        _timer += Time.deltaTime * _speed;

        float targetPos = _timer + _offset;

        float x = Mathf.Cos(targetPos) * _radius;
        float y = Mathf.Sin(targetPos) * _radius;

        transform.position = _spawner.position + new Vector3(x, y, 0);
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
