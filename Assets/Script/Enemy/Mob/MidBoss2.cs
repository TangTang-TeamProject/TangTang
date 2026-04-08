
using UnityEngine;
using UnityEngine.UI;

public class MidBoss2 : BaseEnemy
{
    [Header("HP Bar 연결")]
    [SerializeField] private GameObject _HPBar;
    [SerializeField] private Image _HPBarImage;

    [Header("랜덤 박스")]
    [SerializeField] private GameObject _itemParent;
    [SerializeField] private GameObject _randomBox;

    protected override void Awake()
    {
        base.Awake();

        _HPBar.SetActive(true);
        _HPBarImage.fillAmount = 1f;
    }

    protected override void Update()
    {
        base.Update();

        if (!CanUpdate())
        {
            return;
        }

        Chase();
        CheckDamaged();        
    }

    void LateUpdate()
    {
        MoveIntoBattlezone();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Chase()
    {
        base.Chase();
    }

    protected override void Hit(float dmg)
    {
        base.Hit(dmg);
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
}
