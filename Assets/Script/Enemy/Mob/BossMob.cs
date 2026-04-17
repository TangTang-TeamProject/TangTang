using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMob : BaseEnemy
{
    [Header("무기 연결")]
    [SerializeField] private GameObject _leftWeaponSlot;
    [SerializeField] private GameObject _rightWeaponSlot;
    [SerializeField] private GameObject _weaponPrefab;

    [Header("도끼 던지기 쿨타임 설정")]
    [SerializeField] private float _throwingPatternCool = 3f;
    [SerializeField] private float _getWeapontime = 5f;

    [Header("HP Bar 연결")]
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private Image _HPBarImage;
    
    private SpriteRenderer _leftAxe;
    private SpriteRenderer _rightAxe;
    private GameObject _throwedWeapon1;
    private GameObject _throwedWeapon2;
    private float _nextPatternTime = 0f;
    private float _nextGetWeapon = 0f;


    private bool _leftHand = true;
    private bool _rightHand = true;

    private Vector3 _targetDir;

    private SpriteRenderer[] _spriteRenderers;
    private List<SpriteRenderer> _activeList = new List<SpriteRenderer>();
    private Dictionary<SpriteRenderer, Color> _colorMap = new Dictionary<SpriteRenderer, Color>();

    private string _animString_Move = "1_Move";
    //private string _animString_Damaged = "3_Damaged";
    private string _animString_Attack = "2_Attack";

    protected override void Awake()
    {
        base.Awake();

        _leftAxe = _leftWeaponSlot.GetComponent<SpriteRenderer>();
        _leftAxe.size = new Vector2(1f, 1f);
        _rightAxe = _rightWeaponSlot.GetComponent<SpriteRenderer>();
        _rightAxe.size = new Vector2(1f, 1f);
        
        _nextPatternTime = Timer.Instance.RealTime + _throwingPatternCool;        
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            if (_spriteRenderers[i].sprite == _leftAxe.sprite || _spriteRenderers[i].sprite == _rightAxe.sprite)
            {
                continue;
            }

            _activeList.Add(_spriteRenderers[i]);
            _colorMap.Add(_spriteRenderers[i], _spriteRenderers[i].color);
        }

        _HPBar.SetActive(true);
        _HPBarImage.fillAmount = 1f;
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
                _speed = _monsterData.MoveSpeed; // 스피드 복구
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

        Chase();
        CheckDamaged();

        
        if (Timer.Instance.RealTime >= _nextPatternTime)
        {            
            if (!_leftHand && !_rightHand)
            {
                return;
            }

            _nextPatternTime = Timer.Instance.RealTime + _throwingPatternCool;

            _targetDir = _target.transform.position - transform.position;
            _targetDir.Normalize();

            if (_leftHand)
            {
                Pattern_ThrowWeapon(true);
            }
            else if(_rightHand)
            {
                Pattern_ThrowWeapon(false);
            }
        }
       
        
    }

    private void Pattern_ThrowWeapon(bool isLeft)
    {
        Vector3 spawnPos = transform.position;
        spawnPos += _targetDir;

        if (isLeft)
        {            
            _leftAxe.sprite = null;
            _nextGetWeapon = Timer.Instance.RealTime + _getWeapontime;
            _throwedWeapon1 = Instantiate(_weaponPrefab, spawnPos, Quaternion.identity);
            _throwedWeapon1.GetComponent<BossWeapon_Axe>().ThrowedToTarget(_target);
            _leftHand = false;
            _animator.SetTrigger(_animString_Attack);
            StartCoroutine(ThrowToGet(true));
        }
        else
        {            
            _rightAxe.sprite = null;
            _nextGetWeapon = Timer.Instance.RealTime + _getWeapontime;
            _throwedWeapon2 = Instantiate(_weaponPrefab, spawnPos, Quaternion.identity);
            _throwedWeapon2.GetComponent<BossWeapon_Axe>().ThrowedToTarget(_target);
            _rightHand = false;
            _animator.SetTrigger(_animString_Attack);
            StartCoroutine(ThrowToGet(false));
        }            
    }

    IEnumerator ThrowToGet(bool isLeft)
    {
        yield return new WaitForSeconds(_getWeapontime);

        GetWeaponToHand(isLeft);
    }

    private void GetWeaponToHand(bool isLeft)
    {
        if (isLeft)
        {
            _leftAxe.sprite = _throwedWeapon1.GetComponent<SpriteRenderer>().sprite;
            _leftAxe.size = new Vector2(1f, 1f);
            _leftHand = true;

            // 도끼 함수 호출
            _throwedWeapon1.GetComponent<BossWeapon_Axe>().IsCatched();            
        }
        else if (!isLeft)
        {
            _rightAxe.sprite = _throwedWeapon2.GetComponent<SpriteRenderer>().sprite;
            _rightAxe.size = new Vector2(1f, 1f);
            _rightHand = true;

            _throwedWeapon2.GetComponent<BossWeapon_Axe>().IsCatched();
        }
    }

    public override void Chase()
    {
        base.Chase();        
        _animator.SetBool(_animString_Move, true);
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
        // 

        // 보스 전리품 생성 호출

        Timer.Instance.IsBossDie(true);
        _HPBar.SetActive(false);
        Destroy(gameObject);
    }  
}
