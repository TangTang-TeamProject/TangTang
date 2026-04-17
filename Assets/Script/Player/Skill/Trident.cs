using UnityEngine;

public class Trident : SkillAttack
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _spawner;
    [SerializeField] private float _pushDistance = 0.5f;
    float _top, _bottom, _right, _left;

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

        if (nextPos.x < _left)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.right);
            reflected = true;
        }
        else if (nextPos.x > _right)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.left);
            reflected = true;
        }

        if (nextPos.y < _bottom)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.up);
            reflected = true;
        }
        else if (nextPos.y > _top)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.down);
            reflected = true;
        }

        if (reflected)
        {
            transform.up = currentDir;
            SoundManager.Instance.PlaySfx(ESfxType.Trident);
            Vector3 pos = transform.position;

            pos.x = Mathf.Clamp(pos.x, _left, _right);
            pos.y = Mathf.Clamp(pos.y, _bottom, _top);

            pos += currentDir * _pushDistance;

            transform.position = pos;
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
