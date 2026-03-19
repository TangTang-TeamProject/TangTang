using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CSV_SO_Converter", menuName = "Converter")]
public class CSV_SO_Converter : EditorWindow
{
    private TextAsset csvFile;
    private MonoScript soScript;
    private string soFilePath = "Assets/SO";

    [MenuItem("Utility/CSV To SO Converter")]
    public static void ShowWindow()
    {
        GetWindow<CSV_SO_Converter>("CSV_SO_Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV - SO 변환기", EditorStyles.boldLabel);

        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);

        GUILayout.Space(10);

        soScript = (MonoScript)EditorGUILayout.ObjectField("SO Script", soScript, typeof(MonoScript), false);

        GUILayout.Space(10);

        soFilePath = EditorGUILayout.TextField("Save Pos", soFilePath);

        GUILayout.Space(10);

        if (GUILayout.Button("Convert"))
        {
            if (csvFile == null)
            {
                Debug.Log("csvFile == null");
            }
            else
            {
                ConvertCSV();
            }
        }
    }

    void ConvertCSV()
    {
        string[] lines = csvFile.text.Split('\n');

        string[] headers = lines[0].Split(',');
        int headerLen = headers.Length;

        Type soType = soScript.GetClass();

        Dictionary<string, FieldInfo> fieldDic = new Dictionary<string, FieldInfo>();

        for (int j = 0; j < headerLen; j++)
        {
            FieldInfo fieldInfo = soType.GetField(headers[j]);

            if (fieldInfo == null)
            {
                Debug.Log("CSV - SO / 호환 안됨");
            }

            fieldDic.Add(headers[j], fieldInfo);
        }


        for (int i = 1; i < lines.Length; i++)
        {
            string[] datas = lines[i].Split(',');

            ScriptableObject NEW_SO = ScriptableObject.CreateInstance(soType);

            for (int j = 0; j < headerLen; j++)
            {
                object data = ParseByType(fieldDic[headers[j]].FieldType, datas[j].Trim());
                fieldDic[headers[j]].SetValue(NEW_SO, data);
            }

            string file = $"{soFilePath}/{datas[0]}.asset";

            AssetDatabase.CreateAsset(NEW_SO, file);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    object ParseByType(Type type, string _data)
    {
        if (type == typeof(int))
        {
            return int.Parse(_data);
        }
        if (type == typeof(float))
        {
            return float.Parse(_data);
        }
        if (type == typeof(string))
        {
            return _data;
        }


        Debug.Log("이상한 타입 자료형 발견");

        return null;
    }
}
