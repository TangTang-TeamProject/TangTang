using UnityEngine;

public class Projectile_Guided : BaseProjectile
{
    public override void Init(ProjectilePool pool, Transform targetPos)
    {
        _targetPos = targetPos;
        _pool = pool;

        _spawnedTime = Timer.Instance.RealTime;
        transform.localScale = new Vector3(_projectileSO.SizeScale, _projectileSO.SizeScale, 1);
    }

    private void Update()
    {
        _shootDir = _targetPos.position - transform.position;
        _shootDir.Normalize();

        SetShootDir(_shootDir);
        ShootToTarget();
        if (Timer.Instance.RealTime >= _spawnedTime + _aliveTime)
        {
            Destroy();
        }        
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
