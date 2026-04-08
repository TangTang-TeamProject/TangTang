using UnityEngine;

public class Trident : SkillAttack
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _player;
    [SerializeField] float _top, _bottom, _right, _left;

    private void OnEnable()
    {
        if (_cam == null)
        {
            return;
        }
        GetWall();
    }

    public override void SetTrident(Camera cam, Transform player)
    {
        if (_cam != null &&  _player != null)
        {
            return;
        }
        _cam = cam;
        _player = player;
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

            Vector3 pos = transform.position;
        }
    }

    void GetWall()
    {
        float camHeight = _cam.orthographicSize;
        float camWidth = camHeight * _cam.aspect;

        _top = _player.position.y + camHeight;
        _bottom = _player.position.y - camHeight;
        _right = _player.position.x + camWidth;
        _left = _player.position.x - camWidth;
    }
}
