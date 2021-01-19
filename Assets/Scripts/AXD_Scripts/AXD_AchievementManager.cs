﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_AchievementManager : MonoBehaviour
{
    public List<AXD_AchievementSO> allAchievements;
    public List<AXD_AchievementSO> achievementsLocked;
    public List<AXD_AchievementSO> achievementsUnlocked;
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
    }

    public void ResetAchievement()
    {
        foreach(AXD_AchievementSO achievement in allAchievements)
        {
            achievement.isUnlocked = false;
            achievement.numberDefeated = 0;
                 
        }
        foreach(ELC_PassiveSO passif in passivesList.PassivesList)
        {
            passif.isUnlock = false;
        }
        passivesList.PassivesList.Clear();
    }
}
