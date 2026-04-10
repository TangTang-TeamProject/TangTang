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
    [SerializeField] private PlayerData_SO _data;
    [SerializeField] private string _firstWeapon;
        
    private float _maxHp;
    private float _hp;
    private EPlayerState _playerState;
    private WaitForSeconds _nextCheck = new WaitForSeconds(0.2f);
    private WaitForSeconds _invincibleTime;
    private Coroutine _checkCo;
    private Coroutine _invincibleCo;
    private float _speed;
    private float _attack;
    private int _level = 1;
    private float _requireExp = 100;
    private float _currentExp;
    private int _hasSkillNum;
    private int _hasArtifactNum;

    public float MoveSpeed => _speed;
    public float MaxHp => _maxHp;
    public float CurrentHp => _hp;
    public float Exp => _currentExp;
    public float RequireExp => _requireExp;
    public CircleCollider2D PlayerCol => _playerCol;
    public EPlayerState PlayerState => _playerState;
    public int HasSkillNum => _hasSkillNum;
    public int HasArtifactNum => _hasArtifactNum;
    public int Level => _level;
    public event Action OnHit;
    public event Action OnDead;
    public event Action<float> OnCurrentHPChange;
    public event Action<float> OnMaxHPChange;
    public event Action<float> OnCurrentEXPChange;
    public event Action<float> OnRequireEXPChange;

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
        _data = _playerRegistry.GetPlayerByID("");
        if (_data == null)
        {
            CPrint.Error("플레이어 데이터 SO없음");
            return;
        }
        _maxHp = _data.BaseHP;
        _hp = _maxHp;
        _speed = _data.BaseMoveSpeed;
        _attack = _data.BaseATK;
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
    }

    void LevelUp()
    {
        _currentExp = 0;
        _level++;
        ItemManager.instance.PickMeUp();
        OnCurrentEXPChange?.Invoke(_currentExp);
        OnRequireEXPChange?.Invoke(_requireExp);
    }

    public void GetArtifact(string id, int level)
    {
        // 아티팩트 SO에서 id, 레벨확인 파라미터 적용
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
}
