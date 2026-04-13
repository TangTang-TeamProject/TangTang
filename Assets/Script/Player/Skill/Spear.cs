using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : SkillAttack
{
    private void Awake()
    {
        _baseScale = transform.localScale;
    }
    protected override void Move()
    {
        transform.position += transform.up * _speed * Time.deltaTime;
    }
}
