using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Equip : MonoBehaviour
{
    enum SortType
    {
        ALL,
        Head,
        Body,
        Legs,
        Cape,
    }

    enum SortWay
    { 
        Up,
        Down,
    }

    enum TextType
    {
        Normal,
        NoMoney,
        MaxLevel,
        Success,
        Require,
        SelectPLZ
    }

    [SerializeField]
    private EquipDropSlot head;
    [SerializeField]
    private EquipDropSlot body;
    [SerializeField]
    private EquipDropSlot legs;
    [SerializeField]
    private EquipDropSlot cape;
    [SerializeField]
    private Image player;
    [SerializeField]
    private EquipRegistry equipRegistry;
    [SerializeField]
    private EquipLevelRegistry equipLevelRegistry;    
    [SerializeField]
    private PlayerRegistry playerRegistry;
    [SerializeField]
    private SkillRegistry skillRegistry;

    [SerializeField]
    private List<GameObject> equips;
    [SerializeField]
    private GameObject equipPrefab;
    [SerializeField]
    private GridLayoutGroup equipBox;

    [SerializeField]
    private Button allSort;
    [SerializeField]
    private Button headSort;
    [SerializeField]
    private Button bodySort;
    [SerializeField]
    private Button legsSort;
    [SerializeField]
    private Button capeSort;
    [SerializeField]
    private Button updownSort;

    [SerializeField]
    private Image allSortimg;
    [SerializeField]
    private Image headSortimg;
    [SerializeField]
    private Image bodySortimg;
    [SerializeField]
    private Image legsSortimg;
    [SerializeField]
    private Image capeSortimg;
    [SerializeField]
    private Image updownSortimg;
    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private Sprite upSprite;
    [SerializeField]
    private Sprite downSprite;

    private SortType sortType = SortType.ALL;
    private SortWay sortWay = SortWay.Down;



    [SerializeField]
    private EquipUpGradeDropSlot upGradeEquip;
    [SerializeField]
    private Sprite noUpGradeEquip;
    [SerializeField]
    private Button upGradeBTN;
    [SerializeField]
    private TextMeshProUGUI descText;
    [SerializeField]
    private TextMeshProUGUI goldText;


    private string upgradeID;
    private string grabbedID;

    private Coroutine coroutine;
    
    private void Awake()
    {
        allSort.onClick.AddListener(() => DecideType(allSortimg, SortType.ALL));
        headSort.onClick.AddListener(() => DecideType(headSortimg, SortType.Head));
        bodySort.onClick.AddListener(() => DecideType(bodySortimg, SortType.Body));
        legsSort.onClick.AddListener(() => DecideType(legsSortimg, SortType.Legs));
        capeSort.onClick.AddListener(() => DecideType(capeSortimg, SortType.Cape));
        updownSort.onClick.AddListener(() => DecideWay(updownSortimg));

        head.SetDropWear(ChangeWearEquip);
        body.SetDropWear(ChangeWearEquip);
        legs.SetDropWear(ChangeWearEquip);
        cape.SetDropWear(ChangeWearEquip);

        upGradeEquip.SetDropUpGrade(DropUpgrade);

        upGradeBTN.onClick.AddListener(UpGrade);
    }

    private void OnEnable()
    {
        player.sprite = playerRegistry.GetPlayerByID(SaveManager.data.selectedChar).Icon;

        WearingRefresh();
        UpGradeRefresh();
        DecideType(allSortimg, SortType.ALL);
        DecideWay(updownSortimg);
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    void WearingRefresh()
    {
        head.ChangeIMG(ReturnImg(EquipType.Head));
        body.ChangeIMG(ReturnImg(EquipType.Body));
        legs.ChangeIMG(ReturnImg(EquipType.Leg));
        cape.ChangeIMG(ReturnImg(EquipType.Cape));
    }

    Sprite ReturnImg(EquipType type)
    {
        string id = SaveManager.GetEquip(type);

        return equipRegistry.GetEquipByID(id).IMG;
    }

    void UpGradeRefresh()
    {
        goldText.text = (SaveManager.data.gold).ToString();

        TextChange(0, TextType.Normal);

        upGradeEquip.ChangeIMG(noUpGradeEquip);

        upgradeID = null;
    }

    void ButtonRefresh()
    {
        allSortimg.sprite = normalSprite;
        headSortimg.sprite = normalSprite;
        bodySortimg.sprite = normalSprite;
        legsSortimg.sprite = normalSprite;
        capeSortimg.sprite = normalSprite;
    }

    void DecideType(Image _img, SortType _type)
    {
        ButtonRefresh();

        _img.sprite = selectedSprite;

        sortType = _type;

        Sorting();
    }

    void DecideWay(Image _img)
    {
        switch (sortWay)
        {
            case SortWay.Up:
                sortWay = SortWay.Down;
                _img.sprite = downSprite;
                break;
            case SortWay.Down:
                sortWay = SortWay.Up;
                _img.sprite = upSprite;
                break;
        }

        Sorting();
    }

    void Sorting()
    {
        Clean();

        switch (sortType)
        {
            case SortType.ALL:
                AllSort();
                break;
            case SortType.Cape:
                TypeSort(EquipType.Cape);
                break;
            case SortType.Body:
                TypeSort(EquipType.Body);
                break;
            case SortType.Head:
                TypeSort(EquipType.Head);
                break;
            case SortType.Legs:
                TypeSort(EquipType.Leg);
                break;
        }

        WaySort();
    }

    void TypeSort(EquipType _t)
    {
        List<EquipData_SO> e = new List<EquipData_SO>(equipRegistry.GetEquipByType(_t));

        for (int i = 0; i < e.Count; i++)
        {
            GameObject g = Instantiate(equipPrefab);
            Button btn = g.GetComponentInChildren<Button>();

            btn.image.sprite = e[i].IMG;

            string _id = e[i].EquipID;

            equips.Add(g);
        }
    }

    void AllSort()
    {
        List<EquipData_SO> e = new List<EquipData_SO>(equipRegistry.Equips);

        for (int i = 0; i < e.Count; i++)
        {
            string _id = e[i].EquipID;
            EquipType _t = e[i].Type;

            if (e[i].Type == EquipType.TypeCount)
            {
                continue;
            }

            GameObject g = Instantiate(equipPrefab);
            Button btn = g.GetComponentInChildren<Button>();
            IEquipSlot _slot = g.GetComponent<IEquipSlot>();

            _slot.SetID(_id, GrabEquip);

            btn.image.sprite = e[i].IMG;

            equips.Add(g);
        }
    }

    void WaySort()
    {
        switch (sortWay)
        {
            case SortWay.Up:
                for (int i = 0; i < equips.Count; i++)
                {
                    equips[i].transform.SetParent(equipBox.transform);
                }
                break;
            case SortWay.Down:
                for (int i = equips.Count - 1; i >= 0; i--)
                {
                    equips[i].transform.SetParent(equipBox.transform);
                }
                break;
        }
    }

    void Clean()
    {
        for (int i = 0; i < equips.Count; i++)
        {
            Destroy(equips[i]);
        }

        equips.Clear();
    }

    void ChangeWearEquip(EquipType _type)
    {
        EquipType grabType = equipRegistry.GetEquipByID(grabbedID).Type;

        if(_type != grabType)
        {
            return;
        }

        SaveManager.SetEquip(grabType, grabbedID);
        SaveManager.Save();

        WearingRefresh();
    }

    void GrabEquip(string id)
    {
        grabbedID = id;
    }

    void DropUpgrade()
    {
        upgradeID = grabbedID;

        upGradeEquip.ChangeIMG(equipRegistry.GetEquipByID(upgradeID).IMG);

        int lev = SaveManager.GetEquipLevel(upgradeID);
        int maxLev = equipRegistry.GetEquipByID(upgradeID).MaxLevel;
        int reqGold = equipLevelRegistry.GetEquipsDataByIDLevel(upgradeID, lev).UpGradeRequire;

        if (lev >= maxLev)
        {
            TextChange(reqGold, TextType.MaxLevel);
        }
        else
        {
            TextChange(reqGold, TextType.Require);
        }

    }

    void UpGrade()
    {
        if (string.IsNullOrEmpty(upgradeID))
        {
            TextChange(0, TextType.SelectPLZ);
        }
        else
        {
            int g = SaveManager.data.gold;
            int lev = SaveManager.GetEquipLevel(upgradeID);
            int maxLev = equipRegistry.GetEquipByID(upgradeID).MaxLevel;
            int reqGold = equipLevelRegistry.GetEquipsDataByIDLevel(upgradeID, lev).UpGradeRequire;


            if (lev >= maxLev)
            {
                TextChange(reqGold, TextType.MaxLevel);
                return;
            }
            else if (g < reqGold)
            {
                TextChange(reqGold, TextType.NoMoney);
            }
            else
            {
                TextChange(reqGold, TextType.Success);
                SoundManager.Instance.PlaySfx(ESfxType.ReinforceSuccess);
                SaveManager.CalcGold(-reqGold);
                lev++;
                SaveManager.SetEquipLevel(upgradeID, lev);
                goldText.text = (SaveManager.data.gold).ToString();
            }

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(TextChangeCoroutine(reqGold, lev >= maxLev));
        }
    }

    IEnumerator TextChangeCoroutine(int _reqGold, bool _isMax)
    {
        yield return new WaitForSeconds(2f);

        if (_isMax)
        {
            TextChange(_reqGold, TextType.MaxLevel);
        }
        else
        {
            TextChange(_reqGold, TextType.Require);
        }

        yield break;
    }

    void TextChange(int _reqGold, TextType tt)
    {
        switch(tt)
        {
            case TextType.Normal:
                descText.text = $"장비를\n강화하세요!";
                break;
            case TextType.NoMoney:
                descText.text = "잔액 부족!";
                break;
            case TextType.Require:
                descText.text = $"강화에 {_reqGold}골드가\n필요합니다.";
                break;
            case TextType.MaxLevel:
                descText.text = $"해당 장비는\n최고레벨입니다!";
                break;
            case TextType.Success:
                descText.text = "강화 성공!";
                break;
            case TextType.SelectPLZ:
                descText.text = "장비를\n올려주세요!";
                break;
        }
    }
}
