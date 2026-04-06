using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : MonoBehaviour
{
    public enum AlarmType
    {
        Basic,
        WaveAlarm,
        MidBossAlarm,
        BossAlarm,
        WarnWave,
        MidBoss,
        Boss,
    }

    [System.Serializable]
    private class AlarmData
    {
        public int time;
        public AlarmType type;
    }

    public event Action BigWaveMode;
    public event Action BasicMode;

    [SerializeField]
    private Image mark;
    [SerializeField]
    private Image statMark;
    [SerializeField]
    private Sprite basic;
    [SerializeField]
    private Sprite wave;
    [SerializeField]
    private Sprite midBoss;
    [SerializeField]
    private Sprite boss;


    [SerializeField]
    private TextMeshProUGUI alarmText;
    [SerializeField]
    private string waveText;
    [SerializeField]
    private string midBossText;
    [SerializeField]
    private string bossText;


    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private List<AlarmData> alarms;

    private int alarmStack = 0;

    private AlarmType nowStats = AlarmType.Basic;

    private Coroutine coroutine;


    public void CheckAlarm(int _currentTime)
    {
        MakeTimeText(_currentTime);

        if (_currentTime == alarms[alarmStack].time)
        {
            DecideBehaviour(alarms[alarmStack].type);

            if (alarmStack < alarms.Count)
            {
                alarmStack++;
            }
        }
    }

    void MakeTimeText(int _time)
    {
        int min = _time / 60;

        int sec = _time % 60;

        timeText.text = $"{min:00} : {sec:00}";
    }

    public void Clear()
    {
        DecideBehaviour(AlarmType.Basic);
    }

    void DecideBehaviour(AlarmType _type)
    {
        if(nowStats == _type)
        {
            return;
        }

        nowStats = _type;

        switch (nowStats)
        {
            case AlarmType.WarnWave:
                BigWaveMode?.Invoke();
                statMark.sprite = wave;
                return;
            case AlarmType.MidBoss:
                BasicMode?.Invoke();
                statMark.sprite = midBoss;
                return;
            case AlarmType.Boss:
                BasicMode?.Invoke();
                statMark.sprite = boss;
                return;
            case AlarmType.Basic:
                statMark.sprite = basic;
                return;
            case AlarmType.WaveAlarm:
                WaveEvent();
                return;
            case AlarmType.MidBossAlarm:
                MidBossEvent();
                return;
            case AlarmType.BossAlarm:
                BossEvent();
                return;
        }
    }

    void WaveEvent()
    {
        this.gameObject.SetActive(true);


        if (coroutine != null)
        {
            StopCoroutine(coroutine);    
        }

        coroutine = StartCoroutine(AlarmCoroutine(waveText, wave));
    }

    void MidBossEvent()
    {
        this.gameObject.SetActive(true);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(AlarmCoroutine(midBossText, midBoss));
    }

    void BossEvent()
    {
        this.gameObject.SetActive(true);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(AlarmCoroutine(bossText, boss));
    }

    IEnumerator AlarmCoroutine(string _text, Sprite _img)
    {
        alarmText.text = _text;
        mark.sprite = _img;

        yield return new WaitForSeconds(2f);

        this.gameObject.SetActive(false);

        yield break;
    }
}
