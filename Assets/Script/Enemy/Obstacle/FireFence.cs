using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFence : MonoBehaviour
{
    [SerializeField] private bool _isActive = false;

    private FireObstacles[] _fireObstacles;

    void Awake()
    {
        _fireObstacles = GetComponentsInChildren<FireObstacles>();        
    }

    private void Start()
    {
        Timer.Instance.BossSpawn += ToggleFenceStateTrue;
        Timer.Instance.BossDie += ToggleFenceStateFalse;
    }

    void Update()
    {
        if (_isActive)
        {
            ToggleObstacles(_isActive);
        }            
        else
        {
            ToggleObstacles(_isActive);
        }
    }

    public void ToggleFenceStateTrue()
    {
        _isActive = !_isActive;
    }

    public void ToggleFenceStateFalse(bool last)
    {
        _isActive = !_isActive;
    }

    private void ToggleObstacles(bool isActive)
    {
        for (int i = 0; i < _fireObstacles.Length; i++)
        {
            _fireObstacles[i].gameObject.SetActive(isActive); 
        }
    }
}
