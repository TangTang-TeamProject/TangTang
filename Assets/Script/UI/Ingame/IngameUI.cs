using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    enum PlayStats
    { 
        Playing,
        Pausing,
        Directing,
    }

    [SerializeField]
    private Player player;
    [SerializeField]
    private PlayerCamera pCam;
    [SerializeField]
    private InfiniteMap map;
    [SerializeField]
    private GameObject hurtUI;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private Button pauseBTN;
    [SerializeField]
    private GameObject pauseUI;

    private PlayStats situation = PlayStats.Playing;
    private int beforeTime = 0;
    private Coroutine hurtEffect;

    private WaitForSeconds hurtTime = new WaitForSeconds(1f);

    private void Awake()
    {
        pauseBTN.onClick.AddListener(PauseButtonClick);
    }

    private void Start()
    {
        player.OnHit += HurtUI;
        Timer.Instance.BossSpawn += BossAppear;
        Timer.Instance.BossDie += BossDisappear;
    }

    private void OnDestroy()
    {
        player.OnHit -= HurtUI;
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
        situation = _next;
    }

    void PauseButtonClick()
    {
        if (situation == PlayStats.Pausing)
        {
            ChangeStats(PlayStats.Playing);
            pauseUI.SetActive(false);
            PauseGame(false);
        }
        else if(situation == PlayStats.Playing)
        {
            ChangeStats(PlayStats.Pausing);
            pauseUI.SetActive(true);
            PauseGame(true);
        }
    }

    void HurtUI()
    {
        if (hurtEffect != null)
        { 
            StopCoroutine(hurtEffect);
        }

        hurtEffect = StartCoroutine(HurtCoroutine());
    }

    IEnumerator HurtCoroutine()
    {
        hurtUI.SetActive(true);

        yield return hurtTime;

        hurtUI.SetActive(false);

        yield break;
    }

    void BossAppear()
    {
        StartCoroutine(BossAppearCoroutine());
    }

    IEnumerator BossAppearCoroutine()
    {
        ChangeStats(PlayStats.Directing);
        PauseGame(true);

        map.MakeLock();

        yield return pCam.ZoomCoroutine();

        PauseGame(false);
        ChangeStats(PlayStats.Playing);
        yield break;
    }

    void BossDisappear()
    {
        map.MakeInfinate();
    }
}
