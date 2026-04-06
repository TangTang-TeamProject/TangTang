using UnityEngine;

public class Trident : SkillAttack
{
    [SerializeField]private Camera _cam;
    private Vector3 _lastCamPos;
    float _top, _bottom, _right, _left;
    private void Awake()
    {
        _cam = Camera.main;
    }

    private void OnEnable()
    {
        _lastCamPos = _cam.transform.position;
    }

    protected override void Move()
    {
        ApplyCameraDelta();
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
            pos.x = Mathf.Clamp(pos.x, _left, _right);
            pos.y = Mathf.Clamp(pos.y, _bottom, _top);


            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
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

    private void ApplyCameraDelta()
    {
        if (_cam == null) return;

        Vector3 camDelta = _cam.transform.position - _lastCamPos;
        camDelta.z = 0;

        //카메라 이동만큼 같이 이동
        transform.position += camDelta;

        _lastCamPos = _cam.transform.position;
    }
}
