using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LobbyFlow : MonoBehaviour
{
    enum LobbyState
    {
        Main,
        Character,
        Equip,
        Stage,
        Setting,
        Quit
    }

    [SerializeField]
    private Button startBTN;
    [SerializeField]
    private Button charBTN;
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
    private GameObject charPanel;
    [SerializeField]
    private GameObject stagePanel;
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private GameObject equipPanel;

    LobbyState _state;

    private void Awake()
    {
        charBTN.onClick.AddListener(() => ChangeState(LobbyState.Character));
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
                Show(MainPanel);
                backBTN.gameObject.SetActive(false);
                break;
            case LobbyState.Character:
                Show(charPanel);
                backBTN.gameObject.SetActive(true);
                break;
            case LobbyState.Equip:
                Show(equipPanel);
                backBTN.gameObject.SetActive(true);
                break;
            case LobbyState.Setting:
                Show(settingPanel);
                backBTN.gameObject.SetActive(true);
                break;
            case LobbyState.Stage:
                Show(stagePanel);
                backBTN.gameObject.SetActive(true);
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
        charPanel.SetActive(false);
    }

    void Show(GameObject panel)
    {
        panel.SetActive(true);
    }

    void GameEnd()
    { 
        Application.Quit();
    }
}
