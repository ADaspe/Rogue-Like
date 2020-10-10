using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ELC_PlayerStatManager : MonoBehaviour
{
    [Header ("Move Stats")]
    public float Speed;
    [Header ("Life Stats")]
    public float LifeDecreaseSpeed;
    public float LifeStopDecrease;
    public float MaxHealth;
    public float Resistance;
    [Header ("Dash Stats")]
    public float DashDistance;
    public float DashSpeed;
    public float SwichDashDistance;
    public float SwichDashTime;
    public float ThrustDashDistance;
    public float ThrustDashTime;
    [Header ("General Attack Stats")]
    public float AttackRate;
    public float AttackMultiplicator;
    public float CurrentCombo;
    public float MaxCombo;
    [Header ("Swich Stats")]
    public float SwichDamage;
    [Range(0, 5)]
    public float SwichAreaRadius;
    [Header ("Thrust Stats")]
    public float ThrustDamage;
    [Range(0, 5)]
    public float ThrustWidth;
    [Range(0, 5)]
    public float Thrustlength;



}
