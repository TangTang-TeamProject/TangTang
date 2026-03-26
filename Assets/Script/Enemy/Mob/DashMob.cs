using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMob : BaseEnemy
{
    [SerializeField] private float _dashPower = 5f;    
    [SerializeField] private float _startDelay = 3f;
    [SerializeField] private float _dashTime = 1f;

    private bool _isDashing = false;
    private string animParam = "Dash";
    
    private Vector2 _dashDir;
    private float _checkTime;

    
    private float _corTimeCnt = 0f;

    private void Start()
    {
        _checkTime = _startDelay; // 생성 후 Attack 까지 delay.
    }

    private void Update()
    {
        _corTimeCnt += Time.deltaTime;

        if (Timer.Instance.RealTime >= _checkTime && !_isDashing)
        {
            Attack();
            _checkTime = Timer.Instance.RealTime + _atkCycle;
        }

        CheckDamaged();
    }

    private void FixedUpdate()
    {        
        Chase();
    }
    public override void Attack()
    {        
        _dashDir = (_target.transform.position - transform.position).normalized;

        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        _isDashing = true;
        _animator.SetBool(animParam, true);
        _corTimeCnt = Timer.Instance.RealTime;
        float endTime = _corTimeCnt + _dashTime;

        yield return new WaitForSeconds(0.5f);

        while (_corTimeCnt < endTime)
        {
            Vector2 newPos = transform.position;

            newPos += _dashDir * _speed * _dashPower * Time.deltaTime;

            transform.position = newPos;

            yield return null;            
        }

        _animator.SetBool(animParam, false);
        _isDashing = false;
    }
}
