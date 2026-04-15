using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile_MidBoss2 : BaseProjectile
{
    [SerializeField] private GameObject _splitObject;
    [SerializeField] private float _splitTime = 1.0f;

    private float _splitAt;    
    private bool _isSplit = false;
    private float _splitAngle = 90f;
    private BaseProjectile _splitObjectA;
    private BaseProjectile _splitObjectB;


    private float _bulletSpeed;

    protected override void Awake()
    {
        base.Awake();

        _bulletSpeed = _projectileSO.BulletSpeed;

        _splitAt = Timer.Instance.RealTime + _splitTime;

        _splitObjectA = Instantiate(_splitObject).GetComponent<BaseProjectile>();
        _splitObjectB = Instantiate(_splitObject).GetComponent<BaseProjectile>();

        _splitObjectA.gameObject.SetActive(false);
        _splitObjectB.gameObject.SetActive(false);
    }

    public override void Init(ProjectilePool pool, Transform targetPos)
    {
        base.Init(pool, targetPos);
        
        transform.localScale = new Vector3(_projectileSO.SizeScale, _projectileSO.SizeScale, 1);
    }
    void Update()
    {
        if (Timer.Instance.RealTime < _splitAt)
        {
            ShootToTarget();
        }
        else
        {
            if (!_isSplit)
            {
                StartCoroutine(StartToSplit());                
            }
            ShootToTarget();
        }
        if (Timer.Instance.RealTime >= _spawnedTime + _aliveTime)
        {
            Destroy();
        }
    }

    protected override void ShootToTarget()
    {
        Vector2 pos = transform.position;

        pos += _shootDir * _projectileSO.BulletSpeed * Time.deltaTime;

        transform.position = pos;
    }

    private void Split()
    {
        float shootAngle = Mathf.Atan2(_shootDir.y, _shootDir.x) * Mathf.Rad2Deg;

        float splitA = (shootAngle + _splitAngle) * Mathf.Deg2Rad;
        float splitB = (shootAngle - _splitAngle) * Mathf.Deg2Rad;

        Vector2 splitAVec = new Vector2(Mathf.Cos(splitA), Mathf.Sin(splitA));
        Vector2 splitBVec = new Vector2(Mathf.Cos(splitB), Mathf.Sin(splitB));
       
        _splitObjectA.gameObject.SetActive(true);

        _splitObjectA.Init(null, _targetPos);
        _splitObjectA.transform.position = transform.position;
        _splitObjectA.SetShootDir(splitAVec);

        _splitObjectB.gameObject.SetActive(true);

        _splitObjectB.Init(null, _targetPos);        
        _splitObjectB.transform.position = transform.position;
        _splitObjectB.SetShootDir(splitBVec);

        _isSplit = true;
    }

    IEnumerator StartToSplit()
    {
        _bulletSpeed = 0f;

        yield return new WaitForSeconds(0.5f);

        Split();
        _bulletSpeed = _projectileSO.BulletSpeed;
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
