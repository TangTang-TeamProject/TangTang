using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    [SerializeField] private GameObject _enemyPrefab;

    private Queue<BaseEnemy> _enemyPool = new Queue<BaseEnemy>();

    public void Add(BaseEnemy enemy)
    {
        _enemyPool.Enqueue(enemy);
    }

    public BaseEnemy GetEnemy(Transform parent)
    {
        if (_enemyPool.Count > 0)
        {
            BaseEnemy enemy = _enemyPool.Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }

        GameObject go = Instantiate(_enemyPrefab, parent);
        return go.GetComponent<BaseEnemy>();
    }

    public void Return(BaseEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
        _enemyPool.Enqueue(enemy);
    }
}
