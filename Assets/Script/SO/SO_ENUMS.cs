using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Scenes
{
    Lobby = 0,
    STG_001,
    STG_002,
    SceneCount
}


public enum EnemyType
{ 
    Normal,
    Elite,
    Boss,
}

public enum StatKey
{ 
    Damage,
    HP,
    CoolDown,
    Duration,
    Range,
    AbsorbeRange,
    AutoHeal
}

public enum Waves
{ 
    Basic,
    Big,
    Boss,
}

public class SO_ENUMS : MonoBehaviour
{
}
