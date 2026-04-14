using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour
{
    [SerializeField]
    private PlayerRegistry playerRegistry;
    [SerializeField]
    private EquipRegistry equipRegistry;
    [SerializeField]
    private SkillRegistry skillRegistry;

    [SerializeField]
    private Button startBTN;
    [SerializeField]
    private Button leftBTN;
    [SerializeField]
    private Button rightBTN;

    [SerializeField]
    private Image selectedChar;
    [SerializeField]
    private TextMeshProUGUI charName;
    [SerializeField]
    private List<Image> charEquip;
    [SerializeField]
    private Image charWeapon;

    [SerializeField]
    private List<Sprite> stageIMG;
    [SerializeField]
    private Image selectedStageIMG;


    private Scenes selectedScene = Scenes.STG_001;


    private void Awake()
    {
        startBTN.onClick.AddListener(GameStart);    
        leftBTN.onClick.AddListener(() => MoveSelect(-1));
        rightBTN.onClick.AddListener(() => MoveSelect(1));
    }

    private void OnEnable()
    {
        DataRefresh();
    }

    void DataRefresh()
    {
        PlayerData_SO p = playerRegistry.GetPlayerByID(SaveManager.data.selectedChar);

        selectedChar.sprite = p.Icon;
        charName.text = p.NameKR;


        string[] s = SaveManager.data.equipID;

        charEquip[0].sprite = equipRegistry.GetEquipByID(s[0]).IMG;
        charEquip[1].sprite = equipRegistry.GetEquipByID(s[1]).IMG;
        charEquip[2].sprite = equipRegistry.GetEquipByID(s[2]).IMG;
        charEquip[3].sprite = equipRegistry.GetEquipByID(s[3]).IMG;


        charWeapon.sprite = skillRegistry.GetSkillByID(playerRegistry.GetPlayerByID(SaveManager.data.selectedChar).Weapon).IMG;

        selectedStageIMG.sprite = stageIMG[(int)selectedScene];
    }

    void MoveSelect(int a)
    {
        int nextScene = (int)selectedScene + a;

        if (nextScene >= (int)Scenes.SceneCount)
        {
            nextScene = (int)Scenes.STG_001;
        }
        else if(nextScene < (int)Scenes.STG_001)
        {
            nextScene = (int)Scenes.SceneCount - 1;
        }

        selectedScene = (Scenes)nextScene;

        selectedStageIMG.sprite = stageIMG[nextScene];
    }

    void GameStart()
    {
        SceneChanger.instance.MoveScene(selectedScene);
    }
}
