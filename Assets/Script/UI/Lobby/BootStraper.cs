using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootStraper : MonoBehaviour
{
     readonly RateSetting rs = new RateSetting();

    private void Awake()
    {
        SaveManager.Load();

        SetScreen();

        SetSound();

    }

    void SetScreen()
    {
        Screen.SetResolution(
        rs.dropDownMap[SaveManager.saveData.rateIndex].x,
        rs.dropDownMap[SaveManager.saveData.rateIndex].y,
        SaveManager.saveData.fullScreen);
    }

    void SetSound()
    {
        SoundManager soundManager = FindFirstObjectByType<SoundManager>();

        if (soundManager == null)
        {
            CPrint.Error("SoundManager 못찾음");
            return;
        }



        //  SoundManager 값 설정이 안됨 나중에 협력 필요
    }
}
