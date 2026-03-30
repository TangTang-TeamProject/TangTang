using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum EquipType
{ 
    Head = 0,
    Body,
    Leg,
    Cape,
    Weapon,
    TypeCount,
}

[System.Serializable]
public class SaveData
{
    public int gold;

    public int[] equipID;

    public long dateTime;

    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public int rateIndex;
    public bool fullScreen;
}

public static class SaveManager
{
    private static string dataPath = Application.persistentDataPath + "/save.json";

    static SaveData saveData;

    public static SaveData data
    {
        get 
        {
            LazyLoad();
            return saveData;
        }
    }

    static void LazyLoad()
    {
        if (saveData == null)
        {
            Load();
        }
    }


    public static void Save()
    {
        LazyLoad();

        string toJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(dataPath, toJson);
    }

    public static void Load()
    {
        if (!File.Exists(dataPath))
        {
            saveData = MakeNew();
            Save();
            return;
        }

        string fromJson = File.ReadAllText(dataPath);

        try
        {
            saveData = JsonUtility.FromJson<SaveData>(fromJson);
        }
        catch
        {
            CPrint.Error("세이브 데이터 오류");
            saveData = MakeNew();
            Save();
        }
    }

    public static SaveData MakeNew()
    {
        SaveData newData = new SaveData();

        newData.gold = 0;
        
        newData.equipID = new int[(int)EquipType.TypeCount];

        for (int i = 0; i < (int)EquipType.TypeCount; i++)
        {
            newData.equipID[i] = i;
        }

        newData.masterVolume = 1f;
        newData.bgmVolume = 1f;
        newData.sfxVolume = 1f;

        newData.dateTime = DateTime.UtcNow.Ticks;

        newData.rateIndex = 0;

        newData.fullScreen = false;

        return newData;
    }

    public static void SetDate()
    {
        saveData.dateTime = DateTime.UtcNow.Ticks;
    }

    public static void CalcGold(int num)
    {
        saveData.gold += num;
    }

    public static void SetEquip(EquipType slot, int ID)
    {
        saveData.equipID[(int)slot] = ID;
    }

    public static int GetEquip(EquipType slot)
    {
        return saveData.equipID[(int)slot];
    }

    public static void SetMasterVolume(float _master)
    {
        saveData.masterVolume = _master;
    }

    public static void SetBGMVolume(float _bgm)
    {
        saveData.bgmVolume = _bgm;
    }
    public static void SetSFXVolume(float _sfx)
    {
        saveData.sfxVolume = _sfx;
    }

    public static void SetRate(int _index)
    {
        saveData.rateIndex = _index;
    }

    public static void SetFullScreen(bool _fullScreen)
    {
        saveData.fullScreen = _fullScreen;
    }
}
