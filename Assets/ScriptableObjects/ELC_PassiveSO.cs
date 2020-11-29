﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passive", menuName = "ScriptableObjects/PassiveScriptableObject", order = 1)]
public class ELC_PassiveSO : ScriptableObject
{
    public string PassiveName;
    public float PassivePrice;

    public Sprite HUDSprite;
}