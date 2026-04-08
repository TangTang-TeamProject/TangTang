using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : SkillAttack
{
    protected override void Move()
    {
        transform.position += transform.up * _speed * Time.deltaTime;
    }
}
