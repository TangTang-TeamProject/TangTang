using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour
{
    [SerializeField]
    private PlayerRegistry playerRegistry;

    [SerializeField]
    private Button startBTN;
    [SerializeField]
    private Image selectedChar;

    private Scenes selectedScene = Scenes.Stage_01;

    private void Awake()
    {
        startBTN.onClick.AddListener(GameStart);    
    }

    private void OnEnable()
    {
        DataRefresh();
    }

    void DataRefresh()
    {
        selectedChar.sprite = playerRegistry.GetPlayerByID(SaveManager.data.selectedChar).Icon;

    }

    void GameStart()
    {
        SceneChanger.instance.MoveScene(selectedScene);
    }
}
