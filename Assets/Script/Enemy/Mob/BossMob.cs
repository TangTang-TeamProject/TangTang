using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMob : BaseEnemy
{
    [Header("무기 연결")]
    [SerializeField] private GameObject _leftWeaponSlot;
    [SerializeField] private GameObject _rightWeaponSlot;
    [SerializeField] private GameObject _weaponPrefab;

    [Header("도끼 던지기 쿨타임 설정")]
    [SerializeField] private float _throwingPatternCool = 3f;
    [SerializeField] private float _getWeapontime = 5f;

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

    protected override void Awake()
    {
        base.Awake();

        _leftAxe = _leftWeaponSlot.GetComponent<SpriteRenderer>();
        _rightAxe = _rightWeaponSlot.GetComponent<SpriteRenderer>();
        
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
        if (_isHit)
        {
            _hitTime -= Time.deltaTime;
            _speed = 0f; // 멈칫하는 모션

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

        if (_target == null) // 타겟 없으면 return
        {
            return;
        }

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

        if (Timer.Instance.RealTime >= _nextGetWeapon)
        {
            GetWeaponToHand();
        }

       
        if (!CanUpdate())
        {
            return;
        }
        
        Chase();
        CheckDamaged();
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
        }
        else
        {            
            _rightAxe.sprite = null;
            _nextGetWeapon = Timer.Instance.RealTime + _getWeapontime;
            _throwedWeapon2 = Instantiate(_weaponPrefab, spawnPos, Quaternion.identity);
            _throwedWeapon2.GetComponent<BossWeapon_Axe>().ThrowedToTarget(_target);
            _rightHand = false;
        }            
    }

    private void GetWeaponToHand()
    {
        if (!_leftHand)
        {
            _leftAxe.sprite = _throwedWeapon1.GetComponent<SpriteRenderer>().sprite;
            _leftAxe.size = new Vector2(1f, 1f);

            // 도끼 함수 호출
            _throwedWeapon1.GetComponent<BossWeapon_Axe>().IsCatched();            
        }
        else if (!_rightHand)
        {
            _rightAxe.sprite = _throwedWeapon2.GetComponent<SpriteRenderer>().sprite;
            _rightAxe.size = new Vector2(1f, 1f);

            _throwedWeapon2.GetComponent<BossWeapon_Axe>().IsCatched();
        }
    }
    
    protected override void Hit(float damage)
    {
        _maxHp -= damage;
        _isHit = true;
        _hitTime = _hitTimer; // 계속 최신 기준 hit 로 변경.
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

        Timer.Instance.IsBossDie(true);
        _pool.Return(this);
    }  
}
