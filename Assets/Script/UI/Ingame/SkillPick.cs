using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPick : MonoBehaviour
{
    private Action callback;

    public void StartPick(Action _callback)
    {
        callback = _callback;

        Settings();

        this.gameObject.SetActive(true);
    }

    void Settings()
    {

    }

    void EndPick()
    {
        this.gameObject.SetActive(false);
        callback?.Invoke();
    }
}
