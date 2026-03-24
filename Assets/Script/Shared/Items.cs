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

    void Update()
    {
        if (!_isAbsorbed) // 흡수 시작되었는지 검사
            return;

        MoveToTarget();

    }

    // 플레이어에게 흡수 시작 됐는지
    // true -> 플레이어에게 이동 / false -> 이동 X
    protected bool _isAbsorbed = false;

    // 플레이어가 흡수 시작할 때 호출       
    public virtual void GetItem(GameObject target)
    {
        _isAbsorbed = true; // 흡수 시작
        _target = target; // 타겟 설정
    }
    
    // 흡수 시작되었을때 타겟 방향으로 이동.   
    public virtual void MoveToTarget()
    {
        if (_target == null)
        {
            return;
        }

        Vector2 dir = _target.transform.position - transform.position;
        
        if (dir.magnitude > 0.001f)
        {
            dir.Normalize();
        }
        else
        {
            dir = Vector2.zero;
        }

        Vector2 pos = transform.position;
        pos += dir * _itemMoveSpeed * Time.deltaTime;
        transform.position = pos;
    }

    // 아이템 흡수 완료시 호출 (비활성화 함수)
    // 각 아이템 스크립트에서 override 해서 커스텀.
    public virtual void SetActiveFalse()
    {
        _target = null;
        _isAbsorbed = false;
        gameObject.SetActive(false);
    }

    // 아이템 Tag 반환.
    public virtual string GetItemTag()
    {
        return gameObject.tag;
    }
}
