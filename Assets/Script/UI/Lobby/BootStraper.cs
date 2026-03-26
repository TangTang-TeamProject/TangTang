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
    }

    private void Start()
    {
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
        SoundManager.Instance.MasterVolumeChange(SaveManager.saveData.masterVolume);
        SoundManager.Instance.BGMVolumeChange(SaveManager.saveData.bgmVolume);
        SoundManager.Instance.SfxVolumeChange(SaveManager.saveData.sfxVolume);
    }
}
