using UnityEngine;


public class FireObstacles : BaseEnemy
{
    [SerializeField] private EnemyData_SO _fireData;
    [SerializeField] private ItemData_SO _itemData;
   

    private float _atk;  

    public float Atk => _atk;

    protected override void Awake()
    {
        if ( _fireData == null )
        {
            CPrint.Log("_fireData SO ¿¬°á ¾ÈµÊ");
            enabled = false;
            return;
        }
        _atk = _fireData.ContactDamage;
    }

    protected override void Start()
    {
        return;
    }
}
