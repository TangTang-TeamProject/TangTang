

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MidBoss1 : BaseEnemy
{
    [SerializeField] private float _dashPower = 3f;
    [Header("해당 수치이하 만큼 접근하면 대시 발동")]
    [SerializeField] private float _dashDist = 5f;
    [SerializeField] private float _dashTime = 1f;

    [Header("HP Bar 연결")]
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private Image _HPBarImage;

    [Header("랜덤 박스")]
    [SerializeField] private GameObject _itemParent;
    [SerializeField] private GameObject _randomBox;   

    private bool _isDashing = false;
    private string animParam = "Dash";

    private Vector2 _dashDir;
    private float _checkTime;

    private float _corTimeCnt = 0f;

    protected override void Awake()
    {
        base.Awake();
        _HPBar.SetActive(true);
        _HPBarImage.fillAmount = 1f;
    }

    protected override void Update()
    {
        base.Update();

        if (_target == null) // 타겟 없으면 return
        {
            return;
        }        

        if (Timer.Instance.RealTime >= _checkTime && !_isDashing)
        {
            Attack();
            _checkTime = Timer.Instance.RealTime + _atkCycle;
        }
       
        CheckDamaged();

        if (!CanUpdate() || _isDashing)
        {
            return;
        }

        Chase();
        
    }

    private void LateUpdate()
    {
        MoveIntoBattlezone();
    }

    public override void Attack()
    {
        _dashDir = (_target.transform.position - transform.position);

        if (_dashDir.magnitude > _dashDist)
        {
            return;
        }

        _dashDir.Normalize();

        StartCoroutine(Dash());
    }        

    

    IEnumerator Dash()
    {
        _isDashing = true;
        _animator.SetBool(animParam, true);
        _corTimeCnt = Timer.Instance.RealTime;
        float endTime = _corTimeCnt + _dashTime;

        float t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            MoveIntoBattlezone();
            yield return null;
        }

        while (_corTimeCnt < endTime)
        {
            _corTimeCnt += Time.deltaTime;

            Vector2 newPos = transform.position;

            newPos += _dashDir * _speed * _dashPower * Time.deltaTime;

            transform.position = newPos;
            MoveIntoBattlezone();
            yield return null;
        }

        _animator.SetBool(animParam, false);
        _isDashing = false;
    }

    protected override void Hit(float damage)
    {
        base.Hit(damage);

        float ratio = _maxHp / _monsterData.HP;
        _HPBarImage.fillAmount = ratio;
    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출

        Timer.Instance.IsBossDie(false);
        _HPBar.SetActive(false);
        Instantiate(_randomBox, transform.position, Quaternion.identity, _itemParent.transform);
        Destroy(gameObject);
    }
}
