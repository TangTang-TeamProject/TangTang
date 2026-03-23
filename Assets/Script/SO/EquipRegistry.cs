using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / EquipRegistry", fileName = "EquipRegistrySO")]
public class EquipRegistry : ScriptableObject
{
    [SerializeField]
    private List<EquipData_SO> equips = new List<EquipData_SO>();

    public IReadOnlyList<EquipData_SO> Equips => equips;

    private Dictionary<int, EquipData_SO> dataDic = new Dictionary<int, EquipData_SO>();

    void NullCheck()
    {
        if (dataDic != null && dataDic.Count != 0)
        {
            return;
        }

        ReMake();
    }

    public void ReMake()
    {
        dataDic.Clear();

        for (int i = 0; i < equips.Count; i++)
        {
            dataDic.Add(equips[i].EquipID, equips[i]);
        }
    }


    public EquipData_SO GetEnemyByID(int _ID)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out EquipData_SO data))
        {
            return data;
        }

        CPrint.Error("EquipRegistry - Cant Find");
        return null;
    }
}
