using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AXD_PlayerMoney : MonoBehaviour
{
    public Text moneycount;
    public int currentMoney = 0;

    public void AddMoney(int moneyToAdd)
    {
        currentMoney += moneyToAdd;
        moneycount.text = currentMoney.ToString();
    }

    public void ResetMoney()
    {
        currentMoney = 0;
        moneycount.text = currentMoney.ToString();
    }
}
