using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamagables
{
    public enum EPlayerState
    {
        Normal,
        Invincible,
        Dead,
    }
    [SerializeField] private CircleCollider2D _playerCol;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private string _targetLayerMask1 = "Enemy";
    [SerializeField] private string _targetLayerMask2 = "EnemyBullet";
    [SerializeField] private float _invincibleDuration = 0.5f;
    [SerializeField] private PlayerRegistry _playerRegistry;
    [SerializeField] private EquipRegistry _equipRegistry;
    [SerializeField] private ArtifactRegistry _artifactRegistry;
    [SerializeField] private string _firstWeapon;
        
    private PlayerData_SO _data;
    private float _maxHp;
    private float _hp;
    private EPlayerState _playerState;
    private WaitForSeconds _nextCheck = new WaitForSeconds(0.2f);
    private WaitForSeconds _invincibleTime;
    private WaitForSeconds _nextHeal = new WaitForSeconds(5.0f);
    private Coroutine _checkCo;
    private Coroutine _invincibleCo;
    private Coroutine _autoHealCo;
    private float _speed;
    private float _baseAttack;
    private float _attack;
    private int _level = 1;
    private float _requireExp = 10;
    private float _currentExp;
    private float _absorbePer = 1;
    private float _cool = 1;
    private float _duration = 1;
    private float _range = 1;
    private float _autoHealAmount;

    public float MoveSpeed => _speed;
    public float MaxHp => _maxHp;
    public float CurrentHp => _hp;
    public float Attack => _attack;
    public float Cool => _cool;
    public float Duration => _duration;
    public float Range => _range;
    public float Exp => _currentExp;
    public float RequireExp => _requireExp;
    public CircleCollider2D PlayerCol => _playerCol;
    public EPlayerState PlayerState => _playerState;
    public int Level => _level;
    public event Action OnHit;
    public event Action OnDead;
    public event Action<float> OnCurrentHPChange;
    public event Action<float> OnMaxHPChange;
    public event Action<float> OnCurrentEXPChange;
    public event Action<float> OnRequireEXPChange;
    public event Action<float> OnLootRangeChange;
    public event Action<float> OnAttackChange;
    public event Action<float> OnRangeChange;

    private void Reset()
    {
        PlayerController pc = GetComponent<PlayerController>();
        PlayerItemLoot il = GetComponent<PlayerItemLoot>();

        if (pc == null)
        {
            pc = gameObject.AddComponent<PlayerController>();
        }

        if (il == null)
        {
            il = gameObject.AddComponent<PlayerItemLoot>();
        }
    }

    private void OnValidate()
    {
        // 레이어 강제지정
        int mask1 = 1 << LayerMask.NameToLayer(_targetLayerMask1);
        int mask2 = 1 << LayerMask.NameToLayer(_targetLayerMask2);
        int setMask = mask1 | mask2;
        if ((_targetLayer.value & setMask) != setMask)
        {
            _targetLayer.value |= setMask;
        }

        if (_playerCol == null)
        {
            _playerCol = GetComponent<CircleCollider2D>();
        }
    }

    private void OnEnable()
    {
        _data = _playerRegistry.GetPlayerByID(SaveManager.data.selectedChar);
        if (_data == null)
        {
            CPrint.Error("플레이어 데이터 SO없음");
            return;
        }
        _maxHp = _data.BaseHP;
        _speed = _data.BaseMoveSpeed;
        _baseAttack = _data.BaseATK;
        _firstWeapon = _data.Weapon;
        SetEquipParam();
        _hp = _maxHp;
        _attack = _baseAttack;
    }

    void SetEquipParam()
    {
        float atk = 0, hp = 0, speed = 0;
        /*
        string[] ids = SaveManager.data.equipID;
        for (int i = 0; i < ids.Length; i++)
        {
            EquipData_SO _equipData = _equipRegistry.GetEquipByID(ids[i]);
            atk += _equipData.ATK;
            hp += _equipData.HPChange;
            speed += _equipData.SpeedChange;
        }
        */
        _maxHp *= 1 + (hp * 0.01f);
        _baseAttack *= 1 + (atk * 0.01f);
        _speed *= 1 + (speed * 0.01f);
    }

    void Start()
    {
        _checkCo = StartCoroutine(Co_CheckHit());
        _invincibleTime = new WaitForSeconds(_invincibleDuration);
        ItemManager.instance.EXP += GainExp;
        ItemManager.instance.Heal += GetHeal;
    }

    private void OnDisable()
    {
        ItemManager.instance.EXP -= GainExp;
        ItemManager.instance.Heal -= GetHeal;
        StopAllPlayerCoroutine();
    }

    public string FirstSkill()
    {
        return _firstWeapon;
    }

    IEnumerator Co_CheckHit()
    {
        while (true)
        {
            // 0.2초마다 판정이라 무적시간이 조금 더 길어질 수는 있음
            if (_playerState == EPlayerState.Invincible || _playerState == EPlayerState.Dead)
            {
                yield return _nextCheck;
                continue;
            }

            Vector2 center = (Vector2)transform.position + _playerCol.offset;
            Collider2D hit = Physics2D.OverlapCircle(center, _playerCol.radius, _targetLayer);

            if (hit != null && hit.TryGetComponent(out IAttackables enemy))
            {
                Hit(enemy.Damage);
                enemy.GetDestroy();
            }

            yield return _nextCheck;
        }
    }

    IEnumerator Co_Invincible()
    {
        yield return _invincibleTime;
        _playerState = EPlayerState.Normal;
        _invincibleCo = null;
        CPrint.Log("무적 해제");
    }

    public void Hit(float damage)
    {
        if(_playerState == EPlayerState.Invincible ||  _playerState == EPlayerState.Dead)
        {
            return;
        }
        CPrint.Log("플레이어 맞았음, 무적 시작");
        _hp -= damage;
        OnHit?.Invoke();
        OnCurrentHPChange?.Invoke(_hp);

        if (_hp <= 0)
        {
            _hp = 0;
            Die();
            return;
        }

        _playerState = EPlayerState.Invincible;
        _invincibleCo = StartCoroutine(Co_Invincible());
    }

    public void Die()
    {
        // 예외 처리안함 어차피 Hit하지 않는이상 호출될 일이 없음
        CPrint.Warn("플레이어 죽었음");
        _playerState = EPlayerState.Dead;
        OnDead?.Invoke();
        StopAllPlayerCoroutine();
    }

    void StopAllPlayerCoroutine()
    {
        if (_checkCo != null)
        {
            StopCoroutine(_checkCo);
            _checkCo = null;
        }

        if (_invincibleCo != null)
        {
            StopCoroutine(_invincibleCo);
            _invincibleCo = null;
        }

        if (_autoHealCo != null)
        {
            StopCoroutine(_autoHealCo);
            _autoHealCo = null;
        }
    }

    void LevelUp()
    {
        _currentExp = 0;
        _requireExp = _requireExp + 10;
        _level++;
        ItemManager.instance.PickMeUp();
        OnCurrentEXPChange?.Invoke(_currentExp);
        OnRequireEXPChange?.Invoke(_requireExp);
    }

    public void GetArtifact(string id, int level)
    {
        ArtifactData_SO data = _artifactRegistry.GetArtifactByID(id);
        switch (data.StatKey)
        {
            // 공격력 증가
            case StatKey.Damage:
                _attack = _baseAttack * (1 + (data.ValuePerLevel * 0.01f) * level);
                OnAttackChange?.Invoke(_attack);
                break;
            // 체력 증가
            case StatKey.HP:
                _maxHp += data.ValuePerLevel;
                OnMaxHPChange?.Invoke(_maxHp);
                _hp += data.ValuePerLevel;
                OnCurrentHPChange?.Invoke(_hp);
                break;
            // 발사 쿨
            case StatKey.CoolDown:
                _cool -= data.ValuePerLevel * 0.01f;
                break;
            // 지속 시간
            case StatKey.Duration:
                _duration += data.ValuePerLevel * 0.01f;
                break;
            // 범위
            case StatKey.Range:
                _range += data.ValuePerLevel * 0.01f;
                OnRangeChange?.Invoke(_range);
                break;
            // 흡수 범위
            case StatKey.AbsorbeRange:
                _absorbePer += data.ValuePerLevel * 0.01f;
                OnLootRangeChange?.Invoke(_absorbePer);
                break;
            // 자동회복
            case StatKey.AutoHeal:
                _autoHealAmount += data.ValuePerLevel;
                if(_autoHealCo == null)
                {
                    _autoHealCo = StartCoroutine(Co_AutoHeal());
                }                
                break;
        }
    }

    void GainExp(float exp)
    {
        _currentExp += exp;
        OnCurrentEXPChange?.Invoke(_currentExp);
        if (_currentExp >= _requireExp)
        {
            LevelUp();
        }
    }

    void GetHeal()
    {
        _hp += _maxHp / 2;
        if (_hp > _maxHp)
        {
            _hp = _maxHp;
        }
        OnCurrentHPChange?.Invoke(_hp);
    }

    IEnumerator Co_AutoHeal()
    {
        while (true)
        {
            _hp += _autoHealAmount;
            OnCurrentHPChange?.Invoke(_hp);
            yield return _nextHeal;
        }
    }
}