using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDigger
{
    private int totalGold = 0;
    private int amount;
    private int clearGold;

    //여기서 시작 골드 초기화
    public GoldDigger()
    {
        clearGold = 100; // 맵마다 초기화
        amount = 10; // 
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
        totalGold += clearGold;
        return totalGold;
    }
}
