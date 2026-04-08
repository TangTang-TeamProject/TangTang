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
        Cape
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
    private EquipRegistry equip;

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

    private void Start()
    {
        DataRefresh();
        DecideType(allSortimg, SortType.ALL);
    }

    void DataRefresh()
    {
        ChangeImg(weapon, EquipType.Weapon);
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
        if (sortWay == SortWay.Up)
        {
            _img.sprite = downSprite;
            sortWay = SortWay.Down;
        }
        else
        {
            _img.sprite = upSprite;
            sortWay = SortWay.Up;
        }

        Sorting();
    }

    void Sorting()
    {
        
    }


    void ChangeImg(Image img, EquipType type)
    {
        int id = SaveManager.GetEquip(type);

        img.sprite = equip.GetEquipByID(id).IMG;
    }
}
