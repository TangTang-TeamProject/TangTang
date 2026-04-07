using System;
using System.Collections;
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
        public string pickedID;
    }


    [SerializeField]
    private List<Choice> choices;
    [SerializeField]
    private Sprite evoIMG;
    [SerializeField]
    private Sprite skillIMG;
    [SerializeField]
    private Sprite artiIMG;


    private void Awake()
    {
        for (int i = 0; i < choices.Count; i++)
        {
            if (choices[i] == null)
            {
                CPrint.Error("인스펙터 참조 오류 - choices");
            }

            choices[i].choice.onClick.AddListener(EndPick);
        }
    }

    public void StartPick(Action _callback)
    {
        callback = _callback;

        Settings();

        this.gameObject.SetActive(true);
    }

    void Settings()
    {

    }

    public void EndPick()
    {
        this.gameObject.SetActive(false);

        callback?.Invoke();
    }
}
