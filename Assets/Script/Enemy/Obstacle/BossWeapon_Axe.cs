using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon_Axe : MonoBehaviour, IAttackables
{
    [Header("이동 및 회전 속도")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotSpeed = 30f;

    [Header("데미지")]
    [SerializeField] private float _damage = 10f;

    public float Damage => _damage;
    public float Stun => 0;


    private bool _isThrowed = false;
    private GameObject _target;
    private Vector2 _dirAimed;

    private float _minX = -15f;
    private float _maxX = 15f;
    private float _minY = -10f;
    private float _maxY = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        if (!_isThrowed)
        {
            return;
        }
      
        MovingToTarget();
    }

    public void ThrowedToTarget(GameObject target)
    {
        _isThrowed = true;
        _target = target;
        _dirAimed = _target.transform.position - transform.position;
        if (_dirAimed.magnitude < 0.001f)
        {
            _dirAimed = new Vector2(-1f, -1f);
        }
        _dirAimed.Normalize();
    }

    private void MovingToTarget()
    {
        // 회전
        Quaternion euler = Quaternion.Euler(0f, 0f, _rotSpeed);
        transform.Rotate(new Vector3(0f, 0f, _rotSpeed));

        // 울타리 검사 + 도끼 방향 벡터 반사
        CheckBoundary();

        // 이동
        Vector2 newPos = transform.position;        
        newPos += _dirAimed * _moveSpeed * Time.deltaTime;
        transform.position = newPos;

        
    }

    private void CheckBoundary()
    {
        bool contactedToWall = false;
        bool contactedToVertex = false;

        float nowX = transform.position.x;
        float nowY = transform.position.y;

        Vector2 collisionVec = Vector2.zero; // 충돌 면의 벡터
        float collisionAngle = 0f; // 충돌면의 각도

        float incidentAngle = 0f; // 입사각
        float reflectAngle = 0f; // 반사각
        float reflectRadian = 0f; // 반사라디안

        Vector2 reflectVec = Vector2.zero;

        if (nowX >= _maxX || nowX <= _minX)
        {
            contactedToWall = true;
            collisionVec = new Vector2(0, _dirAimed.y);          
        }       

        else if (nowY >= _maxY || nowY <= _minY)
        {
            contactedToWall = true;
            collisionVec = new Vector2(_dirAimed.x, 0);            
        }

        if ((nowX >= _maxX || nowX <= _minX) && (nowY >= _maxY || nowY <= _minY))
        {
            contactedToVertex = true;
            contactedToWall = false;
        }

        if (contactedToWall)
        {
            collisionVec.Normalize();
            collisionAngle = Mathf.Atan2(collisionVec.y, collisionVec.x) * 180f / Mathf.PI;

            incidentAngle = Vector2.SignedAngle(collisionVec, _dirAimed);
            reflectAngle = incidentAngle - 180 + collisionAngle;
            reflectRadian = reflectAngle * Mathf.Deg2Rad;

            reflectVec = new Vector2(Mathf.Cos(reflectRadian), Mathf.Sin(reflectRadian));
            reflectVec.Normalize();

            _dirAimed = reflectVec;
        }
        else if (contactedToVertex)
        {
            _dirAimed = -_dirAimed;
        }
        return;
    }    

    public void IsCatched()
    {
        Destroy(gameObject);
    }
}
