using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/AchivementScriptableObject", order = 1)]
public class AXD_AchievementSO : ScriptableObject
{
    public string name;
    public int achievementTier;
    public ELC_Enemy ennemyToDefeat;
    public int numberToDefeat;
    public int numberDefeated;
    public bool isUnlocked;
    //public ELC_Passif passifToUnlock;

    public void AddDefeated(int defeated = 1)
    {
        if (!isUnlocked)
        {
            //Debug.Log("Achivement " + name + achievementTier + " progress");
            numberDefeated += defeated;
            if(numberDefeated >= numberToDefeat)
            {
                isUnlocked = true;
                //Debug.Log("Achivement " + name + achievementTier + " unlocked !");
            }
        }
    }

    public void ResetAchievement()
    {
        isUnlocked = false;
        numberDefeated = 0;
    }
}
