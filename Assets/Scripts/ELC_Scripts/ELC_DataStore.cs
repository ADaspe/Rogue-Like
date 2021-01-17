using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_DataStore : MonoBehaviour
{
    AXD_PlayerMoney moneyScript;
    public List<AXD_AchievementSO> passiveList = null;
    

    private void Start()
    {
        passiveList = FindObjectOfType<AXD_AchievementManager>().allAchievements;
        moneyScript = FindObjectOfType<AXD_PlayerMoney>();
        foreach (AXD_AchievementSO SO in passiveList)
        {
            if (PlayerPrefs.GetFloat(SO.achievementName) == 0) SO.isUnlocked = false;
            else SO.isUnlocked = true;
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("playerMoney", moneyScript.currentMoney);
        foreach (AXD_AchievementSO SO in passiveList)
        {
            if(SO.isUnlocked) PlayerPrefs.SetFloat(SO.achievementName, 1);
            else PlayerPrefs.SetFloat(SO.achievementName, 0);
        }
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
