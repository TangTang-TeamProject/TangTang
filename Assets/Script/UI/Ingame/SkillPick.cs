using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPick : MonoBehaviour
{
    private Action callback;

    [SerializeField]
    private Transform pos_1;
    [SerializeField]
    private Transform pos_2;
    [SerializeField]
    private Transform pos_3;

    private string pickedID;

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
