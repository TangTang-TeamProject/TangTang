using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// List 번호 == ID로 맞출 것
[CreateAssetMenu(menuName = "GameData / ArtifactRegistry", fileName = "ArtifactRegistrySO")]
public class ArtifactRegistry : ScriptableObject
{
    [SerializeField]
    private List<ArtifactData_SO> artifacts = new List<ArtifactData_SO>();

    public IReadOnlyList<ArtifactData_SO> Artifacts => artifacts;

    public bool GetArtifactByID(int _ID, out ArtifactData_SO artifact)
    {
        artifact = null;

        if (_ID >= artifacts.Count || artifacts[_ID] == null)
            return false;

        artifact = artifacts[_ID];

        return true;
    }
}
