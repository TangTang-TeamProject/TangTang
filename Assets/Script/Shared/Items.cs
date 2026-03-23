using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour
{
    [Header("아이템 데이터 SO")]
    [SerializeField] protected ItemData_SO _itemData;
    [Header("아이템 흡수 속도")]
    [SerializeField] protected float _itemMoveSpeed = 5f;

    protected GameObject _target;
    protected string _tag;

    private void Awake()
    {
        _tag = gameObject.tag;
    }

    void Update()
    {
        if (!_isAbsorbed)
            return;

        MoveToTarget();

    }

    // 플레이어에게 흡수 시작 됐는지
    // true -> 플레이어에게 이동 / false -> 이동 X
    protected bool _isAbsorbed = false;

    // 플레이어가 흡수 시작할 때 호출   
    // ㄴ 현재 반환값 id 로 통일 안되었으므로 각 아이템 스크립트에서 직접 구현
    public abstract float GetItem(GameObject target);
    

    public virtual void MoveToTarget()
    {
        if (_target == null)
        {
            return;
        }

        Vector2 dir = _target.transform.position - transform.position;
        
        dir.Normalize();

        Vector2 pos = transform.position;
        pos += dir * _itemMoveSpeed * Time.deltaTime;                
    }

    public virtual void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
