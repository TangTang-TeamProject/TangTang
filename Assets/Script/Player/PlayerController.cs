using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData_SO _playerData;
    [SerializeField] private GameObject _spawnDir;

    private float _moveSpeed;
    private float _moveX;
    private float _moveY;
    private float _angle;
    private Camera _mainCam;
    private Vector2 _mouse;
    private Vector2 _baseScale;
    private Vector2 _target;

    void Awake()
    {
        if (_spawnDir == null)
        {
            CPrint.Warn("플레이어 컨트롤러에 스폰방향 오브젝트 없음");
            return;
        }

        if (_playerData == null)
        {
            CPrint.Warn("플레이어 컨트롤러에 플레이어데이터 SO 없음");
            return;
        }
    }

    void Start()
    {
        _mainCam = Camera.main;
        _moveSpeed = _playerData.Speed;
        _baseScale = transform.localScale;
        _target = _spawnDir.transform.position;
    }

    void Update()
    {
        if (_playerData == null)
            return;

        _moveX = Input.GetAxisRaw("Horizontal");
        _moveY = Input.GetAxisRaw("Vertical");


        _mouse = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        PlayerMove();
        PlayerLook();

        if (_spawnDir == null)
            return;

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
        Vector3 direction = _mouse - _target;
        direction.z = 0;
        direction.Normalize();
        _spawnDir.transform.up = -direction;
    }
}
