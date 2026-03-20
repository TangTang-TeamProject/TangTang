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

    private float xDist;
    private float yDist;

    private void Awake()
    {
        xDist = maxX - minX;
        yDist = maxY - minY;
    }

    void Update()
    {
        CheckPos();
    }

    void CheckPos()
    {
        Vector3 calcPos = Vector3.zero;

        if (player.position.x > maxX)
        {
            calcPos.x = -xDist;
        }
        else if (player.position.x < minX)
        {
            calcPos.x = xDist;
        }

        if (player.position.y > maxY)
        {
            calcPos.y = -yDist;
        }
        else if (player.position.y < minY)
        {
            calcPos.y = yDist;
        }

        godObject.position += calcPos;
    }

    void MakeBattleZone()
    { 
    
    }
}
