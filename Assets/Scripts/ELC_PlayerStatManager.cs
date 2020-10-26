using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PlayerStatManager : MonoBehaviour
{
    [Header ("Move Stats")]
    public float Speed;
    public bool invulnerability;

    [Header ("Life Stats")]
    public float LifeDecreaseSpeed;
    public float LifeStopDecrease;
    public int MaxHealth;
    public float Resistance;
    public float currentHealth;
    public float maxStock;
    public float currentStock;
    public float healingRate;

    [Header ("Dash Stats")]
    public float DashDistance;
    public float DashTime;
    public float SwichDashDistance;
    public float SwichDashTime;
    public float ThrustDashDistance;
    public float ThrustDashTime;

    [Header ("General Attack Stats")]
    public float SwichAttackRate;
    public float SponkAttackRate;
    public float SwichKnockbackDistance;
    public float SwichStunTime;
    public float SponkKnockbackDistance;
    public float SponkStunTime;
    public float AttackMultiplicator;
    public float CurrentCombo;
    public float MaxCombo;
    public float ComboResetTime;
    [Range(0, 100)]
    public int colateralDamage;
    [Range(0, 100)]
    public float mainTargetKnockBack;

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

    [Header("Animations Times")]
    public float AnimationDashTime;
    public float AnimationSwichTime;
    public float AnimationSponkTime;



}
