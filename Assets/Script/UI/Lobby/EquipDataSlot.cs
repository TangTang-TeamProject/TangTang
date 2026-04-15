using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipDataSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IEquipSlot
{
    [SerializeField]
    private EquipInfo infoPrefab;
    [SerializeField]
    private EquipDrag dragPrefab;

    private event Action<string> grab;

    private EquipInfo info;
    private EquipDrag drag;

    private string ID;

    public void SetID(string _id, Action<string> _grab)
    {
        ID = _id;
        grab = _grab;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (drag != null)
        {
            return;
        }

        drag = Instantiate(dragPrefab, transform);
        drag.transform.SetAsLastSibling();
        drag.SetDrag(ID);
        grab?.Invoke(ID);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CleanDrag();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(info != null)
        {
            return;
        }

        info = Instantiate(infoPrefab, transform);
        info.transform.SetAsLastSibling();
        info.SetInfo(ID);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CleanInfo();
    }

    void CleanDrag()
    {
        if (drag != null)
        {
            Destroy(drag.gameObject);
        }

        drag = null;
    }
    void CleanInfo()
    {
        if (info != null)
        {
            Destroy(info.gameObject);
        }

        info = null;
    }

    private void OnDestroy()
    {
        CleanDrag();
        CleanInfo();
    }
}
