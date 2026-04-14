public class Projectile_SummonMob : BaseProjectile
{
    public override void Destroy()
    {
        if (Timer.Instance.RealTime < _spawnedTime + _aliveTime)
        {
            return;
        }

        _targetPos = null;
        Destroy(gameObject);
    }


    public override void CutOff()
    {
        _targetPos = null;
        Destroy(gameObject);
    }
}
