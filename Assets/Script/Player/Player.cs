using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagables
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Hit(float damage)
    {
        CPrint.Log("플레이어 맞았음");
    }

    public void Die()
    {

    }
}
