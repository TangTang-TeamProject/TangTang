using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMob : BaseEnemy
{   

    void Update()
    {        
        CheckDamaged();
    }

    private void FixedUpdate()
    {
        Chase();
    }
}
