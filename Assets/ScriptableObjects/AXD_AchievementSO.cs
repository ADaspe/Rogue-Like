using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/AchivementScriptableObject", order = 1)]
public class AXD_AchievementSO : ScriptableObject
{
    public string achievementName;
    public int achievementTier;
    public ELC_Enemy ennemyToDefeat;
    public int numberToDefeat;
    public int numberDefeated;
    public bool isUnlocked;
    public ELC_PassiveSO passifToUnlock;
    [SerializeField]
    private AXD_AchievementManager achievementManager;
    public Sprite HUDSprite;


    public void AddDefeated(int defeated = 1)
    {
        if (!isUnlocked)
        {
            //Debug.Log("Achivement " + name + achievementTier + " progress");
            numberDefeated += defeated;
            if(numberDefeated >= numberToDefeat)
            {
                isUnlocked = true;
                passifToUnlock.isUnlock = true;
                if (!achievementManager.passivesList.PassivesList.Contains(passifToUnlock))
                {
                    achievementManager.passivesList.PassivesList.Add(passifToUnlock);
                }
                
            }
        }
    }

    public void ResetAchievement()
    {
        isUnlocked = false;
        numberDefeated = 0;
    }

    public void setAchievementManager(AXD_AchievementManager manager)
    {
        achievementManager = manager;
    }
}
