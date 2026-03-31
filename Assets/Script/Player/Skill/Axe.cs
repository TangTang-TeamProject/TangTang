using UnityEngine;

public class Axe : SkillAttack
{
    [SerializeField] private Transform _spawner;
    [SerializeField] private float _radius = 2.0f;
    private float _timer;
    private float _offset;
    private void OnEnable()
    {
        // 임시 실험용 나중에 구조 고치면서 스포너에서 넘겨줄거임
        GameObject spawner = GameObject.Find("SkillSpawner");
        _spawner = spawner.transform;
        //
        _isSpin = true;
        _spinZ = 360f;
        _timer = 0;
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
}
