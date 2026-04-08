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


    private DateTime dateTime;
    private DateTime endTime;
    TimeSpan total;

    private float updateCycle = 1;
    private float currentCycle = 0;

    private void Awake()
    {
        rewardBTN.onClick.AddListener(() => GetReward());
    }

    private void Start()
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

        gaugeTextPos.anchoredPosition = Vector3.Lerp(gaugeTextStart.anchoredPosition, gaugeTextEnd.anchoredPosition, percent);
    }

    void GetReward()
    {
        // 보상 누적 코드

        CPrint.Log("보상 수령");

        SaveManager.SetDate();
        SaveManager.Save();

        CalcTimes();
        AFKUIUpdate();
        UpdateGold();
    }

    void CalcTimes()
    {
        if (dateTime == null)
        {
            CPrint.Error("SaveManager-dateTime == null");
        }

        dateTime = new DateTime(SaveManager.data.dateTime);
        // 수령 주기 이후에 추가 필요
        endTime = dateTime.AddMinutes(10);
        total = endTime - dateTime;
    }

    void UpdateGold()
    {
        goldText.text = SaveManager.data.gold.ToString();
    }
}
