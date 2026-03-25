using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : SkillAttack
{
    protected override void Move()
    {
        transform.position += Vector3.up * _speed * Time.deltaTime;
    }
}
