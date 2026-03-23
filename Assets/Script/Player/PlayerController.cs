using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _spawnDir;
    [SerializeField] private Vector3 _spawnArrowPos = new Vector3(0, 0.5f, 0);
    [SerializeField] private float _radius = 0.7f;

    private float _moveSpeed;
    private float _moveX;
    private float _moveY;
    private Camera _mainCam;
    private Vector2 _mouse;
    private Vector2 _baseScale;

    void Awake()
    {
        if (_spawnDir == null)
        {
            CPrint.Warn("플레이어 컨트롤러에 스폰방향 오브젝트 없음");
            return;
        }
        if (_player == null)
        {
            _player = GetComponent<Player>();
            if (_player == null)
            {
                CPrint.Warn("Player가 없음");
                return;
            }
            CPrint.Log("PlayerController에 Player참조 안되어있어서 받아옴");
        }
    }

    void Start()
    {
        _mainCam = Camera.main;
        _baseScale = transform.localScale;
        MoveSpeedSet(_player.MoveSpeed);
    }

    void Update()
    {
        if (_spawnDir == null)
        {
            return;
        }

        _moveX = Input.GetAxisRaw("Horizontal");
        _moveY = Input.GetAxisRaw("Vertical");

        _mouse = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        PlayerMove();
        PlayerLook();
        SpawnLook();
    }

    void PlayerMove()
    {
        Vector3 dir = new Vector3(_moveX, _moveY, 0);

        if (dir.sqrMagnitude > 1f)
        {
            dir.Normalize();
        }

        transform.position += dir * _moveSpeed * Time.deltaTime;
    }

    void PlayerLook()
    {
        Vector3 scale = _baseScale;
        if(_mouse.x < transform.position.x)
        {
            scale.x = _baseScale.x;
        }
        else
        {
            scale.x = -_baseScale.x;
        }
        transform.localScale = scale;
        _spawnDir.transform.localScale = -scale;
    }

    void SpawnLook()
    {
        Vector2 _target = transform.position;
        Vector3 direction = _mouse - _target;
        direction.z = 0;
        direction.Normalize();
        _spawnDir.transform.right = direction;
        _spawnDir.transform.position = transform.position + (direction * _radius) + _spawnArrowPos;
    }

    void MoveSpeedSet(float value)
    {
        _moveSpeed = value;
    }
}
