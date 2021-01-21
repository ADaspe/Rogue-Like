using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AXD_PlayerMoney : MonoBehaviour
{
    public Text moneycount;
    public int currentMoney = 0;

    private void Start()
    {
        currentMoney = FindObjectOfType<ELC_DataStore>().LoadData("playerMoney");
        moneycount.text = currentMoney.ToString();
    }

    public void AddMoney(int moneyToAdd)
    {
        currentMoney = currentMoney + moneyToAdd;
        moneycount.text = currentMoney.ToString();
        FindObjectOfType<ELC_DataStore>().SaveData();
    }

    public void ResetMoney()
    {
        currentMoney = 0;
        moneycount.text = currentMoney.ToString();
        FindObjectOfType<ELC_DataStore>().SaveData();
    }
}
