using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
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
    private bool isPause;

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
    }

    private void OnDestroy()
    {
        player.OnHit -= HurtUI;
        Timer.Instance.BossSpawn -= BossAppear;
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

    void PauseButtonClick()
    {
        if (isPause)
        {
            isPause = false;
            ResumeGame();
        }
        else
        {
            isPause = true;
            PauseGame();
        }
    }

    void ResumeGame()
    {
        Timer.Instance.IsTimeStop(false);
    }

    void PauseGame()
    {
        Timer.Instance.IsTimeStop(true);
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
        PauseGame();
        map.MakeLock();

        //Ä«¸Þ¶ó ÁÜ ¾Æ¿ô

        yield return hurtTime;

        // Ä«¸Þ¶ó ÁÜ ÀÎ
        
        ResumeGame();

        yield break;
    }

    void BossDisappear()
    {
        map.MakeInfinate();
    }
}
