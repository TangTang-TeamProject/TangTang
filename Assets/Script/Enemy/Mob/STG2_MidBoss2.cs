using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class STG2_MidBoss2 : BaseEnemy
{
    [Header("HP Bar 연결")]
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private Image _HPBarImage;

    [Header("랜덤 박스")]
    [SerializeField] private GameObject _itemParent;
    [SerializeField] private GameObject _randomBox;

    [Header("투사체 연결")]
    [SerializeField] private GameObject _projectile;

    private Vector2 _shootDir;
    private Vector2 _shootOrigin;
    private float _nextShoot;

    protected override void Awake()
    {
        base.Awake();

        _HPBar.SetActive(true);
        _HPBarImage.fillAmount = 1f;
        _shootOrigin = transform.position;
        _shootOrigin.y += 0.5f;

        _nextShoot = Timer.Instance.RealTime + _atkCycle;
    }


    protected override void Update()
    {
        base.Update();

        if (!CanUpdate())
        {
            return;
        }

        if (Timer.Instance.RealTime >= _nextShoot)
        {
            StartCoroutine(StopToShoot());
            StartCoroutine(ShootCoroutine());
            _nextShoot = Timer.Instance.RealTime + _atkCycle;
        }


        Chase();
        CheckDamaged();
    }

    void LateUpdate()
    {
        MoveIntoBattlezone();
    }

    protected override void Hit(IAttackables attackables)
    {
        base.Hit(attackables);
        float ratio = _maxHp / _monsterData.HP;
        _HPBarImage.fillAmount = ratio;
    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출

        Timer.Instance.IsBossDie(false);
        _HPBar.SetActive(false);
        Instantiate(_randomBox, transform.position, Quaternion.identity, _itemParent.transform);
        Destroy(gameObject);
    }

    private void ShootProjectile()
    {
        Vector2 startVec = transform.position;
        startVec.y += 0.5f;
        _shootDir = (Vector2)_target.transform.position - startVec;
        _shootDir.Normalize();    

        GameObject go = Instantiate(_projectile, _shootOrigin, Quaternion.identity, transform);
        BaseProjectile projectile = go.GetComponent<BaseProjectile>();
        projectile.Init(null, _target.transform);
    }

    IEnumerator ShootCoroutine()
    {        
        for (int i = 0; i < 3; i++)
        {
            ShootProjectile();
            yield return new WaitForSeconds(0.2f);
        }        
    }

    IEnumerator StopToShoot()
    {
        _speed = 0;

        yield return new WaitForSeconds(1.2f);

        _speed = _monsterData.MoveSpeed;
    }
}
