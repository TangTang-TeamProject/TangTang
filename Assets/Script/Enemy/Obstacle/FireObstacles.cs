using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireObstacles : MonoBehaviour
{
    [SerializeField] private EnemyData_SO _fireData;
   
    
    private float _atk;  

    public float Atk => _atk;

    void Awake()
    {
        if ( _fireData == null )
        {
            CPrint.Log("_fireData SO ¿¬°á ¾ÈµÊ");
            enabled = false;
            return;
        }
        _atk = _fireData.ATK;
    }
}
