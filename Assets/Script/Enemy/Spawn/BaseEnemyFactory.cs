using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyFactory : MonoBehaviour
{
    [SerializeField] protected GameObject _target;
    [SerializeField] protected EnemyPool _pool;

    public abstract BaseEnemy CreateEnemy(Vector2 position);
    
}
