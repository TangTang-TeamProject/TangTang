using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameFlow : MonoBehaviour
{
    enum PlayStats
    {
        Playing,
        Pausing,
        Directing,
        GameOver,
    }

    [SerializeField]
    private PlayerCamera pCam;
    [SerializeField]
    private InfiniteMap map;
    [SerializeField]
    private Button pauseBTN;
    [SerializeField]
    private Lottery lotteryUI;
    [SerializeField]
    private SkillPick skillPickUI;
    [SerializeField]
    private IngameSettings pauseUI;
    [SerializeField]
    private Alarm AlarmUI;
    [SerializeField]
    private GameEnd gameEndUI;

    private Player player;

    private PlayStats situation = PlayStats.Playing;
    private int beforeTime = 0;

    private void Awake()
    {
        pauseBTN.onClick.AddListener(PauseButtonClick);

        player = FindFirstObjectByType<Player>();
    }

    private void Start()
    {
        player.OnDead += GameOver;
        Timer.Instance.BossSpawn += BossAppear;
        Timer.Instance.BossDie += BossDisappear;
        ItemManager.instance.SkillPick += SkillPickEvent;
        ItemManager.instance.LuckyBox += LotteryEvent;
        AlarmUI.BigWaveMode += ZoomOutCam;
        AlarmUI.BasicMode += ZoomInCam;
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.OnDead -= GameOver;
        }

        if (Timer.Instance != null)
        {
            Timer.Instance.BossSpawn -= BossAppear;
            Timer.Instance.BossDie -= BossDisappear;
        }

        if (ItemManager.instance)
        {
            ItemManager.instance.SkillPick -= SkillPickEvent;
            ItemManager.instance.LuckyBox -= LotteryEvent;
        }
    }

    private void Update()
    {
        int currentTime = (int)Timer.Instance.GameTime;

        if (beforeTime == currentTime)
            return;

        beforeTime = currentTime;

        AlarmUI.CheckAlarm(currentTime);
    }

    void PauseGame(bool isPause)
    {
        Timer.Instance.IsTimeStop(isPause);
    }

    void ChangeStats(PlayStats _next)
    {
        if (situation == _next)
        {
            return;
        }

        situation = _next;

        switch(situation)
        {
            case PlayStats.Playing:
                PauseGame(false);
                break;
            case PlayStats.Pausing:
                PauseGame(true);
                break;
            case PlayStats.Directing:
                PauseGame(true);
                break;
            case PlayStats.GameOver:
                PauseGame(true);
                break;
        }
    }

    void PauseButtonClick()
    {
        if (situation == PlayStats.Pausing)
        {
            ChangeStats(PlayStats.Playing);
            pauseUI.ActiveUI(false);
        }
        else if (situation == PlayStats.Playing)
        {
            ChangeStats(PlayStats.Pausing);
            pauseUI.ActiveUI(true);
        }
    }

    void SkillPickEvent()
    {
        ChangeStats(PlayStats.Directing);

        skillPickUI.StartPick(EndEvent);
    }

    void LotteryEvent()
    {
        ChangeStats(PlayStats.Directing);

        lotteryUI.StartLottery(EndEvent);
    }

    void EndEvent()
    {
        ChangeStats(PlayStats.Playing);
    }

    void ZoomOutCam()
    {
        pCam.ZoomOut();
    }

    void ZoomInCam()
    {
        pCam.ZoomIn();
    }


    void BossAppear()
    {
        map.MakeLock();
    }


    void BossDisappear(bool last)
    {
        map.MakeInfinate();

        AlarmUI.Clear();

        if (last)
        {
            GameClear();
        }
    }

    void GameOver()
    {
        ChangeStats(PlayStats.GameOver);

        gameEndUI.GameOver();
    }

    void GameClear()
    {
        ChangeStats(PlayStats.GameOver);

        gameEndUI.GameClear();
    }
}
