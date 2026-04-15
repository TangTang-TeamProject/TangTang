using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / EquipLevelRegistry", fileName = "EquipLevelRegistrySO")]
public class EquipLevelRegistry : ScriptableObject
{
    [SerializeField]
    private List<EquipLevel_SO> equips = new List<EquipLevel_SO>();

    public IReadOnlyList<EquipLevel_SO> Equips => equips;

    private Dictionary<string, Dictionary<int, EquipLevel_SO>> dataDic = new Dictionary<string, Dictionary<int, EquipLevel_SO>>();

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

        for (int i = 0; i < equips.Count; i++)
        {
            if (!dataDic.ContainsKey(equips[i].EquipID))
            {
                dataDic.Add(equips[i].EquipID, new Dictionary<int, EquipLevel_SO>());
            }

            dataDic[equips[i].EquipID].Add(equips[i].Level, equips[i]);
        }
    }

    public EquipLevel_SO GetEquipsDataByIDLevel(string _ID, int _level)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out Dictionary<int, EquipLevel_SO> data))
        {
            return data[_level];
        }

        CPrint.Error("EquipRegistry - Cant Find");
        return null;
    }
}
