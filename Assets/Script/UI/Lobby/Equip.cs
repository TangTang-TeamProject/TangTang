using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip : MonoBehaviour
{
    [SerializeField]
    private Image weapon;
    [SerializeField]
    private Image head;
    [SerializeField]
    private Image body;
    [SerializeField]
    private Image legs;
    [SerializeField]
    private Image cape;
    [SerializeField]
    private EquipRegistry equip;


    private void Start()
    {
        DataRefresh();
    }

    void DataRefresh()
    {
        ChangeImg(weapon, EquipType.Weapon);
        ChangeImg(head, EquipType.Head);
        ChangeImg(body, EquipType.Body);
        ChangeImg(legs, EquipType.Leg);
        ChangeImg(cape, EquipType.Cape);
    }

    void ChangeImg(Image img, EquipType type)
    {
        int id = SaveManager.GetEquip(type);

        img.sprite = equip.GetEquipByID(id).IMG;
    }
}
