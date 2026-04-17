using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class STG2_MidBoss1 : BaseEnemy
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
    private string animParam_Move = "1_Move";
    private string animParam = "2_Attack";

    private Vector2 _dashDir;
    private float _checkTime;

    private float _corTimeCnt = 0f;

    private SpriteRenderer[] _spriteRenderers;
    private List<SpriteRenderer> _activeList = new List<SpriteRenderer>();
    private Dictionary<SpriteRenderer, Color> _colorMap = new Dictionary<SpriteRenderer, Color>();

    protected override void Awake()
    {
        base.Awake();

        _HPBar.SetActive(true);
        _HPBarImage.fillAmount = 1f;


        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _activeList.Add(_spriteRenderers[i]);
            _colorMap.Add(_spriteRenderers[i], _spriteRenderers[i].color);
        }

        _animator = GetComponentInChildren<Animator>();
    }

    public override void Init(EnemyPool pool, int idx)
    {
        if (pool != null)
        {
            _pool = pool;
        }
        _idx = idx;

        _id = _monsterData.EnemyID;
        _maxHp = _monsterData.HP;
        _contactDamage = _monsterData.ContactDamage;
        _speed = _monsterData.MoveSpeed;
        _atkCycle = _monsterData.ATKCycle;
        _bulletSpeed = _monsterData.BulletSpeed;
        _expDrop = _monsterData.ExpDrop;
        _mobType = _monsterData.EnemyType;

        _checkTime = Timer.Instance.RealTime + _atkCycle;
    }

    protected override void Update()
    {       
        if (_target == null) // 타겟 없으면 return
        {
            return;
        }
         
        if (_isHit)
        {
            _hitTime -= Time.deltaTime;   

            if (_hitTime <= 0f)
            {
                _isHit = false;
                _hitTime = _hitTimer;
                _speed = _monsterData.MoveSpeed; // 스피드 복구
                
                for (int i = 0; i < _activeList.Count; i++)
                {
                    _activeList[i].color = _colorMap[_activeList[i]];
                }
            }
        }

        if (Timer.Instance.RealTime >= _checkTime && !_isDashing)
        {
            Attack();
            _checkTime = Timer.Instance.RealTime + _atkCycle;
        }

        

        if (!CanUpdate())
        {
            return;
        }

        CheckDamaged();

        if (_isDashing)
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

    public override void Chase()
    {
        base.Chase();
        _animator.SetBool(animParam_Move, true);
    }

    IEnumerator Dash()
    {
        _isDashing = true;
        
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

        _animator.SetTrigger(animParam);
        _isDashing = false;
    }

   protected override void Hit(IAttackables attackables)
    {
        _maxHp -= attackables.Damage;
        _isHit = true;
        _hitTime = _hitTimer; // 계속 최신 기준 hit 로 변경.
        SoundManager.Instance.PlaySfx(ESfxType.EnemyHit);                              
        //_animator.SetBool(_animString_Move, false);
        //_animator.SetTrigger(_animString_Damaged);

        float ratio = _maxHp / _monsterData.HP;
        _HPBarImage.fillAmount = ratio;

        for (int i = 0; i < _activeList.Count; i++)
        {
            _activeList[i].color = Color.red;
        }

        if (_maxHp <= 0)
        {
            Die();
        }
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
