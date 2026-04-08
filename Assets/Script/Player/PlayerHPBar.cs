using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Image _hpBar;
    private float _maxHp;
    private float _currentHp;

    private void Awake()
    {
        if(_player == null)
        {
            _player = GetComponentInParent<Player>();
        }
    }

    void Start()
    {
        StartCoroutine(Co_DelayedInput());
    }

    IEnumerator Co_DelayedInput()
    {
        yield return new WaitForSeconds(0.05f);
        _player.OnCurrentHPChange += CHpChange;
        _player.OnMaxHPChange += MHpChange;
        _maxHp = _player.MaxHp;
        _currentHp = _player.CurrentHp;
    }

    void CHpChange(float currentHp)
    {
        _currentHp = currentHp;
        ChangeBar();
    }

    void MHpChange(float maxHp)
    {
        _maxHp = maxHp;
        ChangeBar();
    }

    void ChangeBar()
    {
        float per = _currentHp / _maxHp;
        _hpBar.fillAmount = per;
    }
}
