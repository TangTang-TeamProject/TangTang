using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
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

    void MakeTimeText(int _time)
    {
        int min = _time / 60;

        int sec = _time % 60;

        fullTimeText.text = $"{min:00} : {sec:00}";
    }

    void BackToMenu()
    {
        Timer.Instance.IsTimeStop(false);
        SceneChanger.instance.MoveScene(Scenes.Lobby);
    }

    void ReGame()
    {
        Timer.Instance.IsTimeStop(false);
        SceneChanger.instance.ReLoadScene();
    }

    void GoldCalc(int _endgold)
    {
        earnCoinText.text = _endgold.ToString();
        SaveManager.CalcGold(_endgold);
        SaveManager.Save();
    }

    public void GameOver(int endgold)
    {
        endStatsText.text = "GameOver!";
        MakeTimeText((int)Timer.Instance.RealTime);
        GoldCalc(endgold);
        this.gameObject.SetActive(true);
    }

    public void GameClear(int endgold)
    {
        endStatsText.text = "GameClear!";
        MakeTimeText((int)Timer.Instance.RealTime);
        GoldCalc(endgold);
        this.gameObject.SetActive(true);
    }
}
