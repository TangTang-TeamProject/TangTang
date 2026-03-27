using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.WSA;

public class LayerSorting : MonoBehaviour
{
    
    private bool _isPlayer = false;

    private SpriteRenderer _sr;
    private SortingGroup _sg;
    
    private string _playerLayerString = "Player";

    private int _yValue;
    private float _prevY;

    void Awake()
    {        
        if (gameObject.layer == LayerMask.NameToLayer(_playerLayerString))
        {
            _isPlayer = true;
            _sg = GetComponent<SortingGroup>();
            if (_sg == null)
            {
                CPrint.Warn($"{this} : SortingGroup 연결 안됨");
                enabled = false;
                return;
            }
        }
        else
        {
            _isPlayer = false;
            _sr = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        float currentYvalue = transform.position.y;

        if (Mathf.Abs(_prevY - currentYvalue) > 0.001f)
        {
            LayerSort();
        }
    }

    private void LayerSort()
    {        
        _yValue = -(int)(transform.position.y * 100); 

        int sortOrder = _yValue; // y 좌표 반올림한 후 * -1000 -> order layer 값 

        if (_isPlayer)
        {
            _sg.sortingOrder = sortOrder;
        }
        else
        {
            _sr.sortingOrder = sortOrder;
        }
        _prevY = _yValue;   
    }
}
