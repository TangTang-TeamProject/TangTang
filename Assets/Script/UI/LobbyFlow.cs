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
        Stage,
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
        startBTN.onClick.AddListener(() => ChangeState(LobbyState.Stage));
        quitBTN.onClick.AddListener(() => ChangeState(LobbyState.Quit));
    }

    void ChangeState(LobbyState nextState)
    {
        _state = nextState;

        ClearAll();

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
            case LobbyState.Stage:
                GameStart();
                break;
            case LobbyState.Quit:
                GameEnd();
                break;
        }
    }

    void ClearAll()
    {
        MainPanel.SetActive(false);
        stagePanel.SetActive(false);
        settingPanel.SetActive(false);
        equipPanel.SetActive(false);
    }

    void BackToMain()
    {
        MainPanel.SetActive(true);
    }

    void Equip()
    {
        equipPanel.SetActive(true);
    }

    void Setting()
    {
        settingPanel.SetActive(true);
    }

    void GameStart()
    {
        stagePanel.SetActive(true);
    }

    void GameEnd()
    { 
        Application.Quit();
    }
}
