using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private Slider masterVolume;
    [SerializeField]
    private Slider bgmVolume;
    [SerializeField]
    private Slider sfxVolume;
    [SerializeField]
    private TMP_Dropdown windowRate;
    [SerializeField]
    private Toggle fullScreen;

    readonly RateSetting rs = new RateSetting();

    private void Awake()
    {
        soundManager = FindFirstObjectByType<SoundManager>();

        if (soundManager == null)
        {
            CPrint.Error("SoundManager ¸øÃ£À½");
        }



        List<string> options = new List<string>();

        foreach (Vector2 rate in rs.dropDownMap)
        {
            options.Add($"{rate.x} : {rate.y}");
        }

        windowRate.AddOptions(options);




        windowRate.onValueChanged.AddListener(RateChanged);
        fullScreen.onValueChanged.AddListener(FullChanged);
        masterVolume.onValueChanged.AddListener(MasterVolumeChanged);
        bgmVolume.onValueChanged.AddListener(BGMVolumeChanged);
        sfxVolume.onValueChanged.AddListener(SFXVolumeChanged);
    }

    private void Start()
    {
        DataRefresh();
    }

    private void OnDisable()
    {
        SaveManager.Save();
    }

    void DataRefresh()
    {
        masterVolume.value = soundManager.MasterVolume;
        bgmVolume.value = soundManager.BGMVolume;
        sfxVolume.value = soundManager.SfxVolume;

        fullScreen.isOn = SaveManager.saveData.fullScreen;
        windowRate.value = SaveManager.saveData.rateIndex;
    }

    void RateChanged(int _index)
    {
        SaveManager.SetRate(_index);


        Screen.SetResolution(
        rs.dropDownMap[SaveManager.saveData.rateIndex].x,
        rs.dropDownMap[SaveManager.saveData.rateIndex].y,
        SaveManager.saveData.fullScreen);
    }

    void FullChanged(bool _full)
    {
        SaveManager.SetFull(_full);

        Screen.SetResolution(
        rs.dropDownMap[SaveManager.saveData.rateIndex].x,
        rs.dropDownMap[SaveManager.saveData.rateIndex].y,
        SaveManager.saveData.fullScreen);
    }

    void MasterVolumeChanged(float _vol)
    {
        SaveManager.SetMasterVolume(_vol);

        //soundManager
    }

    void BGMVolumeChanged(float _vol)
    {
        SaveManager.SetBGMVolume(_vol);

        //soundManager
    }

    void SFXVolumeChanged(float _vol)
    {
        SaveManager.SetSFXVolume(_vol);

        //soundManager
    }
}
