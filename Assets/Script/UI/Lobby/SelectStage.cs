using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour
{
    [SerializeField]
    private Button startBTN;

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


    }

    void GameStart()
    {
        SceneChanger.instance.MoveScene(selectedScene);
    }
}
