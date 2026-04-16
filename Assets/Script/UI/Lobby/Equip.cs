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

        descText.text = $"아이템을 강화하세요!";

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
    }

    void UpGrade()
    {
        if (string.IsNullOrEmpty(upgradeID))
        {
            descText.text = "장비를 선택해주세요!";
        }
        else
        {
            int g = SaveManager.data.gold;
            int lev = SaveManager.GetEquipLevel(upgradeID);
            int maxLev = equipRegistry.GetEquipByID(upgradeID).MaxLevel;

            if (g < 50)
            {
                descText.text = "잔액 부족!";
            }
            else if (lev >= maxLev)
            {
                descText.text = "최고 레벨!";
            }
            else
            {
                descText.text = "강화 성공!";
                SaveManager.CalcGold(-50);
                lev++;
                SaveManager.SetEquipLevel(upgradeID, lev);
                SaveManager.Save();
            }
        }

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(TextChangeCoroutine());
    }

    IEnumerator TextChangeCoroutine()
    {
        yield return new WaitForSeconds(2f);

        descText.text = $"";

        yield break;
    }
}
