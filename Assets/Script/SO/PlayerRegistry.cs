using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData / PlayerRegistry", fileName = "PlayerRegistrySO")]
public class PlayerRegistry : ScriptableObject
{
    [SerializeField]
    private List<PlayerData_SO> players = new List<PlayerData_SO>();

    public IReadOnlyList<PlayerData_SO> Players => players;

    private Dictionary<int, PlayerData_SO> dataDic = new Dictionary<int, PlayerData_SO>();

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

        for (int i = 0; i < players.Count; i++)
        {
            dataDic.Add(players[i].PlayerID, players[i]);
        }
    }

    public PlayerData_SO GetEnemyByID(int _ID)
    {
        NullCheck();

        if (dataDic.TryGetValue(_ID, out PlayerData_SO data))
        {
            return data;
        }

        CPrint.Error("PlayerRegistry - Cant Find");
        return null;
    }
}
