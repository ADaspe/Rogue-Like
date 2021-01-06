using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_CrateProperties : MonoBehaviour
{
    public ELC_CratesSO CratesSO;

    public int actualLife;
    public int actualMoney;
    public int actualMoneyStockCapacity;
    private List<int> saveLevels = null;

    public bool isFull;
    public int securedMoney;

    void Start()
    {
        actualLife = CratesSO.health;
        actualMoney = Random.Range(CratesSO.startingMinMoney, CratesSO.startingMaxMoney);

    }

    private void Update()
    {
        actualMoneyStockCapacity = CratesSO.stockLimit - actualMoney;

        for (int i = 0; i < CratesSO.steps.Count; i++)
        {
            if (actualMoney > CratesSO.steps[i])
            {
                if (i == CratesSO.steps.Count) securedMoney = CratesSO.steps[i];
                else if (actualMoney < CratesSO.steps[i + 1])
                {
                    securedMoney = CratesSO.steps[i];
                }
            }
        }
    }

    public void HitCrate()
    {
        actualMoney -= Mathf.RoundToInt(actualMoney * ((float)CratesSO.percentageOfMoneyLostWhenHit / 100));
    }

    
}
