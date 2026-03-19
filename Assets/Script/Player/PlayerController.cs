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


    void Start()
    {
        _mainCam = Camera.main;
        _target = transform.position;
        _moveSpeed = _playerData.Speed;
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
        Vector2 dir = new Vector2(_moveX, _moveY);

        if (dir.sqrMagnitude > 1f)
        {
            dir.Normalize();
        }

        _target += dir * _moveSpeed * Time.deltaTime;
    }

    void PlayerLook()
    {
        _mouse = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        if(_mouse.x < _target.x)
        {
            //transform.rotation 
        }
    }
}
