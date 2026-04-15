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

    public string[] wearingEquip;
    public Dictionary<string, int> equipLevel;

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
        
        newData.wearingEquip = new string[(int)EquipType.TypeCount];

        newData.wearingEquip[(int)EquipType.Cape] = "NoCape";
        newData.wearingEquip[(int)EquipType.Head] = "NoHead";
        newData.wearingEquip[(int)EquipType.Body] = "NoBody";
        newData.wearingEquip[(int)EquipType.Leg] = "NoLegs";

        newData.equipLevel = new Dictionary<string, int>();

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
        saveData.wearingEquip[(int)slot] = ID;
    }

    public static string GetEquip(EquipType slot)
    {
        return saveData.wearingEquip[(int)slot];
    }

    public static void SetEquipLevel(string id, int level)
    {
        if (saveData.equipLevel.ContainsKey(id))
        {
            saveData.equipLevel[id] = level;
        }
        else
        {
            saveData.equipLevel.Add(id, 0);
        }
    }

    public static int GetEquipLevel(string id)
    {
        if (saveData.equipLevel.ContainsKey(id))
        {
            return saveData.equipLevel[id];
        }
        else
        {
            saveData.equipLevel.Add(id, 0);

            return 0;
        }
    }

    public static void SetChar(string ID)
    {
        saveData.selectedChar = ID;
    }

    public static void UnLockChar(ClosedChar _enum)
    {
        saveData.openChar[(int)_enum] = true;
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
