using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        public ChoiceType type;
    }

    private struct PreChoice
    {
        public string id;
        public ChoiceType type;
    }

    enum ChoiceType
    {
        Artifact,
        Skill,
        Gold,
        BigGold,
    }

    [SerializeField]
    private SkillRegistry skillRegistry;
    [SerializeField]
    private ArtifactRegistry artifactRegistry;

    [SerializeField]
    private SkillSlot slot;

    [SerializeField]
    private GameObject lotteryPlate;
    [SerializeField]
    private Transform basePos;
    [SerializeField]
    private Transform movePos;
    [SerializeField]
    private Transform selected;
    [SerializeField]
    private List<float> rolltime;
    [SerializeField]
    private List<Choice> choices;

    [SerializeField]
    private GameObject resultPlate;
    [SerializeField]
    private Image resultIMG;
    [SerializeField]
    private TextMeshProUGUI resultDesc;
    [SerializeField]
    private TextMeshProUGUI goldText;

    [SerializeField]
    private Sprite goldIMG;
    [SerializeField]
    private Sprite bigGoldIMG;


    private List<PreChoice> choiceList = new List<PreChoice>();
    private int selectedChoice = 0;

    private int nowGold;
    private int amount;

    private Coroutine coroutine;

    private void Awake()
    {
        resultPlate.SetActive(false);
        lotteryPlate.transform.position = basePos.position;
    }

    public void StartLottery(Action _callback, int _nowGold, int _amount)
    {
        callback = _callback;

        amount = _amount;
        nowGold = _nowGold;

        LotteryListUp();

        this.gameObject.SetActive(true);

        Rolling();
    }

    void LotteryListUp()
    {
        choiceList.Clear();

        for (int i = 0; i < slot.ArtifactNum; i++)
        {
            slot.ArtifactLevel(i, out string id, out int level);

            if (level < artifactRegistry.GetArtifactByID(id).MaxLevel)
            {
                PreChoice pc = new PreChoice();

                pc.id = id;
                pc.type = ChoiceType.Artifact;

                choiceList.Add(pc);
            }
        }

        for (int i = 0; i < slot.SkillNum; i++)
        {
            slot.SkillLevel(i, out string id, out int level);

            if (level < skillRegistry.GetSkillByID(id).MaxLevel)
            {
                PreChoice pc = new PreChoice();

                pc.id = id;
                pc.type = ChoiceType.Skill;

                choiceList.Add(pc);
            }
        }

        {
            PreChoice pc = new PreChoice();
            pc.id = "";
            pc.type = pc.type = ChoiceType.Gold;

            choiceList.Add(pc);
            choiceList.Add(pc);
            choiceList.Add(pc);
        }

        {
            PreChoice pc = new PreChoice();
            pc.id = "";
            pc.type = pc.type = ChoiceType.BigGold;

            choiceList.Add(pc);
        }

        for (int i = 0; i < choices.Count; i++)
        {
            int pick = UnityEngine.Random.Range(0, choiceList.Count);

            choices[i].id = choiceList[pick].id;

            switch (choiceList[pick].type)
            {
                case ChoiceType.Skill:
                    choices[i].type = ChoiceType.Skill;
                    choices[i].choice_Icon.sprite = skillRegistry.GetSkillByID(choiceList[pick].id).IMG;
                    break;

                case ChoiceType.Artifact:
                    choices[i].type = ChoiceType.Artifact;
                    choices[i].choice_Icon.sprite = artifactRegistry.GetArtifactByID(choiceList[pick].id).IMG;
                    break;

                case ChoiceType.Gold:
                    choices[i].type = ChoiceType.Gold;
                    choices[i].choice_Icon.sprite = goldIMG;
                    break;

                case ChoiceType.BigGold:
                    choices[i].type = ChoiceType.BigGold;
                    choices[i].choice_Icon.sprite = bigGoldIMG;
                    break;
            }
        }
    }

    void Rolling()
    {
        int simonSays = UnityEngine.Random.Range(0, choices.Count);

        selectedChoice = simonSays;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(RollingCoroutine());
    }

    IEnumerator RollingCoroutine()
    {
        for (int i = 0; i < rolltime.Count; i++)
        {
            yield return SelectedRoll(rolltime[i]);
        }

        WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(rolltime[rolltime.Count - 1]);

        for (int i = 0; i < choices.Count; i++)
        {
            selected.position = choices[i].choice_Icon.transform.position;

            if (i == selectedChoice)
            {
                yield return new WaitForSecondsRealtime(1f);

                break;
            }

            yield return waitTime;
        }

        yield return ShowResultCoroutine();

        yield break;
    }

    IEnumerator SelectedRoll(float t)
    {
        WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(t);
        
        for (int i = 0; i < choices.Count; i++)
        {
            selected.position = choices[i].choice_Icon.transform.position;
            yield return waitTime;
        }

        yield break;
    }

    IEnumerator ShowResultCoroutine()
    {
        resultIMG.sprite = choices[selectedChoice].choice_Icon.sprite;

        switch (choices[selectedChoice].type)
        {
            case ChoiceType.Skill:
                resultDesc.text = skillRegistry.GetSkillByID(choices[selectedChoice].id).NameKR;

                slot.SkillUp(choices[selectedChoice].id);

                break;

            case ChoiceType.Artifact:
                resultDesc.text = artifactRegistry.GetArtifactByID(choices[selectedChoice].id).NameKR;

                slot.ArtifactUp(choices[selectedChoice].id);

                break;

            case ChoiceType.Gold:
                resultDesc.text = "돈! 돈! 돈!";

                ADDGold();

                break;

            case ChoiceType.BigGold:
                resultDesc.text = "더 많은 돈!";

                ADDGold();
                ADDGold();
                ADDGold();

                break;
        }

        goldText.text = nowGold.ToString();

        float realtime = 0;
        float slidetime = 2;

        while (realtime < slidetime)
        { 
            realtime += Time.unscaledDeltaTime;


            lotteryPlate.transform.position = Vector3.Lerp(basePos.position, movePos.position, realtime / slidetime);

            yield return null;
        }

        resultPlate.SetActive(true);

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        CleanUp();

        yield break;
    }

    void ADDGold()
    {
        ItemManager.instance.ShowMeTheMoney();
        nowGold += amount;
    }

    void CleanUp()
    {
        resultPlate.SetActive(false);
        lotteryPlate.transform.position = basePos.position;
        selected.position = choices[0].choice_Icon.transform.position;

        EndLottery();
    }

    public void EndLottery()
    {
        coroutine = null;

        this.gameObject.SetActive(false);
        
        callback?.Invoke();
    }
}
