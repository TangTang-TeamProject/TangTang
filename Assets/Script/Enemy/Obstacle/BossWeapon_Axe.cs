using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon_Axe : MonoBehaviour
{
    [Header("이동 및 회전 속도")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotSpeed = 90f;

    private bool _isThrowed = false;
    private GameObject _target;

    private float _minX = -15f;
    private float _maxX = 15f;
    private float _minY = -15f;
    private float _maxY = 15f;

    void Start()
    {
        
    }

    void Update()
    {
        if (!_isThrowed)
        {
            return;
        }


    }

    public void ThrowedToTarget(GameObject target)
    {
        _isThrowed = true;
        _target = target;
    }

    private void MovingToTarget()
    {
        Quaternion euler = Quaternion.Euler(0f, 0f, _rotSpeed);
        transform.Rotate(new Vector3(0f, 0f, _rotSpeed));

        Vector2 newPos = transform.position;

        Vector2 newDir = _target.transform.position - transform.position;
        newDir.Normalize();

        

    }
}
