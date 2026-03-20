using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireObstacles : MonoBehaviour
{
    [SerializeField] private EnemyData_SO _fireData;
    [SerializeField] private float _radius = 0.5f;

    private LayerMask _playerLayer;
    private string _playerTag = "Player";
    
    private float _damage;
    private float _atkCycle;
    private float _nextAtk = 0f;
    private Vector3 _basePos;

    void Awake()
    {
        if ( _fireData == null )
        {
            CPrint.Log("_fireData SO 연결 안됨");
            enabled = false;
            return;
        }

        _damage = _fireData.DMG;
        _atkCycle = _fireData.ATKCycle;
        _playerLayer = LayerMask.GetMask("Player");

        _basePos = transform.position;
        _basePos.y += 0.5f;
    }

    void Update()
    {
        TriggeredPlayer();
    }

    public void TriggeredPlayer()
    {
        if (Time.time <  _nextAtk)
        {
            return;
        }

        Collider2D[] hit = Physics2D.OverlapCircleAll(_basePos, _radius, _playerLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].CompareTag(_playerTag)) 
            {
                if (hit[i].gameObject.TryGetComponent(out IDamagables damagables))
                {
                    damagables.Hit(_damage);
                    _nextAtk = Time.time + _atkCycle;
                    return;
                }
                else
                {
                    CPrint.Log($"{hit[i].gameObject} IDamagables 못 찾음");
                    return;
                }
            }
        }
    }
}
