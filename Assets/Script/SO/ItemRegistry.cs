using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameData / ItemRegistry", fileName = "ItemRegistrySO")]
public class ItemRegistry : ScriptableObject
{
    [SerializeField]
    private List<ItemData_SO> items = new List<ItemData_SO>();

    public IReadOnlyList<ItemData_SO> Items => items;

    private Dictionary<int, ItemData_SO> dataDic = new Dictionary<int, ItemData_SO>();

    public void ReMake()
    {
        dataDic.Clear();

        for (int i = 0; i < items.Count; i++)
        {
            dataDic.Add(items[i].ItemID, items[i]);
        }
    }

    public ItemData_SO GetEnemyByID(int _ID)
    {
        if (dataDic.TryGetValue(_ID, out ItemData_SO data))
        {
            return data;
        }

        CPrint.Log("EnemyRegistry - Error");
        return null;
    }
}
