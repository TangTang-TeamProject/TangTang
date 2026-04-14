using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDigger : MonoBehaviour
{
    [SerializeField]
    private StageRegistry stageRegistry;

    private int totalGold = 0;
    private int amount;

    //여기서 시작 골드 초기화
    public GoldDigger()
    {
        string scenes = SceneChanger.instance.NowScene();

        amount = stageRegistry.GetStageDataByID(scenes).Amount; 
    }

    public void AddGold()
    {
        totalGold += amount;
    }

    public int CalcGoldGameOver()
    {
        return totalGold;
    }

    public int GetAmount()
    {
        return amount;
    }

    public int CalcGoldClear()
    {
        string scenes = SceneChanger.instance.NowScene();

        totalGold += stageRegistry.GetStageDataByID(scenes).ClearReward;

        return totalGold;
    }
}
