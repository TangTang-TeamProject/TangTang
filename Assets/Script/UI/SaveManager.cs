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
    TypeCount,
}

public enum ClosedChar
{ 
    Noah = 0,
    Ria,
    CharCount,
}

[System.Serializable]
public class SaveData
{
    public int gold;

    public string[] equipID;

    public long dateTime;

    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public int rateIndex;
    public bool fullScreen;

    public string selectedChar;

    public bool[] openChar;
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

    static SaveData MakeNew()
    {
        SaveData newData = new SaveData();

        newData.gold = 0;
        
        newData.equipID = new string[(int)EquipType.TypeCount];

        newData.equipID[(int)EquipType.Cape] = "CAPE1";
        newData.equipID[(int)EquipType.Head] = "HEAD1";
        newData.equipID[(int)EquipType.Body] = "BODY1";
        newData.equipID[(int)EquipType.Leg] = "LEG1";

        newData.masterVolume = 1f;
        newData.bgmVolume = 1f;
        newData.sfxVolume = 1f;

        newData.dateTime = DateTime.UtcNow.Ticks;

        newData.rateIndex = 0;

        newData.fullScreen = true;

        newData.selectedChar = "CHR_001";

        newData.openChar = new bool[(int)ClosedChar.CharCount];

        for (int i = 0; i < (int)ClosedChar.CharCount; i++)
        {
            newData.openChar[i] = false;
        }

        return newData;
    }

    public static void Refresh()
    {
        saveData = MakeNew();
        Save();
    }

    public static void SetDate()
    {
        saveData.dateTime = DateTime.UtcNow.Ticks;
    }

    public static void CalcGold(int num)
    {
        saveData.gold += num;
    }

    public static void SetEquip(EquipType slot, string ID)
    {
        saveData.equipID[(int)slot] = ID;
    }

    public static string GetEquip(EquipType slot)
    {
        return saveData.equipID[(int)slot];
    }

    public static void SetChar(string ID)
    {
        saveData.selectedChar = ID;
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
