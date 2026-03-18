using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed;

    private float _moveX;
    private float _moveY;
    private float _angle;
    private Camera _mainCam;
    private Vector2 _target;
    private Vector2 _mouse;

    private void Awake()
    {
        if (_rb == null)
        {
            Debug.LogWarning("Player에 리지드바디 없음");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        _mainCam = Camera.main;
        _target = transform.position;
    }

    void Update()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
        _moveY = Input.GetAxisRaw("Vertical");

        _mouse = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        _angle = Mathf.Atan2(_mouse.y - _target.y, _mouse.x - _target.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(_angle - 90, Vector3.forward);
    }

    private void LateUpdate()
    {
        PlayerMove();
    }

    void PlayerMove()
    {        
        Vector2 dir = new Vector2(_moveX, _moveY).normalized;
        Vector2 v = dir * _moveSpeed;
        _rb.velocity = v;
    }
}
