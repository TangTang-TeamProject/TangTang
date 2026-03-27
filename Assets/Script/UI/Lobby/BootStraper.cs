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
        rs.dropDownMap[SaveManager.data.rateIndex].x,
        rs.dropDownMap[SaveManager.data.rateIndex].y,
        SaveManager.data.fullScreen);
    }

    void SetSound()
    {
        SoundManager.Instance.MasterVolumeChange(SaveManager.data.masterVolume);
        SoundManager.Instance.BGMVolumeChange(SaveManager.data.bgmVolume);
        SoundManager.Instance.SfxVolumeChange(SaveManager.data.sfxVolume);
    }
}
