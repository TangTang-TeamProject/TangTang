public class Projectile_Splited : BaseProjectile
{

    public override void Destroy()
    {        

        _targetPos = null;
        Destroy(gameObject);
    }

    public override void CutOff()
    {
        
    }
}
