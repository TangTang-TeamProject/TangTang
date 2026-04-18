using UnityEngine;


public class FireObstacles : BaseEnemy
{
    [SerializeField] private EnemyData_SO _fireData;
    [SerializeField] private ItemData_SO _itemData;
      

    protected override void Awake()
    {
        if ( _fireData == null )
        {
            CPrint.Log("_fireData SO 연결 안됨");
            enabled = false;
            return;
        }       
        _contactDamage = _fireData.ContactDamage;
    }

    protected override void Start()
    {
        return;
    }
}
