using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : SkillAttack
{
    //발사 방향에따라 rightSpeed 음수 양수 변경
    private float _upSpeed;
    private void OnEnable()
    {
        _isSpin = true;
        _spinZ = 120f;
        _upSpeed = 8f;
    }

    protected override void Move()
    {
        transform.position += Vector3.up * _upSpeed * Time.deltaTime;
        transform.position += Vector3.right * _speed * Time.deltaTime;

        _upSpeed -= 9.8f * Time.deltaTime;
    }

    protected override void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, _spinZ) * Time.deltaTime, Space.World);
    }
}
