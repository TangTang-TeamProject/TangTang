using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXPBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Image _expBar;
    [SerializeField] private Image _sliderHead;
    [SerializeField] private TMP_Text _expText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private float _minPosX = -920f;
    [SerializeField] private float _maxPosX = 920f;
    private float _requireExp;
    private float _currentExp;
    private int _currentlevel;

    private void Awake()
    {
        if (_player == null)
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
        _player.OnCurrentEXPChange += CExpChange;
        _player.OnRequireEXPChange += RExpChange;
        _requireExp = _player.RequireExp;
        _currentExp = _player.Exp;
        _currentlevel = _player.Level;
        _levelText.text = _currentlevel.ToString();
        ChangeBar();
    }

    void CExpChange(float currentExp)
    {
        _currentExp = currentExp;
        ChangeBar();
    }

    void RExpChange(float requireExp)
    {
        _requireExp = requireExp;
        _currentlevel++;
        _levelText.text = _currentlevel.ToString();
        ChangeBar();
    }

    void ChangeBar()
    {
        float per = _currentExp / _requireExp;
        _expBar.fillAmount = per;
        _expText.text = (per * 100f).ToString("F2") + "%";
        float x = Mathf.Lerp(_minPosX, _maxPosX, per);
        Vector2 pos = _sliderHead.rectTransform.anchoredPosition;
        pos.x = x;
        _sliderHead.rectTransform.anchoredPosition = pos;
    }
}
