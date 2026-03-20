using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyFlow : MonoBehaviour
{
    enum LobbyState
    {
        Main,
        Equip,
        Start,
        Setting,
        Quit
    }

    [SerializeField]
    private Button startBTN;
    [SerializeField]
    private Button equipBTN;
    [SerializeField]
    private Button settingBTN;
    [SerializeField]
    private Button quitBTN;
    [SerializeField]
    private Button backBTN;
    [SerializeField]
    private GameObject MainPanel;
    [SerializeField]
    private GameObject stagePanel;
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private GameObject equipPanel;

    LobbyState _state;

    private void Awake()
    {
        equipBTN.onClick.AddListener(() => ChangeState(LobbyState.Equip));
        settingBTN.onClick.AddListener(() => ChangeState(LobbyState.Setting));
        backBTN.onClick.AddListener(() => ChangeState(LobbyState.Main));
        startBTN.onClick.AddListener(() => ChangeState(LobbyState.Start));
        quitBTN.onClick.AddListener(() => ChangeState(LobbyState.Quit));
    }

    void ChangeState(LobbyState nextState)
    {
        _state = nextState;

        switch (_state)
        {
            case LobbyState.Main:
                BackToMain();
                break;
            case LobbyState.Equip:
                Equip();
                break;
            case LobbyState.Setting:
                Setting();
                break;
            case LobbyState.Start:
                GameStart();
                break;
            case LobbyState.Quit:
                GameEnd();
                break;
        }
    }

    void BackToMain()
    {
        MainPanel.SetActive(true);
        stagePanel.SetActive(false);
        settingPanel.SetActive(false);
        equipPanel.SetActive(false);
    }

    void Equip()
    {
        MainPanel.SetActive(false);
        equipPanel.SetActive(true);
    }

    void Setting()
    {
        MainPanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    void GameStart()
    {
        MainPanel.SetActive(false);
        stagePanel.SetActive(true);
    }

    void GameEnd()
    { 
        Application.Quit();
    }
}
