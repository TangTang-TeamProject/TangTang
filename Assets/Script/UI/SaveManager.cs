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

    public float volume;

    public string dateTime;
}

public static class SaveManager
{
    private static string dataPath = Application.persistentDataPath + "/save.json";

    public static SaveData saveData { get; private set; }

    public static void Save()
    {
        if(saveData == null)
        {
            CPrint.Error("세이브 데이터 없음");
            return;
        }

        string toJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(dataPath, toJson);
    }

    public static void Load()
    {
        if (!File.Exists(dataPath))
        {
            saveData = MakeNew();
            Save();
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
            newData.equipID[i] = 0;
        }

        newData.volume = 100;

        newData.dateTime = null;

        return newData;
    }

    public static void SetDate(string date)
    {
        saveData.dateTime = date;
        Save();
    }

    public static void CalcGold(int num)
    {
        saveData.gold += num;
        Save();
    }

    public static void SetEquip(EquipType slot, int ID)
    {
        saveData.equipID[(int)slot] = ID;
        Save();
    }

    public static int GetEquip(EquipType slot)
    {
        return saveData.equipID[(int)slot];
    }

    public static void SetVolume(float value)
    {
        saveData.volume = value;
        Save();
    }
}
