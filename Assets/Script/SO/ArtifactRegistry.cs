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

    private Dictionary<string, ArtifactData_SO> dataDic = new Dictionary<string, ArtifactData_SO>();

    void NullCheck()
    {
        if (dataDic != null && dataDic.Count != 0)
        {
            return;
        }

        MakeDic();
    }

    public void MakeDic()
    {
        dataDic.Clear();

        for (int i = 0; i < artifacts.Count; i++)
        {
            dataDic.Add(artifacts[i].ArtifactID, artifacts[i]);
        }
    }

    public ArtifactData_SO GetArtifactByID(string _ID)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out ArtifactData_SO data))
        {
            return data;
        }

        CPrint.Error("ArtifactRegistry - Cant Find");
        return null;
    }

    public ArtifactData_SO GetRandomArti()
    {
        int a = Random.Range(0, artifacts.Count);

        return artifacts[a];
    }
}
