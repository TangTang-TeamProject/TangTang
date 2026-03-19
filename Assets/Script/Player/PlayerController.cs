using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

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
    }

    void Update()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
        _moveY = Input.GetAxisRaw("Vertical");


        PlayerMove();
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
}
