using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
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
        masterVolume.value = SaveManager.data.masterVolume;
        bgmVolume.value = SaveManager.data.bgmVolume;
        sfxVolume.value = SaveManager.data.sfxVolume;

        fullScreen.isOn = SaveManager.data.fullScreen;
        windowRate.value = SaveManager.data.rateIndex;
    }

    void RateChanged(int _index)
    {
        SaveManager.SetRate(_index);


        Screen.SetResolution(
        rs.dropDownMap[SaveManager.data.rateIndex].x,
        rs.dropDownMap[SaveManager.data.rateIndex].y,
        SaveManager.data.fullScreen);
    }

    void FullChanged(bool _full)
    {
        SaveManager.SetFullScreen(_full);

        Screen.SetResolution(
        rs.dropDownMap[SaveManager.data.rateIndex].x,
        rs.dropDownMap[SaveManager.data.rateIndex].y,
        SaveManager.data.fullScreen);
    }

    void MasterVolumeChanged(float _vol)
    {
        SaveManager.SetMasterVolume(_vol);

        SoundManager.Instance.MasterVolumeChange(_vol);
    }

    void BGMVolumeChanged(float _vol)
    {
        SaveManager.SetBGMVolume(_vol);

        SoundManager.Instance.BGMVolumeChange(_vol);
    }

    void SFXVolumeChanged(float _vol)
    {
        SaveManager.SetSFXVolume(_vol);

        SoundManager.Instance.SfxVolumeChange(_vol);
    }
}
