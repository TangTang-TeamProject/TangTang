using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMap : MonoBehaviour
{
    [SerializeField]
    private Transform godObject;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float maxX = 15f;
    [SerializeField]
    private float minX = -15f;
    [SerializeField]
    private float maxY = 10f;
    [SerializeField]
    private float minY = -10f;
    [SerializeField]
    private float buffer = 0.1f;

    void Update()
    {
        CheckPos();
    }

    void CheckPos()
    {
        Vector3 calcPos = godObject.position;

        if (player.position.x > maxX)
        {
            calcPos.x = minX + buffer;
        }
        else if (player.position.x < minX)
        {
            calcPos.x = maxX - buffer;
        }

        if (player.position.y > maxY)
        {
            calcPos.y = minY + buffer;
        }
        else if (player.position.y < minY)
        {
            calcPos.y = maxY - buffer;
        }

        godObject.position = calcPos;
    }
}
