using UnityEngine;

public class Trident : SkillAttack
{
    [SerializeField]private Camera _cam;
    private void Awake()
    {
        _cam = Camera.main;
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

        Vector3 pos = _cam.WorldToViewportPoint(transform.position);
        Vector3 currentDir = transform.up;
        bool reflected = false;

        float border = 0.02f;

        if (pos.x < border)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.right);
            pos.x = border + 0.001f;
            reflected = true;
        }
        else if (pos.x > 1f - border)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.right);
            pos.x = 1f - border - 0.001f;
            reflected = true;
        }

        // 상하 벽 체크
        if (pos.y < border)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.up);
            pos.y = border + 0.001f;
            reflected = true;
        }
        else if (pos.y > 1f - border)
        {
            currentDir = Vector2.Reflect(currentDir, Vector2.up);
            pos.y = 1f - border - 0.001f;
            reflected = true;
        }

        if (reflected)
        {
            transform.up = currentDir;

            float zDist = Mathf.Abs(_cam.transform.position.z - transform.position.z);
            transform.position = _cam.ViewportToWorldPoint(new Vector3(pos.x, pos.y, zDist));
        }
    }
}
