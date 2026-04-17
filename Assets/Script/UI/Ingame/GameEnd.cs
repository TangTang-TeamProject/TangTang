using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    [SerializeField]
    private StageRegistry stageRegistry;
    [SerializeField]
    private TextMeshProUGUI endStatsText;
    [SerializeField]
    private TextMeshProUGUI earnCoinText;
    [SerializeField]
    private TextMeshProUGUI fullTimeText;
    [SerializeField]
    private Button reGameBTN;
    [SerializeField]
    private Button menuBTN;




    private void Awake()
    {
        reGameBTN.onClick.AddListener(ReGame);
        menuBTN.onClick.AddListener(BackToMenu);
    }

    public void GameOver(int endgold)
    {
        SoundManager.Instance.PlaySfx(ESfxType.GameOver);
        endStatsText.text = "GameOver!";
        MakeTimeText((int)Timer.Instance.RealTime);
        GoldCalc(endgold);
        this.gameObject.SetActive(true);
    }

    public void GameClear(int endgold)
    {
        SoundManager.Instance.PlaySfx(ESfxType.GameClear);
        endStatsText.text = "GameClear!";
        MakeTimeText((int)Timer.Instance.RealTime);
        GoldCalc(endgold);
        CheckUnLock();
        this.gameObject.SetActive(true);
    }


    void MakeTimeText(int _time)
    {
        int min = _time / 60;

        int sec = _time % 60;

        fullTimeText.text = $"{min:00} : {sec:00}";
    }

    void BackToMenu()
    {
        SceneChanger.instance.MoveScene(Scenes.Lobby);
    }

    void ReGame()
    {
        SceneChanger.instance.ReLoadScene();
    }

    void GoldCalc(int _endgold)
    {
        earnCoinText.text = _endgold.ToString();
        SaveManager.CalcGold(_endgold);
        SaveManager.Save();
    }

    void CheckUnLock()
    {
        string n = SceneChanger.instance.NowScene();

        StageData_SO sd = stageRegistry.GetStageDataByID(n);

        if (sd.UnLockStage)
        {
            SaveManager.UnLockChar(sd.UnLockChar);

            SaveManager.Save();
        }
    }
}
