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
    // 나중에 SO로 하든 컨트롤할 값이 들어오면 변경
    [SerializeField] private float _hp = 20;

    public EPlayerState _playerState { get; private set; }
    private WaitForSeconds _nextCheck = new WaitForSeconds(0.2f);
    private WaitForSeconds _invincibleTime;
    private Coroutine _checkCo;
    private Coroutine _invincibleCo;


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
}
