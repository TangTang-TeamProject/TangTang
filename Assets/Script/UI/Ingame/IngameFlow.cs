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
    private TextMeshProUGUI timeText;
    [SerializeField]
    private Button pauseBTN;
    [SerializeField]
    private IngameSettings pauseUI;
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
    }

    private void OnDisable()
    {
        player.OnDead -= GameOver;
        Timer.Instance.BossSpawn -= BossAppear;
        Timer.Instance.BossDie -= BossDisappear;
    }

    private void Update()
    {
        int currentTime = (int)Timer.Instance.GameTime;

        if (beforeTime == currentTime)
            return;

        beforeTime = currentTime;

        MakeTimeText(currentTime);
    }

    void MakeTimeText(int _time)
    {
        int min = _time / 60;

        int sec = _time % 60;

        timeText.text = $"{min:00} : {sec:00}";
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

    void BossAppear()
    {
        StartCoroutine(BossAppearCoroutine());
    }

    IEnumerator BossAppearCoroutine()
    {
        ChangeStats(PlayStats.Directing);

        map.MakeLock();

        yield return pCam.ZoomCoroutine();

        ChangeStats(PlayStats.Playing);

        yield break;
    }

    void BossDisappear()
    {
        map.MakeInfinate();
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
