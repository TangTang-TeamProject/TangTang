public class Projectile_SummonMob : BaseProjectile
{
    public override void Destroy()
    {       
        _targetPos = null;
        Destroy(gameObject);
    }


    public override void CutOff()
    {
        _targetPos = null;
        Destroy(gameObject);
    }

}
