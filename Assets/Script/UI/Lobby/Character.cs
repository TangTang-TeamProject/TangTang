using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
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
    private GameObject charPrefab;
    [SerializeField]
    private GridLayoutGroup charBox;

    private string selectedChar;

    private void Awake()
    {
        selectBTN.onClick.AddListener(SelectChar);

        for(int i = 0; i < playerRegistry.Players.Count; i++)
        {
            GameObject g = Instantiate(charPrefab, charBox.transform);

            Button btn = g.GetComponentInChildren<Button>();

            PlayerData_SO p = playerRegistry.Players[i];

            btn.onClick.AddListener(() => ViewChar(p.CharacterID));

            btn.image.sprite = p.Icon;
        }
    }

    private void OnEnable()
    {
        selectedChar = SaveManager.data.selectedChar;
        ViewChar(selectedChar);
    }

    void ViewChar(string id)
    {
        PlayerData_SO p = playerRegistry.GetPlayerByID(id);

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
            selectedChar = id;
            selectBTN.gameObject.SetActive(true);
        }
    }


    void SelectChar()
    {
        SaveManager.SetChar(selectedChar);
        SaveManager.Save();
    }
}
