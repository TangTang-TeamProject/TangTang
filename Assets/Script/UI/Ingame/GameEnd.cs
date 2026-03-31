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

    private void OnEnable()
    {
        MakeTimeText((int)Timer.Instance.RealTime);
    }

    void MakeTimeText(int _time)
    {
        int min = _time / 60;

        int sec = _time % 60;

        fullTimeText.text = $"{min:00} : {sec:00}";
    }

    void BackToMenu()
    {
        GoldCalc();
        SceneChanger.instance.MoveScene(Scenes.Lobby);
    }

    void ReGame()
    {
        GoldCalc();
        SceneChanger.instance.ReLoadScene();
    }

    void GoldCalc()
    { 
        
    } 
}
