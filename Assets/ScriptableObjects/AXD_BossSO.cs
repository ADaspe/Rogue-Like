using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/BossScriptableObject", order = 1)]
public class AXD_BossSO : ScriptableObject
{
    public bool invulnerable;
    public int maxHealth;
    public int[] healthPhase;
    public float vulnerableTime;
    public float stratTime;
}
