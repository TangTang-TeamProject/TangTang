using UnityEngine;

public class Trident : SkillAttack
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _player;
    float _top, _bottom, _right, _left;

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

        Vector3 nextPos = transform.position + transform.up * _speed * Time.deltaTime;
        Vector3 currentDir = transform.up;
        bool reflected = false;

        if (nextPos.x < _player.position.x - Mathf.Abs(_left) || nextPos.x > _player.position.x + Mathf.Abs(_right))
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.right);
            reflected = true;
        }

        if (nextPos.y < _player.position.y - Mathf.Abs(_bottom) || nextPos.y > _player.position.y + Mathf.Abs(_top))
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.up);
            reflected = true;
        }

        if (reflected)
        {
            transform.up = currentDir;

            Vector3 pos = transform.position;
            //pos.x = Mathf.Clamp(pos.x, _left, _right);
            //pos.y = Mathf.Clamp(pos.y, _bottom, _top);


            //transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }

    void GetWall()
    {
        float camHeight = _cam.orthographicSize;
        float camWidth = camHeight * _cam.aspect;

        _top = _cam.transform.position.y + camHeight;
        _bottom = _cam.transform.position.y - camHeight;
        _right = _cam.transform.position.x + camWidth;
        _left = _cam.transform.position.x - camWidth;
    }
}
