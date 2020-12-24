using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_CrateProperties : MonoBehaviour
{
    public ELC_CratesSO CratesSO;

    public int actualLife;
    public int actualMoney;
    public int actualMoneyStockCapacity;

    public bool isFull;
    private int securedMoney;

    void Start()
    {
        actualLife = CratesSO.health;
        actualMoney = Random.Range(CratesSO.startingMinMoney, CratesSO.startingMaxMoney);
    }

    private void Update()
    {
        actualMoneyStockCapacity = CratesSO.stockLimit - actualMoney;
    }

    public void HitCrate()
    {
        actualMoney -= Mathf.RoundToInt(actualMoney * ((float)CratesSO.percentageOfMoneyLostWhenHit / 100));
    }
}
