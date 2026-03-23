using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamagables
{
    [Flags]
    public enum EPlayerSkill
    {
        Arrow = 0 << 1,
        Spear = 1 << 1
    }
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
    // 나중에 SO로 하든 컨트롤할 값이 들어오면 변경
    [SerializeField] private float _hp = 20;

    public EPlayerState _playerState { get; private set; }
    private WaitForSeconds _nextCheck = new WaitForSeconds(0.2f);
    private WaitForSeconds _invincibleTime;
    private Coroutine _checkCo;
    private Coroutine _invincibleCo;
    private float _speed;
    private float _attack;
    private int _level;
    private int _requireExp;
    private int _currentExp;

    public float MoveSpeed => _speed;
    public CircleCollider2D PlayerCol => _playerCol;
    public EPlayerState PlayerState => _playerState;

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
        _data = _playerRegistry.GetEnemyByID(1);
        if (_data == null)
        {
            CPrint.Error("플레이어 데이터 SO없음");
            return;
        }
        _hp = _data.HP;
        _speed = _data.Speed;
        _attack = _data.ATK;
    }

    void Start()
    {
        _checkCo = StartCoroutine(Co_CheckHit());
        _invincibleTime = new WaitForSeconds(_invincibleDuration);        
    }

    private void OnDisable()
    {
        StopAllPlayerCoroutine();
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
    }
    /* 스킬 스크립트 리스트 활성화할거 -> 스킬 데이터, 사용여부 -> 스킬 사용스크립트 <- 플레이어 공격력
    // 예상 : 구조체 받아서 해체 스킬 레벨 공격계수 범위 등등
    public void SkillUpgrade()
    {

    }
    */
    // 젬이 플레이어에게 닿으면 젬에게서 경험치프로퍼티를 받는다
    public void GainExp(int exp)
    {
        _currentExp += exp;
        if (_currentExp >= _requireExp)
        {
            LevelUp();
        }
    }
}
