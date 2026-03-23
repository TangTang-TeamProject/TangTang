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
    

    public void SpawnGem(BaseEnemy enemy)
    {
        int typeCnt = Enum.GetValues(typeof(GemType)).Length;
        int randTypeInt = UnityEngine.Random.Range(0, typeCnt); // 타입 번호들 중 랜덤 번호 고르기

        GemType gemType = (GemType)randTypeInt; // int -> enum 변환

        _factory.CreateGem(enemy.gameObject.transform.position, gemType); // 해당 type의 젬 생성하기.

    }
}

