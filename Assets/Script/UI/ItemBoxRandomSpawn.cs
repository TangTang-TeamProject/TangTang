using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxRandomSpawn : MonoBehaviour
{
    [SerializeField]
    private float spawnRadius = 8.5f;
    [SerializeField]
    private float spawnCycle;
    [SerializeField]
    private ItemRegistry item;
    [SerializeField]
    private int spawnItemID = 0;

    private float currentTime = 0f;

    private void Awake()
    {
        currentTime = Timer.Instance.GameTime + spawnCycle;
    }

    private void Update()
    {
        if (Timer.Instance.GameTime > currentTime)
        {
            SpawnBox();

            currentTime += spawnCycle;
        }
    }

    void SpawnBox()
    {
        GameObject obj = Instantiate(item.GetItemByID(spawnItemID).Prefab, transform);

        Vector2 pos = Random.insideUnitCircle;

        obj.transform.position = pos * spawnRadius;
    }
}
