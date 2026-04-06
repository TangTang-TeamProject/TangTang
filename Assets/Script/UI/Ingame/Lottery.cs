using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : MonoBehaviour
{
    private Action callback;

    public void StartLottery(Action _callback)
    {
        callback = _callback;
        Settings();

        this.gameObject.SetActive(true);
    }

    void Settings()
    {
    
    }

    public void EndLottery()
    {
        this.gameObject.SetActive(false);
        callback?.Invoke();
    }
}
