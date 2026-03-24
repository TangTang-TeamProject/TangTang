using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemLoot : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private string _targetLayerMask = "Item";


    private readonly Collider2D[] _hits = new Collider2D[300];
    private HashSet<Items> _hitRecord = new HashSet<Items>(300);
    private HashSet<Items> _thisFrameRecord = new HashSet<Items>(300);

    private WaitForSeconds _nextCheck = new WaitForSeconds(0.2f);
    private Coroutine _rootCo;
    private Coroutine _absorbeCo;

    private void Start()
    {
        _targetLayer = LayerMask.GetMask(_targetLayerMask);
        _rootCo = StartCoroutine(Co_CheckRoot());
        _absorbeCo = StartCoroutine(Co_CheckAbsorbe());
    }

    private void OnDisable()
    {
        if (_rootCo != null)
        {
            StopCoroutine(_rootCo);
            _rootCo = null;
        }

        if (_absorbeCo != null)
        {
            StopCoroutine(_absorbeCo);
            _absorbeCo = null;
        }
    }

    IEnumerator Co_CheckRoot()
    {
        while (true)
        {
            if (_player.PlayerState == Player.EPlayerState.Dead)
            {
                StopCoroutine(_rootCo);
                _rootCo = null;
                break;
            }

            Vector2 center = (Vector2)transform.position + _player.PlayerCol.offset;
            int count = Physics2D.OverlapCircleNonAlloc(center, _player.PlayerCol.radius, _hits, _targetLayer);

            for (int i = 0; i < count; i++)
            {
                // 마지막으로 널 체크 한번 더
                // 경험치 체력회복 폭탄 자석 태그
                if (_hits[i] != null && _hits[i].TryGetComponent(out ExpGem target))
                {
                    // 어차피 닿으면 사라짐 해쉬셋 필요없음
                    _player.GainExp((int)target.Exp);
                }
            }
            yield return _nextCheck;
        }
    }

    IEnumerator Co_CheckAbsorbe()
    {
        while (true)
        {
            if (_player.PlayerState == Player.EPlayerState.Dead)
            {
                StopCoroutine(_absorbeCo);
                _absorbeCo = null;
                break;
            }

            // 아이템이 추상클래스라 모노비헤이비어를 안씀
            _hitRecord.RemoveWhere(target => target == null || (target as MonoBehaviour) == null);

            Vector2 center = (Vector2)transform.position + _player.PlayerCol.offset;
            int count = Physics2D.OverlapCircleNonAlloc(center, _player.PlayerCol.radius + 0.05f, _hits, _targetLayer);

            for (int i = 0; i < count; i++)
            {
                // 마지막으로 널 체크 한번 더
                if (_hits[i] != null && _hits[i].TryGetComponent(out Items target))
                {
                    _thisFrameRecord.Add(target);

                    if (_hitRecord.Add(target))
                    {
                        target.GetItem(_player.gameObject);
                    }
                }
            }
            // 교집합만 남긴다
            _hitRecord.IntersectWith(_thisFrameRecord);
            _thisFrameRecord.Clear();

            yield return _nextCheck;
        }
    }
}
