using System.Collections;
using System.Collections.Generic;
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
    private Image weapon;
    [SerializeField]
    private Image head;
    [SerializeField]
    private Image body;
    [SerializeField]
    private Image legs;
    [SerializeField]
    private Image cape;
    [SerializeField]
    private Image player;
    [SerializeField]
    private EquipRegistry equipRegistry;
    [SerializeField]
    private PlayerRegistry playerRegistry;

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
    
    private void Awake()
    {
        allSort.onClick.AddListener(() => DecideType(allSortimg, SortType.ALL));
        headSort.onClick.AddListener(() => DecideType(headSortimg, SortType.Head));
        bodySort.onClick.AddListener(() => DecideType(bodySortimg, SortType.Body));
        legsSort.onClick.AddListener(() => DecideType(legsSortimg, SortType.Legs));
        capeSort.onClick.AddListener(() => DecideType(capeSortimg, SortType.Cape));
        updownSort.onClick.AddListener(() => DecideWay(updownSortimg));
    }

    private void OnEnable()
    {
        player.sprite = playerRegistry.GetPlayerByID(SaveManager.data.selectedChar).Icon;

        DataRefresh();
        DecideType(allSortimg, SortType.ALL);
        DecideWay(updownSortimg);
    }

    void DataRefresh()
    {
        ChangeImg(head, EquipType.Head);
        ChangeImg(body, EquipType.Body);
        ChangeImg(legs, EquipType.Leg);
        ChangeImg(cape, EquipType.Cape);
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

            btn.onClick.AddListener(() => ChangeWearEquip(_t, _id));

            equips.Add(g);
        }
    }

    void AllSort()
    {
        List<EquipData_SO> e = new List<EquipData_SO>(equipRegistry.Equips);

        for (int i = 0; i < e.Count; i++)
        {
            GameObject g = Instantiate(equipPrefab);
            Button btn = g.GetComponentInChildren<Button>();

            btn.image.sprite = e[i].IMG;

            string _id = e[i].EquipID;
            EquipType _t = e[i].Type;

            btn.onClick.AddListener(() => ChangeWearEquip(_t, _id));

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

    void ChangeWearEquip(EquipType _t, string _id)
    {
        SaveManager.SetEquip(_t, _id);
        SaveManager.Save();

        DataRefresh();
    }


    void ChangeImg(Image img, EquipType type)
    {
        string id = SaveManager.GetEquip(type);

        img.sprite = equipRegistry.GetEquipByID(id).IMG;
    }
}
