using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI desc;
    [SerializeField]
    private EquipRegistry equipRegistry;


    public void SetInfo(string _id)
    {
        itemName.text = equipRegistry.GetEquipByID(_id).EquipName;
        desc.text = equipRegistry.GetEquipByID(_id).Desc;
    }
}
