using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class STG2_MidBoss2 : BaseEnemy
{
    [Header("HP Bar 연결")]
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private Image _HPBarImage;

    [Header("랜덤 박스")]
    [SerializeField] private GameObject _itemParent;
    [SerializeField] private GameObject _randomBox;

    [Header("투사체 연결")]
    [SerializeField] private GameObject _projectile;

    private Vector2 _shootDir;
    private Vector2 _shootOrigin;
    private float _nextShoot;    

    private SpriteRenderer[] _spriteRenderers;
    private List<SpriteRenderer> _activeList = new List<SpriteRenderer>();
    private Dictionary<SpriteRenderer, Color> _colorMap = new Dictionary<SpriteRenderer, Color>();

    private string _animString_Move = "1_Move";
    private string _animString_Attack = "2_Attack";

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

        _nextShoot = Timer.Instance.RealTime + _atkCycle;

        if (TryGetComponent(out CircleCollider2D circleCollider2D))
        {
            _col = GetComponent<CircleCollider2D>();
        }
        else
        {
            CPrint.Log($"{this} -> CircleCollider2D 없음");
        }
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
                for (int i = 0; i < _activeList.Count; i++)
                {
                    _activeList[i].color = _colorMap[_activeList[i]];
                }
            }
        }

        if (!CanUpdate())
        {
            return;
        }

        if (Timer.Instance.RealTime >= _nextShoot)
        {
            StartCoroutine(StopToShoot());
            StartCoroutine(ShootCoroutine());
            _nextShoot = Timer.Instance.RealTime + _atkCycle;
        }


        Chase();
        CheckDamaged();
    }

    void LateUpdate()
    {
        MoveIntoBattlezone();
    }


    protected override void Hit(IAttackables attackables)
    {
        _maxHp -= attackables.Damage;
        _isHit = true;
        _hitTime = _hitTimer; // 계속 최신 기준 hit 로 변경.        
        //_animator.SetBool(_animString_Move, false);
        //_animator.SetTrigger(_animString_Damaged);
        SoundManager.Instance.PlaySfx(ESfxType.EnemyHit);

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

    private void ShootProjectile()
    {
        _shootOrigin = transform.position;
        _shootOrigin.y += 0.5f;
        
        _shootDir = (Vector2)_target.transform.position - _shootOrigin;
        _shootDir.Normalize();

        _shootOrigin += _shootDir;

        GameObject go = Instantiate(_projectile, _shootOrigin, Quaternion.identity);
        Projectile_MidBoss2 projectile = go.GetComponent<Projectile_MidBoss2>();
        projectile.Init(null, _target.transform);
        projectile.SetShootDir(_shootDir);
    }

    IEnumerator ShootCoroutine()
    {        
        for (int i = 0; i < 3; i++)
        {
            ShootProjectile();
            yield return new WaitForSeconds(0.2f);
        }        
    }

    IEnumerator StopToShoot()
    {
        _speed = 0;
        _animator.SetTrigger(_animString_Attack);

        yield return new WaitForSeconds(1.2f);

        _speed = _monsterData.MoveSpeed;        
    }

    public override void Chase()
    {
        base.Chase();
        _animator.SetBool(_animString_Move, true);
    }
}
