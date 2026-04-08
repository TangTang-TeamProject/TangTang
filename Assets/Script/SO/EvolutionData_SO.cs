using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameData / EvolutionData", fileName = "EvolutionDataSO")]
public class EvolutionData_SO : ScriptableObject
{
    [SerializeField]
    private string evolutionID = "";
    [SerializeField]
    private string baseSkillID = "";
    [SerializeField]
    private string requiredArtifactID = "";

    public string EvolutionID => evolutionID;
    public string BaseSkillID => baseSkillID;
    public string RequiredArtifactID => requiredArtifactID;
}
