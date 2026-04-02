using System.Collections;
using UnityEngine;

public class DashMob : BaseEnemy
{
    [SerializeField] private float _dashPower = 5f;
    [Header("해당 수치이하 만큼 접근하면 대시 발동")]
    [SerializeField] private float _dashDist = 5f;
    [SerializeField] private float _dashTime = 1f;

    private bool _isDashing = false;
    private string animParam = "Dash";
    
    private Vector2 _dashDir;
    private float _checkTime;

    
    private float _corTimeCnt = 0f;    

    protected override void Update()
    {
        base.Update();

        if (_target == null) // 타겟 없으면 return
        {
            return;
        }

        _corTimeCnt += Time.deltaTime;

        if (Timer.Instance.RealTime >= _checkTime && !_isDashing)
        {
            Attack();
            _checkTime = Timer.Instance.RealTime + _atkCycle;
        }

        if (!CanUpdate())
        {
            return;
        }

        Chase();
        CheckDamaged();

    }

    private void FixedUpdate()
    {        
        
    }
    public override void Attack()
    {        
        _dashDir = (_target.transform.position - transform.position);

        if (_dashDir.magnitude > _dashDist)
        {
            return;
        }

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

    public override void Die()
    {
        base.Die();
    }
}
