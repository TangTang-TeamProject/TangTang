using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public enum GemType
    {
        Small,
        Medium,
        Large
    }

    [SerializeField] private GemPool _pool;
    [SerializeField] private ItemData_SO _expGem;

    private LayerMask _playerLayer;
    private string _playerTag = "Player";

    private float _exp;
    private float _xSize;
    private float _ySize;

    private float _checkTime = 0f;

    public void Init(GemPool pool)
    {
        if (_expGem == null)
        {
            CPrint.Log($"{this} -> SO 연결 안됨");
            enabled = false;
            return;
        }

        _pool = pool;

        _exp = _expGem.EXP;
        _xSize = transform.localScale.x;
        _ySize = 1.0f;

        _playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        TriggerPlayer();
    }

    private void TriggerPlayer()
    {
        if (Time.time < _checkTime)
        {
            return;
        }
        // 함수 진입 0.1초마다로 제한.
        _checkTime = Time.time + 0.1f;  

        Vector2 size = new Vector2(_xSize, _ySize);
        Collider2D[] hit = Physics2D.OverlapCapsuleAll(transform.position, size, CapsuleDirection2D.Vertical, 0f, _playerLayer);

        for (int i = 0; i <  hit.Length; i++)
        {
            if (hit[i].CompareTag(_playerTag))
            {
                // 플레이어 경험치 증가 함수 호출
                CPrint.Log($"{this} : 플레이어 경험치 {_exp} 증가.");
                gameObject.SetActive(false);
                return;
            }
        }
    }
}
