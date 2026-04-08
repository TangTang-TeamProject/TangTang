using UnityEngine;
using UnityEngine.Rendering;


public class LayerSorting : MonoBehaviour
{
    
    //private bool _isPlayer = false;

    private SpriteRenderer _sr;
    private SortingGroup _sg;

    private EnemyType _enemyType;

    //private string _playerLayerString = "Player";

    private int _yValue;
    private float _prevY;
    private bool _isFinalBoss = false;

    void Awake()
    {        
        _sr = GetComponent<SpriteRenderer>();
        if (TryGetComponent(out BossMob finalBoss))
        {
            _sg = finalBoss.GetComponent<SortingGroup>();
            _isFinalBoss = true;
        }
    }

    private void Update()
    {
        if (Time.frameCount % 3 != 0)
        {
            return;
        }

        float currentYvalue = transform.position.y;

        if (Mathf.Abs(_prevY - currentYvalue) > 0.001f)
        {
            LayerSort();
        }
    }

    private void LayerSort()
    {        
        _yValue = -(int)(transform.position.y * 1000); 

        int sortOrder = _yValue; // y 좌표 반올림한 후 * -1000 -> order layer 값 
        
        if (_isFinalBoss)
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
