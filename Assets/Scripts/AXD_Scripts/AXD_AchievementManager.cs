using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_AchievementManager : MonoBehaviour
{
    public List<AXD_AchievementSO> achievementsLocked;
    public List<AXD_AchievementSO> achievementsUnlocked;
    public ELC_PassivesList passivesList;
    private void Start()
    {

        foreach (AXD_AchievementSO achievement in achievementsLocked)
        {
            achievement.setAchievementManager(this);
        }
        foreach (AXD_AchievementSO achievement in achievementsUnlocked)
        {
            achievement.setAchievementManager(this);
        }
    }

    public void FulfillList(AXD_AchievementSO achievement)
    {
        if (achievement.isUnlocked)
        {
            achievementsUnlocked.Add(achievement);
        }
        else
        {
            achievementsLocked.Add(achievement);
        }
    }
}
