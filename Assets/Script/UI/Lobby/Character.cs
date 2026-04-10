using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private class CharBlock
    {
        public string id;
        public Image img;
        public Button menuBTN;
    }

    [SerializeField]
    private PlayerRegistry playerRegistry;

    [SerializeField]
    private TextMeshProUGUI charName;
    [SerializeField]
    private TextMeshProUGUI charDesc;
    [SerializeField]
    private Image charIMG;
    [SerializeField]
    private GameObject isClosed;
    [SerializeField]
    private Button selectBTN;
    [SerializeField]
    private List<CharBlock> menuBTN;

    private string selectedChar;

    private void Awake()
    {
        selectBTN.onClick.AddListener(SelectChar);
    }

    private void OnEnable()
    {
        selectedChar = SaveManager.data.selectedChar;
        //ViewChar();
    }

    void ViewChar()
    {
        PlayerData_SO p = playerRegistry.GetPlayerByID(selectedChar);

        charName.text = p.NameKR;

        if (p.IsClosed)
        {
            isClosed.SetActive(true);
            charDesc.text = "해당 캐릭터 해금 필요";
            charDesc.color = Color.red;
            charIMG.sprite = p.Icon;
            selectBTN.gameObject.SetActive(false);
        }
        else
        {
            isClosed.SetActive(false);
            charDesc.text = p.Desc;
            charDesc.color = Color.white;
            charIMG.sprite = p.Icon;
            selectBTN.gameObject.SetActive(true);
        }
    }


    void SelectChar()
    {
        SaveManager.data.selectedChar = "CHR_001";
        SaveManager.Save();
    }
}
