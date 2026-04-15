using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipDropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Image equipimg;
    [SerializeField]
    private EquipType et;


    private event Action<EquipType> drop;


    public void SetDropWear(Action<EquipType> _drop)
    { 
        drop = _drop;
    }

    public void OnDrop(PointerEventData eventData)
    {
        drop?.Invoke(et);
    }

    public void ChangeIMG(Sprite _sprite)
    {
        equipimg.sprite = _sprite;
    }
}
