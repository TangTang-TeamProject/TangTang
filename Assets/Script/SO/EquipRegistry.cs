using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / EquipRegistry", fileName = "EquipRegistrySO")]
public class EquipRegistry : ScriptableObject
{
    [SerializeField]
    private List<EquipData_SO> equips = new List<EquipData_SO>();

    public IReadOnlyList<EquipData_SO> Equips => equips;

    private Dictionary<int, EquipData_SO> dataDicID = new Dictionary<int, EquipData_SO>();

    private Dictionary<EquipType, List<EquipData_SO>> dataDicType = new Dictionary<EquipType, List<EquipData_SO>>();

    void NullCheck()
    {
        if (dataDicType != null && dataDicType.Count != 0 && dataDicID != null && dataDicID.Count != 0)
        {
            return;
        }

        MakeDic();
    }

    public void MakeDic()
    {
        dataDicID.Clear();
        dataDicType.Clear();

        for (int i = 0; i < equips.Count; i++)
        {
            dataDicID.Add(equips[i].EquipID, equips[i]);
            
            if (!dataDicType.ContainsKey(equips[i].Type))
            {
                dataDicType.Add(equips[i].Type, new List<EquipData_SO>());
            }

            dataDicType[equips[i].Type].Add(equips[i]);
        }
    }


    public EquipData_SO GetEquipByID(int _ID)
    {
        NullCheck();

        if (dataDicID.TryGetValue(_ID, out EquipData_SO data))
        {
            return data;
        }

        CPrint.Error("EquipRegistry - Cant Find");
        return null;
    }

    public List<EquipData_SO> GetEquipByType(EquipType _type)
    {
        NullCheck();

        if (dataDicType.TryGetValue(_type, out List<EquipData_SO> data))
        {
            return data;
        }

        CPrint.Error("EquipRegistry - Cant Find");

        return null;
    }
}
