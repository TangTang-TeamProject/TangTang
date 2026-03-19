using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;


[CreateAssetMenu(menuName = "GameData / ArtifactRegistry", fileName = "ArtifactRegistrySO")]
public class ArtifactRegistry : ScriptableObject
{
    [SerializeField]
    private List<ArtifactData_SO> artifacts = new List<ArtifactData_SO>();

    public IReadOnlyList<ArtifactData_SO> Artifacts => artifacts;

    private Dictionary<int, ArtifactData_SO> dataDic = new Dictionary<int, ArtifactData_SO>();

    public void ReMake()
    {
        dataDic.Clear();

        for (int i = 0; i < artifacts.Count; i++)
        {
            dataDic.Add(artifacts[i].ArtifactID, artifacts[i]);
        }
    }

    public ArtifactData_SO GetArtifactByID(int _ID)
    {
        if (dataDic.TryGetValue(_ID, out ArtifactData_SO data))
        {
            return data;
        }

        CPrint.Log("ArtifactRegistry - Error");
        return null;
    }
}
