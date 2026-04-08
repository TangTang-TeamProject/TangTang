using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
        public string pickedID;
        public ChoiceType type;
    }

    private class PreChoice
    {
        public string pickedID;
        public ChoiceType type;
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

    private List<PreChoice> choiceList = new List<PreChoice>();
    private HashSet<string> artiList = new HashSet<string>();
    private HashSet<string> haveList = new HashSet<string>();

    private void Awake()
    {
        for (int i = 0; i < choices.Count; i++)
        {
            if (choices[i] == null)
            {
                CPrint.Error("ÀÎ½ºÆåÅÍ ÂüÁ¶ ¿À·ù - choices");
            }

            int index = i;

            choices[i].choice.onClick.AddListener(() => EndPick(index));
        }
    }

    public void StartPick(Action _callback)
    {
        callback = _callback;

        HoldListUp();

        Settings();

        this.gameObject.SetActive(true);
    }

    void HoldListUp()
    {
        choiceList.Clear();
        artiList.Clear();
        haveList.Clear();

        for(int i = 0; i < slot.ArtifactNum; i++)
        {
            slot.ArtifactLevel(i, out string id, out int level);

            ArtifactData_SO arti = artifactRegistry.GetArtifactByID(id);

            artiIMG[i].sprite = arti.IMG;

            if (level < arti.MaxLevel)
            {
                PreChoice pc = MakePreData(id, ChoiceType.Artifact);

                choiceList.Add(pc);
            }

            artiList.Add(id);
            haveList.Add(id);
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
            else
            {
                string require = evolutionRegistry.GetEvolutionRequire(id);
                string evo = evolutionRegistry.GetEvolution(id);

                if (artiList.Contains(require))
                {
                    PreChoice pc = MakePreData(evo, ChoiceType.Evo);

                    choiceList.Add(pc);
                }
            }

            haveList.Add(id);
        }

        int executeLimit = 0;

        while(choiceList.Count < 8 && executeLimit < 50)
        {
            executeLimit++;

            if (slot.IsArtifactFull() && slot.IsSkillFull())
            {
                break;
            }
            else if (slot.IsSkillFull())
            {
                ArtifactData_SO a = artifactRegistry.GetRandomArti();

                PreChoice pc = MakePreData(a.ArtifactID, ChoiceType.Artifact);

                if (!haveList.Contains(pc.pickedID))
                {
                    choiceList.Add(pc);
                    haveList.Add(pc.pickedID);
                }
            }
            else if (slot.IsArtifactFull())
            {
                SkillData_SO s = skillRegistry.GetRandomSkill();

                PreChoice pc = MakePreData(s.SkillID, ChoiceType.Skill);

                if (!haveList.Contains(pc.pickedID))
                {
                    choiceList.Add(pc);
                    haveList.Add(pc.pickedID);
                }
            }
            else
            {
                int c = UnityEngine.Random.Range(0, 2);

                if (c == 0)
                {
                    SkillData_SO s = skillRegistry.GetRandomSkill();

                    PreChoice pc = MakePreData(s.SkillID, ChoiceType.Skill);

                    if (!haveList.Contains(pc.pickedID))
                    {
                        choiceList.Add(pc);
                        haveList.Add(pc.pickedID);
                    }
                }
                else
                {
                    ArtifactData_SO a = artifactRegistry.GetRandomArti();

                    PreChoice pc = MakePreData(a.ArtifactID, ChoiceType.Artifact);

                    if (!haveList.Contains(pc.pickedID))
                    {
                        choiceList.Add(pc);
                        haveList.Add(pc.pickedID);
                    }
                }
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
                    break;

                case ChoiceType.Skill:
                    SkillData_SO s = skillRegistry.GetSkillByID(id);
                    choices[i].choice_icon.sprite = s.IMG;
                    choices[i].choice_back.sprite = skillPlate;
                    choices[i].choice_desc.text = s.NameKR;
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
                    choices[i].choice_desc.text = "µ· µ· µ·";
                    break;
            }
        }
    }

    void Shuffle()
    {
        int n = choiceList.Count;

        while(n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n);

            PreChoice pc = choiceList[k];
            choiceList[k] = choiceList[n];
            choiceList[n] = pc;
        }
    }

    PreChoice MakePreData(string _id, ChoiceType _t)
    {
        PreChoice pc = new PreChoice();
        pc.pickedID = _id;
        pc.type = _t;

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
            case ChoiceType.Evo:
                slot.SkillUp(choices[i].pickedID);
                break;

            case ChoiceType.Gold:
                ItemManager.instance.ShowMeTheMoney();
                break;
        }

        this.gameObject.SetActive(false);

        callback?.Invoke();
    }
}
