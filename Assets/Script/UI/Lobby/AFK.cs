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
    private GameObject gaugeTextPos;
    [SerializeField]
    private TextMeshProUGUI gaugeText;

    private void Awake()
    {
        rewardBTN.onClick.AddListener(() => GetReward());
    }

    private void Update()
    {
        DateTime endTime = SaveManager.saveData.dateTime.AddMinutes(30);

        TimeSpan total = endTime - SaveManager.saveData.dateTime;
        TimeSpan current = DateTime.UtcNow - SaveManager.saveData.dateTime;

        double percent = current.TotalSeconds / total.TotalSeconds;

        gaugeText.text = ((int)(percent * 100)).ToString();

        gauge.fillAmount = (float)percent;
    }


    void GetReward()
    {
        // 보상 누적 코드

        SaveManager.SetDate();
    }
}
