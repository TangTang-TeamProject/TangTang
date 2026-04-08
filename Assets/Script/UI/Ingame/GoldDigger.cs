using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDigger
{
    private int totalGold = 0;
    private int amount;

    public void AddGold()
    {
        totalGold += amount;
    }


    public int CalcGold()
    {
        return totalGold;
    }
}
