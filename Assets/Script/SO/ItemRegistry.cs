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

    void NullCheck()
    {
        if (dataDic != null && dataDic.Count != 0)
        {
            return;
        }

        MakeDic();
    }

    public void MakeDic()
    {
        dataDic.Clear();

        for (int i = 0; i < items.Count; i++)
        {
            dataDic.Add(items[i].ItemID, items[i]);
        }
    }

    public ItemData_SO GetItemByID(int _ID)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out ItemData_SO data))
        {
            return data;
        }

        CPrint.Error("ItemRegistry - Cant Find");
        return null;
    }
}
