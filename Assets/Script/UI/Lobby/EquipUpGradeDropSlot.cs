using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipUpGradeDropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Image equipimg;

    private event Action drop;


    public void SetDropUpGrade(Action _drop)
    {
        drop = _drop;
    }

    public void OnDrop(PointerEventData eventData)
    {
        drop?.Invoke();
    }

    public void ChangeIMG(Sprite _sprite)
    {
        equipimg.sprite = _sprite;
    }
}
