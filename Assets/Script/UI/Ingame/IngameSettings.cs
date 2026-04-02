using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameSettings : Settings
{
    [SerializeField]
    private Button GiveUpBTN;

    protected override void Awake()
    {
        base.Awake();

        GiveUpBTN.onClick.AddListener(GiveUpGame);
    }

    void GiveUpGame()
    {
        SceneChanger.instance.MoveScene(Scenes.Lobby);
    }

    public void ActiveUI(bool _active)
    {
        this.gameObject.SetActive(_active);
    }
}
