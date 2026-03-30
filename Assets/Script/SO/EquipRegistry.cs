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

    private Dictionary<EquipType, EquipData_SO> dataDicType = new Dictionary<EquipType, EquipData_SO>();

    void NullCheckID()
    {
        if (dataDicID != null && dataDicID.Count != 0)
        {
            return;
        }

        MakeIDDic();
    }

    void NullCheckType()
    {
        if (dataDicType != null && dataDicType.Count != 0)
        {
            return;
        }

        MakeTypeDic();
    }

    public void MakeIDDic()
    {
        dataDicID.Clear();

        for (int i = 0; i < equips.Count; i++)
        {
            dataDicID.Add(equips[i].EquipID, equips[i]);
        }
    }

    public void MakeTypeDic()
    {
        dataDicType.Clear();

        for (int i = 0; i < equips.Count; i++)
        {
            dataDicType.Add(equips[i].Type, equips[i]);
        }
    }


    public EquipData_SO GetEquipByID(int _ID)
    {
        NullCheckID();

        if (dataDicID.TryGetValue(_ID, out EquipData_SO data))
        {
            return data;
        }

        CPrint.Error("EquipRegistry - Cant Find");
        return null;
    }

    public List<EquipData_SO> GetEquipByType(EquipType _type)
    {
        NullCheckType();

        List<EquipData_SO> data = new List<EquipData_SO>();

        if (dataDicType.TryGetValue(_type, out EquipData_SO dataPart))
        {
            data.Add(dataPart);
        }

        if (data.Count == 0)
        {
            CPrint.Error("EquipRegistry - Cant Find");
        }

        return data;
    }
}
