using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipDrag : MonoBehaviour
{
    [SerializeField]
    private Image img;
    [SerializeField]
    private EquipRegistry equipRegistry;

    public void SetDrag(string _id)
    {
        img.sprite = equipRegistry.GetEquipByID(_id).IMG;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
