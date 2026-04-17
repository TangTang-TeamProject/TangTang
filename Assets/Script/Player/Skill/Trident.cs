using UnityEngine;

public class Trident : SkillAttack
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _spawner;
    [SerializeField] float _top, _bottom, _right, _left;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }
    private void OnEnable()
    {
        if (_cam == null)
        {
            return;
        }
        GetWall();
    }

    public override void SetComponent(Transform spawner, Camera cam)
    {
        if (_cam != null &&  _spawner != null)
        {
            return;
        }
        _cam = cam;
        _spawner = spawner;
    }

    protected override void Move()
    {
        ScreenBounce();
        transform.position += transform.up * _speed * Time.deltaTime;
    }

    private void ScreenBounce()
    {
        if (_cam == null)
            return;
        GetWall();
        Vector3 nextPos = transform.position + transform.up * _speed * Time.deltaTime;
        Vector3 currentDir = transform.up;
        bool reflected = false;

        if (nextPos.x < _left || nextPos.x > _right)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.right);
            reflected = true;
        }

        if (nextPos.y < _bottom || nextPos.y > _top)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.up);
            reflected = true;
        }

        if (reflected)
        {
            transform.up = currentDir;
            SoundManager.Instance.PlaySfx(ESfxType.Trident);
            Vector3 pos = transform.position;
        }
    }

    void GetWall()
    {
        float camHeight = _cam.orthographicSize;
        float camWidth = camHeight * _cam.aspect;

        _top = _spawner.position.y + camHeight;
        _bottom = _spawner.position.y - camHeight;
        _right = _spawner.position.x + camWidth;
        _left = _spawner.position.x - camWidth;
    }
}
