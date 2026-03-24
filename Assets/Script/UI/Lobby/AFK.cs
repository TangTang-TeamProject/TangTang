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

    private DateTime dateTime;
    private DateTime endTime;
    TimeSpan total;

    private void Awake()
    {
        rewardBTN.onClick.AddListener(() => GetReward());
    }

    private void Start()
    {
        CalcTimes();
    }

    private void Update()
    {
        AFKUIUpdate();
    }

    void AFKUIUpdate()
    {
        TimeSpan current = DateTime.UtcNow - dateTime;

        float percent = (float)(current.TotalSeconds / total.TotalSeconds);

        gaugeText.text = $"{(int)(percent * 100)}%";

        gauge.fillAmount = percent;

        gaugeTextPos.anchoredPosition = Vector3.Lerp(gaugeTextStart.anchoredPosition, gaugeTextEnd.anchoredPosition, percent);
    }


    void GetReward()
    {
        // 보상 누적 코드

        CPrint.Log("보상 수령");

        SaveManager.SetDate();

        CalcTimes();
    }

    void CalcTimes()
    {
        dateTime = new DateTime(SaveManager.saveData.dateTime);
        endTime = dateTime.AddMinutes(10);
        total = endTime - dateTime;
    }
}
