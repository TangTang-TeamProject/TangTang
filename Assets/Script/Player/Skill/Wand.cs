using UnityEngine;

public class Wand : SkillAttack
{
    [SerializeField] private GameObject _fire;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private Collider2D _target;
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _offset = new Vector3(-2f, 6f, 0);
    private bool _isExplode;
    private float _elapsed;
    private float _duration = 1;

    private void OnEnable()
    {
        _targetPos = transform.position;
        transform.position += _offset;
        _startPos = transform.position;
        _explosion.SetActive(false);
        _fire.SetActive(true);
        _isExplode = false;
        _target.enabled = false;
        _elapsed = 0;
    }

    protected override void Move()
    {
        if (_isExplode)
        {
            return;
        }

        _elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsed / _duration);
        t = t * t * t;

        transform.position = Vector2.Lerp(_startPos, _targetPos, t);

        if (t >= 1)
        {
            transform.position = _targetPos;
            Explode();
        }

    }

    void Explode()
    {
        _isExplode = true;
        _fire.SetActive(false);
        _explosion.SetActive(true);
        _target.enabled = true;
    }
}
