using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lottery : MonoBehaviour
{
    private Action callback;

    [System.Serializable]
    private class Choice
    {
        public string id;
        public Image choice_Icon;
    }

    [SerializeField]
    private SkillRegistry skillRegistry;

    [SerializeField]
    private SkillSlot slot;

    [SerializeField]
    private Transform lotteryPlate;
    [SerializeField]
    private List<Choice> choices;

    [SerializeField]
    private Transform resultPlate;
    [SerializeField]
    private Image resultIMG;
    [SerializeField]
    private TextMeshProUGUI resultDesc;
    [SerializeField]
    private TextMeshProUGUI goldText;


    private Coroutine coroutine;

    public void StartLottery(Action _callback)
    {
        callback = _callback;

        LotteryListUp();

        this.gameObject.SetActive(true);

        Rolling();
    }

    void LotteryListUp()
    { 
    
    }

    void Rolling()
    {



        if (coroutine != null)
        {
            return;
        }

        coroutine = StartCoroutine(RollingCoroutine());
    }

    IEnumerator RollingCoroutine()
    {
        ShowResult();

        yield return ShowResultCoroutine();

        yield break;
    }

    void ShowResult()
    { 
    
    }

    IEnumerator ShowResultCoroutine()
    {



        while (!Input.anyKey)
        {
            yield return null;
        }

        EndLottery();

        yield break;
    }

    public void EndLottery()
    {
        coroutine = null;

        this.gameObject.SetActive(false);
        
        callback?.Invoke();
    }
}
