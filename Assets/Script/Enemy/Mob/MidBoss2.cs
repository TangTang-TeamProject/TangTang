
public class MidBoss2 : BaseEnemy
{
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

    }

    public override void Die()
    {
        // 사망 애니메이션

        // 보스 전리품 생성 호출

        Timer.Instance.IsBossDie(false);
        Destroy(gameObject);
    }
}
