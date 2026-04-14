using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPick : MonoBehaviour
{
    private Action callback;

    [System.Serializable]
    private class Choice
    {
        public Button choice;
        public Image choice_back;
        public Image choice_icon;
        public TextMeshProUGUI choice_desc;
        public GameObject starPos;
        public string pickedID;
        public ChoiceType type;
    }

    private struct PreChoice
    {
        public string pickedID;
        public ChoiceType type;
        public int lev;
    }

    enum ChoiceType
    { 
        Artifact,
        Skill,
        Evo,
        Gold,
    }


    [SerializeField]
    private List<Image> skillIMG;
    [SerializeField]
    private List<Image> artiIMG;


    [SerializeField]
    private SkillRegistry skillRegistry;
    [SerializeField]
    private ArtifactRegistry artifactRegistry;
    [SerializeField]
    private EvolutionRegistry evolutionRegistry;
    [SerializeField]
    private SkillSlot slot;


    [SerializeField]
    private List<Choice> choices;
    [SerializeField]
    private Sprite evoPlate;
    [SerializeField]
    private Sprite skillPlate;
    [SerializeField]
    private Sprite artiPlate;
    [SerializeField]
    private Sprite goldIcon;
    [SerializeField]
    private GameObject aliveStar;
    [SerializeField]
    private GameObject deadStar;


    private List<GameObject> starList = new List<GameObject>();



    private List<PreChoice> choiceList = new List<PreChoice>();

    private HashSet<string> artiHistory = new HashSet<string>();
    private HashSet<string> skillHistory = new HashSet<string>();

    private List<string> allSkill = new List<string>();
    private List<string> allArti = new List<string>();

    public void ManualAwake()
    {
        for (int i = 0; i < choices.Count; i++)
        {
            if (choices[i] == null)
            {
                CPrint.Error("인스펙터 참조 오류 - choices");
            }

            int index = i;

            choices[i].choice.onClick.AddListener(() => EndPick(index));
        }

        for (int i = 0; i < skillRegistry.Skills.Count; i++)
        {
            if (!skillRegistry.Skills[i].IsEvo)
            {
                allSkill.Add(skillRegistry.Skills[i].SkillID);
            }
        }

        for (int i = 0; i < artifactRegistry.Artifacts.Count; i++)
        {
            allArti.Add(artifactRegistry.Artifacts[i].ArtifactID);
        }
    }

    public void StartPick(Action _callback)
    {
        callback = _callback;

        Refresh();

        HoldListUp();

        UnHoldListUp();

        Settings();

        this.gameObject.SetActive(true);
    }

    void Refresh()
    {
        choiceList.Clear();

        for (int i = 0; i < starList.Count; i++)
        {
            Destroy(starList[i]);
        }

        starList.Clear();
    }

    void HoldListUp()
    {
        for(int i = 0; i < slot.ArtifactNum; i++)
        {
            slot.ArtifactLevel(i, out string id, out int level);

            ArtifactData_SO arti = artifactRegistry.GetArtifactByID(id);

            artiIMG[i].sprite = arti.IMG;

            if (level < arti.MaxLevel)
            {
                PreChoice pc = MakePreData(id, ChoiceType.Artifact, level);

                choiceList.Add(pc);
            }

            artiHistory.Add(id);
        }

        for (int i = 0; i < slot.SkillNum; i++)
        {
            slot.SkillLevel(i, out string id, out int level);

            SkillData_SO skill = skillRegistry.GetSkillByID(id);

            skillIMG[i].sprite = skill.IMG;

            if (level < skill.MaxLevel)
            {
                PreChoice pc = MakePreData(id, ChoiceType.Skill);

                choiceList.Add(pc);
            }
            else if(!skill.IsEvo)
            {
                string require = evolutionRegistry.GetEvolutionRequire(id);
                string evo = evolutionRegistry.GetEvolution(id);

                if (artiHistory.Contains(require))
                {
                    PreChoice pc = MakePreData(evo, ChoiceType.Evo, level);

                    choiceList.Add(pc);
                }
            }

            skillHistory.Add(id);
        }
    }

    void UnHoldListUp()
    {

        List<string> notHaveSkill = new List<string>();
        List<string> notHaveArti = new List<string>();

        for (int i = 0; i < allSkill.Count; i++)
        {
            if (!skillHistory.Contains(allSkill[i]))
            {
                notHaveSkill.Add(allSkill[i]);
            }
        }

        for (int i = 0; i < allArti.Count; i++)
        {
            if (!artiHistory.Contains(allArti[i]))
            {
                notHaveArti.Add(allArti[i]);
            }
        }

        bool flag = false;

        int safety = 10;

        while (choiceList.Count < 8 && safety > 0)
        {
            safety--;

            if (slot.IsSkillFull() && slot.IsArtifactFull())
            {
                break;
            }
            else if (slot.IsArtifactFull() && notHaveSkill.Count > 0)
            {
                int pick = UnityEngine.Random.Range(0, notHaveSkill.Count);

                PreChoice pc = MakePreData(notHaveSkill[pick], ChoiceType.Skill);

                notHaveSkill.RemoveAt(pick);

                choiceList.Add(pc);
            }
            else if (slot.IsSkillFull() && notHaveArti.Count > 0)
            {
                int pick = UnityEngine.Random.Range(0, notHaveArti.Count);

                PreChoice pc = MakePreData(notHaveArti[pick], ChoiceType.Artifact);

                notHaveArti.RemoveAt(pick);

                choiceList.Add(pc);
            }
            else if (flag && notHaveSkill.Count > 0)
            {
                flag = false;

                int pick = UnityEngine.Random.Range(0, notHaveSkill.Count);

                PreChoice pc = MakePreData(notHaveSkill[pick], ChoiceType.Skill);

                notHaveSkill.RemoveAt(pick);

                choiceList.Add(pc);
            }
            else if (notHaveArti.Count > 0)
            {
                flag = true;

                int pick = UnityEngine.Random.Range(0, notHaveArti.Count);

                PreChoice pc = MakePreData(notHaveArti[pick], ChoiceType.Artifact);

                notHaveArti.RemoveAt(pick);

                choiceList.Add(pc);
            }
            else
            {
                CPrint.Warn("헛돌았음");
            }
        }
    }

    void Settings()
    {
        while(choiceList.Count < choices.Count)
        {
            PreChoice pc = MakePreData("", ChoiceType.Gold);

            choiceList.Add(pc);
        }

        Shuffle();

        for (int i = 0; i < choices.Count; i++)
        {
            string id = choiceList[i].pickedID;

            choices[i].type = choiceList[i].type;
            choices[i].pickedID = id;

            switch (choiceList[i].type)
            {
                case ChoiceType.Artifact:
                    ArtifactData_SO a = artifactRegistry.GetArtifactByID(id);
                    choices[i].choice_icon.sprite = a.IMG;
                    choices[i].choice_back.sprite = artiPlate;
                    choices[i].choice_desc.text = a.NameKR;
                    MakeStar(i, choiceList[i].lev, a.MaxLevel);
                    break;

                case ChoiceType.Skill:
                    SkillData_SO s = skillRegistry.GetSkillByID(id);
                    choices[i].choice_icon.sprite = s.IMG;
                    choices[i].choice_back.sprite = skillPlate;
                    choices[i].choice_desc.text = s.NameKR;
                    MakeStar(i, choiceList[i].lev, s.MaxLevel);
                    break;

                case ChoiceType.Evo:
                    SkillData_SO evo = skillRegistry.GetSkillByID(id);
                    choices[i].choice_icon.sprite = evo.IMG;
                    choices[i].choice_back.sprite = evoPlate;
                    choices[i].choice_desc.text = evo.NameKR;
                    break;

                case ChoiceType.Gold:
                    choices[i].choice_icon.sprite = goldIcon;
                    choices[i].choice_back.sprite = evoPlate;
                    choices[i].choice_desc.text = "돈 돈 돈";
                    break;
            }
        }
    }

    void MakeStar(int choiceAddr, int lev, int maxLev)
    {
        Transform t = choices[choiceAddr].starPos.transform;

        for (int i = 0; i < lev; i++)
        {
            GameObject g = Instantiate(aliveStar, t);

            starList.Add(g);
        }

        for (int i = 0; i < maxLev - lev; i++)
        {
            GameObject g = Instantiate(deadStar, t);

            starList.Add(g);
        }
    }

    void Shuffle()
    {
        int n = choiceList.Count;

        while(n > 1)
        {
            n--;

            int k = UnityEngine.Random.Range(0, n + 1);

            PreChoice pc = choiceList[k];
            choiceList[k] = choiceList[n];
            choiceList[n] = pc;
        }
    }

    PreChoice MakePreData(string _id, ChoiceType _t, int _lev = 0)
    {
        PreChoice pc = new PreChoice();
        pc.pickedID = _id;
        pc.type = _t;
        pc.lev = _lev + 1;

        return pc;
    }

    public void EndPick(int i)
    {
        switch(choices[i].type)
        {
            case ChoiceType.Artifact:
                slot.ArtifactUp(choices[i].pickedID);
                break;

            case ChoiceType.Skill:
                slot.SkillUp(choices[i].pickedID);
                break;

            case ChoiceType.Evo:
                slot.SkillEvo(evolutionRegistry.GetBase(choices[i].pickedID) , choices[i].pickedID);
                break;

            case ChoiceType.Gold:
                ItemManager.instance.ShowMeTheMoney();
                break;
        }

        this.gameObject.SetActive(false);

        callback?.Invoke();
    }
}
