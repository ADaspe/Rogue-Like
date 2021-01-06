using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_DataStore : MonoBehaviour
{
    AXD_PlayerMoney moneyScript;

    

    private void Start()
    {
        moneyScript = FindObjectOfType<AXD_PlayerMoney>();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("playerMoney", moneyScript.currentMoney);
        PlayerPrefs.Save();
    }

    public int LoadData(string dataName)
    {

        if (PlayerPrefs.HasKey(dataName))
        {
            return PlayerPrefs.GetInt(dataName);
        }
        else
        {
            Debug.Log("No data with name " + dataName);
            return 0;
        }
    }
    
}
