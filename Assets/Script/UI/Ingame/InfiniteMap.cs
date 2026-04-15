using System;
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
    private bool isInfinite = true;

    private float xDist;
    private float yDist;

    public event Action<Vector3> OnTeleport;

    private void Awake()
    {
        xDist = maxX - minX;
        yDist = maxY - minY;
    }

    void Update()
    {
        if (isInfinite)
        {
            MakeInfinite();
        }
        else
        {
            MakeBattleZone();
        }
    }

    void MakeInfinite()
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
        OnTeleport?.Invoke(calcPos);
    }

    void MakeBattleZone()
    {
        Vector3 calcPos = player.position;

        if (player.position.x > maxX)
        {
            calcPos.x = maxX;
        }
        else if (player.position.x < minX)
        {
            calcPos.x = minX;
        }

        if (player.position.y > maxY)
        {
            calcPos.y = maxY;
        }
        else if (player.position.y < minY)
        {
            calcPos.y = minY;
        }

        player.position = calcPos;
    }

    public void MakeInfinate()
    {
        isInfinite = true;
    }

    public void MakeLock()
    {
        isInfinite = false;
    }
}
