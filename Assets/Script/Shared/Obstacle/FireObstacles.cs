using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireObstacles : MonoBehaviour
{
    [SerializeField] private EnemyData_SO _fireData;

    private LayerMask _playerLayer;
    private string _playerTag = "Player";
    
    private float _damage;
    private float _atkCycle;
    private float _nextAtk = 0f;
    private float _radius;

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

        _radius = GetComponent<CircleCollider2D>() != null ? GetComponent<CircleCollider2D>().radius : 0f;

        if (_radius == 0f)
        {
            CPrint.Log($"{this} -> CircleCollider2D 없음");
        }
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

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _radius, _playerLayer);
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
