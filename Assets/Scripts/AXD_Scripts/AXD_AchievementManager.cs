using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AXD_AchievementManager : MonoBehaviour
{
    public List<AXD_AchievementSO> allAchievements;
    public List<AXD_AchievementSO> achievementsLocked;
    public List<AXD_AchievementSO> achievementsUnlocked;
    public List<ELC_PassiveSO> allTimeUnlockedPassives;
    public ELC_PassivesList passivesList;
    public bool reset;
    public bool hasUnlockedAchievement;
    private void Start()
    {
        foreach (AXD_AchievementSO achievement in allAchievements)
        {
            achievement.setAchievementManager(this);
            if (achievement.isUnlocked)
            {
                achievementsUnlocked.Add(achievement);
                if(achievement.passifToUnlock != null)
                {
                    passivesList.PassivesList.Add(achievement.passifToUnlock);
                }
            }
            else
            {
                achievementsLocked.Add(achievement);
            }
            Debug.Log("Achievement added");
        }
        if(SceneManager.GetActiveScene().name == "EMD_HUB")
        {
            hasUnlockedAchievement = false;
        }
        if (reset)
        {
            ResetAchievement();
        }
    }

    public void ResetAchievement()
    {
        foreach(AXD_AchievementSO achievement in allAchievements)
        {
            achievement.isUnlocked = false;
            achievement.numberDefeated = 0;
        }
        passivesList.PassivesList.Clear();
        foreach (ELC_PassiveSO passif in passivesList.PassivesList)
        {
            if (allTimeUnlockedPassives.Contains(passif))
            {
                passif.isUnlock = true;
                passivesList.PassivesList.Add(passif);
            }
            else
            {
                passif.isUnlock = false;
            }
        }
        PlayerPrefs.SetInt("playerMoney", 0);
        reset = false;
    }
}
