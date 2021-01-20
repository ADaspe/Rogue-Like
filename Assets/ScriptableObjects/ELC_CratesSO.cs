using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crates", menuName = "ScriptableObjects/CratesScriptableObject", order = 1)]
public class ELC_CratesSO : ScriptableObject
{
    [Header("Type Properties")]
    public string Name;
    public enum rarity { Common, Rare, Epic};
    public rarity spawnFrequency;

    [Header("Money Properties")]
    public int stockLimit;
    public List<int> steps = null;
    public int startingMinMoney;
    public int startingMaxMoney;

    [Header("Durability Properties")]
    public bool isBreakable;
    public int health;
    public int percentageOfMoneyLostWhenHit;

    [Header("Visual Properties")]
    public Sprite HUDSpriteLeft;
    public Sprite HUDSpriteRight;
    public Sprite GroundSprite;

}
