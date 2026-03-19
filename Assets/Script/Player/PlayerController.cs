using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData_SO _playerData;

    private float _moveSpeed;
    private float _moveX;
    private float _moveY;
    private float _angle;
    private Camera _mainCam;
    private Vector2 _target;
    private Vector2 _mouse;
    private Vector2 _baseScale;


    void Start()
    {
        _mainCam = Camera.main;
        _target = transform.position;
        _moveSpeed = _playerData.Speed;
        _baseScale = transform.localScale;
    }

    void Update()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
        _moveY = Input.GetAxisRaw("Vertical");


        PlayerMove();
        PlayerLook();
        // 나중에 공격 방향 정하는 오브젝트에서 사용
        //_mouse = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        //_angle = Mathf.Atan2(_mouse.y - _target.y, _mouse.x - _target.x) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.AngleAxis(_angle - 90, Vector3.forward);
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
        _mouse = _mainCam.ScreenToWorldPoint(Input.mousePosition);
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
    }
}
