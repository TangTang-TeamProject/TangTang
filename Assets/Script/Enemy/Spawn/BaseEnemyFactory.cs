using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyFactory : MonoBehaviour
{
    public abstract BaseEnemy CreateEnemy(Vector2 position);
    
}
