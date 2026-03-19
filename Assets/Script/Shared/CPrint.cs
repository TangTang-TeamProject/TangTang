using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CPrint
{
    public static bool Enable = true;
    public static bool EnableRichText = true;

    private static int _indentLevel = 0;

    private const int INDENT_SPACES = 2;

    private static string Indent
    {

        get { return new string(' ', _indentLevel * INDENT_SPACES); }
    }

    public static void IndentPush()
    {
        _indentLevel++;
    }

    public static void IndentPop()
    {
        _indentLevel--;
        if (_indentLevel < 0) _indentLevel = 0;
    }


    private enum ELogKind
    {
        Log,
        Warn,
        Error,
        Success
    }

    private static void Emit(ELogKind kind, string msg, string tag = null, string colorHex = null)
    {

        if (!Enable) return;

        string prefix = string.Empty;

        if (!string.IsNullOrEmpty(tag))
        {

            if (EnableRichText && !string.IsNullOrEmpty(colorHex))
            {
                prefix = $"<color={colorHex}>[{tag}]</color>";
            }

            else
            {
                prefix = $"[{tag}]";
            }
        }

        string final = $"{Indent}{prefix}{msg}";

        switch (kind)
        {
            case ELogKind.Log:
            case ELogKind.Success:
                Debug.Log(final);
                break;

            case ELogKind.Warn:
                Debug.LogWarning(final);
                break;

            case ELogKind.Error:
                Debug.LogError(final);
                break;
        }
    }   

    // Log / Warn / Error
    public static void Log(string msg)
    {
        Emit(ELogKind.Log, msg);
    }

    public static void Warn(string msg)
    {
        Emit(ELogKind.Warn, msg, "WARN", "#FF9100");
   
    }
    public static void Error(string msg)
    {
        Emit(ELogKind.Error, msg, "ERROR", "#FF1744");
    }

    public static void Success(string msg)
    {
        Emit(ELogKind.Success, msg, "OK", "#00C853");
    }

   
}
