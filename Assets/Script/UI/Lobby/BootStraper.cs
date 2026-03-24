using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootStraper : MonoBehaviour
{
    private void Awake()
    {
        SaveManager.Load();
    }
}
