﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PowerUp", menuName = "ScriptableObjects/PowerUpScriptableObject", order = 1)]
public class ELC_PowerUpSO : ScriptableObject
{
    //Pour le type de Power Up
    public enum Type {Attack, Speed, Heal, MoneyEarn};
    public Type type;

    //Pour son niveau de puissance
    public int level;

    //De compbien il multiplie la valeur
    [Range(1, 4)]
    public float multiplicator;

    //Durée du PowerUp
    public float duration;

    //sprite affiché sur le HUD
    public Sprite HUDSprite;
}
