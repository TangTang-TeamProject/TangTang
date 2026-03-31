using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualBlade : SkillAttack
{
    [SerializeField] private Transform _spawner;
    private void OnEnable()
    {
        // 임시 실험용 나중에 구조 고치면서 스포너에서 넘겨줄거임
        GameObject spawner = GameObject.Find("SkillSpawner");
        _spawner = spawner.transform;
        //
        _isSpin = true;
    }
    protected override void Move()
    {
        transform.position = _spawner.transform.position;
    }

    protected override void Rotate()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }
}
