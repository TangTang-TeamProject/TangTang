using UnityEngine;

public class Projectile_Splited : BaseProjectile
{
    public override void Init(ProjectilePool pool, Transform targetPos)
    {
        base.Init(pool, targetPos);

        transform.localScale = new Vector3(_projectileSO.SizeScale, _projectileSO.SizeScale, 1);
    }
    public override void Destroy()
    {        

        _targetPos = null;
        Destroy(gameObject);
    }

    public override void CutOff()
    {
        
    }
}
