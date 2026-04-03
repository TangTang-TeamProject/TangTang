using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [Header("EnemyPool 참조 연결")]
    [SerializeField] private EnemyPool _pool;
    [Header("GemFactory 연결")]
    [SerializeField] private GemFactory _factory;

    [Header("Gem Registry")]
    [SerializeField] private ItemRegistry _itemRegistry;

    private List<ItemData_SO> _gemDatas;

    void Awake()
    {
        if (_pool == null)
        {
            CPrint.Error($"{this} : EnemyPool 연결 안됨");
            enabled = false;
            return;
        }

        if (_factory == null)
        {
            CPrint.Error($"{this} : GemFactory 연결 안됨");
            enabled = false;
            return;
        }

        _pool.OnEnemyDead += SpawnGem; // enemy 사망시 젬 스폰 구독.
    }
    
    private void FindGemData()
    {        
    }

    public void SpawnGem(BaseEnemy enemy)
    {
        if (enemy.MobType != EnemyType.Normal)
        {
            return;
        }

        GemType gemType;

        if (enemy.ExpDrop < 10) // 10 미만 스몰
        {
            gemType = GemType.Small;
        }
        else if (enemy.ExpDrop < 20) // 20 미만 미디움
        {
            gemType = GemType.Medium;
        }
        else // 20 이상 라지
        {
            gemType = GemType.Large;
        }        

        ExpGem gem = _factory.CreateGem(enemy.gameObject.transform.position, gemType); // 해당 type의 젬 생성하기.
        gem.GetExp += ItemManager.instance.GetGems;
    }
}

