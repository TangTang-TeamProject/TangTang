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

    private WaitForSeconds _nextCheck = new WaitForSeconds(0.1f);
    private Coroutine _rootCo;
    private Coroutine _absorbeCo;
    private Vector2 _center;
    private float _radius;
    private float _absorbeMultiply;

    private void Start()
    {
        _targetLayer = LayerMask.GetMask(_targetLayerMask);
        _radius = _player.PlayerCol.bounds.extents.x;
        _absorbeMultiply = 3.0f;
        _rootCo = StartCoroutine(Co_CheckLoot());
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

    IEnumerator Co_CheckLoot()
    {
        while (true)
        {
            if (_player.PlayerState == Player.EPlayerState.Dead)
            {
                StopCoroutine(_rootCo);
                _rootCo = null;
                break;
            }

            _center = _player.PlayerCol.bounds.center;
            int count = Physics2D.OverlapCircleNonAlloc(_center, _radius * 0.8f, _hits, _targetLayer);

            for (int i = 0; i < count; i++)
            {
                // 마지막으로 널 체크 한번 더
                // 경험치 체력회복 폭탄 자석 태그
                if (_hits[i] != null && _hits[i].TryGetComponent(out Items target))
                {
                    // 어차피 닿으면 사라짐 해쉬셋 필요없음
                    target.SetActiveFalse();
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

            _hitRecord.RemoveWhere(target => target == null || (target as MonoBehaviour) == null);

            _center = _player.PlayerCol.bounds.center;
            int count = Physics2D.OverlapCircleNonAlloc(_center, _radius * _absorbeMultiply, _hits, _targetLayer);

            for (int i = 0; i < count; i++)
            {
                // 마지막으로 널 체크 한번 더
                if (_hits[i] != null && _hits[i].TryGetComponent(out Items target))
                {
                    _thisFrameRecord.Add(target);

                    if (_hitRecord.Add(target))
                    {
                        CPrint.Log("대상 발견");
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

    public void AbsorbeRangeUP(float percent)
    {
        _absorbeMultiply *= 1 + percent * 0.01f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_center, _radius*_absorbeMultiply);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_center, _radius*0.8f);

    }
}
