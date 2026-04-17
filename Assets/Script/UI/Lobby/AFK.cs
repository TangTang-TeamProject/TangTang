using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AFK : MonoBehaviour
{
    [SerializeField]
    private Button rewardBTN;
    [SerializeField]
    private Image gauge;
    [SerializeField]
    private RectTransform gaugeTextStart;
    [SerializeField]
    private RectTransform gaugeTextEnd;
    [SerializeField]
    private RectTransform gaugeTextPos;
    [SerializeField]
    private TextMeshProUGUI gaugeText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private float rewardAmount = 100f;
    [SerializeField]
    private float rewardCycleMin = 10f;


    private DateTime dateTime;
    private DateTime endTime;
    TimeSpan total;

    private float updateCycle = 0.5f;
    private float currentCycle = 0;

    private void Awake()
    {
        rewardBTN.onClick.AddListener(() => GetReward());
    }

    private void OnEnable()
    {
        CalcTimes();
        UpdateGold();
    }

    private void Update()
    {
        UpdateCycle();
    }

    void UpdateCycle()
    {
        currentCycle += Time.deltaTime;

        if (currentCycle < updateCycle)
        {
            return;
        }

        currentCycle = 0;

        AFKUIUpdate();
    }

    void AFKUIUpdate()
    {
        TimeSpan current = DateTime.UtcNow - dateTime;

        float percent = Mathf.Clamp01((float)(current.TotalSeconds / total.TotalSeconds));

        gaugeText.text = $"{(int)(percent * 100)}%";

        gauge.fillAmount = percent;

        gaugeTextPos.anchoredPosition = Vector2.Lerp(gaugeTextStart.anchoredPosition, gaugeTextEnd.anchoredPosition, percent);
    }

    void GetReward()
    {
        if (!rewardBTN.interactable)
        {
            return;
        }
        rewardBTN.interactable = false;

        TimeSpan current = DateTime.UtcNow - dateTime;

        float percent = Mathf.Clamp01((float)(current.TotalSeconds / total.TotalSeconds));

        SaveManager.CalcGold(
        (int)(rewardAmount * percent));

        SaveManager.SetDate();
        SaveManager.Save();

        CalcTimes();
        AFKUIUpdate();
        UpdateGold();

        rewardBTN.interactable = true;
    }

    void CalcTimes()
    {
        dateTime = new DateTime(SaveManager.data.dateTime);
        endTime = dateTime.AddMinutes(rewardCycleMin);
        total = endTime - dateTime;
    }

    void UpdateGold()
    {
        goldText.text = SaveManager.data.gold.ToString();
    }
}
