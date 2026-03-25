using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;

    private Queue<BaseProjectile> _projectilePool = new Queue<BaseProjectile>();

    void Awake()
    {
        if (_projectilePrefab == null)
        {
            CPrint.Log($"{this} : ProjectilePrefab ¿¬°á ¾ÈµÊ");
            enabled = false;
            return;
        }
    }

    public BaseProjectile GetProjectile(Transform parent)
    {
        if (_projectilePool.Count > 0)
        {
            BaseProjectile baseProjectile = _projectilePool.Dequeue();
            baseProjectile.gameObject.SetActive(true);
            return baseProjectile;
        }

        GameObject newProjectile = Instantiate(_projectilePrefab, parent);       
        return newProjectile.GetComponent<BaseProjectile>();
    }

    public void Return(BaseProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
        _projectilePool.Enqueue(projectile);        
    }
}
