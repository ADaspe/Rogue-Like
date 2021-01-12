using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PlayerStatManager : MonoBehaviour
{

    public enum Chain { Blue, Orange, Red}
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
    public bool losingLife;
    public float lifeThreshold;

    [Header ("Dash Stats")]
    public float DashDistance;
    public float DashTime;
    public float SwichDashDistance;
    public float SwichDashTime;
    public float SponkDashDistance;
    public float SponkDashTime;

    [Header ("General Attack Stats")]
    public float SwichAttackRate;
    public float SponkAttackRate;
    public float SwichKnockbackDistance;
    public float SwichStunTime;
    public float SwichSlowMotionValue;
    public float SwichSlowMotionDuration;
    public float SponkKnockbackDistance;
    public float SponkStunTime;
    public float SponkSlowMotionValue;
    public float SponkSlowMotionDuration;
    public float AttackMultiplicatorChain;
    public float BerserkMultiplicator;
    public float CurrentCombo;
    public float MaxCombo;
    public float ComboResetTime;
    [Range(0, 100)]
    public int colateralDamage;
    [Range(0, 100)]
    public float mainTargetKnockBack;


    [Header("Chain")]
    public Chain currentChain;
    public int hitToNextChain;
    public int currentHitChain;
    public float chainTime;
    [Header("Blue")]
    [Range(1, 5)]
    public float damageMultiplicatorBlue;
    [Range(1, 5)]
    public float KnockbackMultiplicatorBlue;
    [Range(1, 5)]
    public float DashMultiplicatorBlue;
    [Header("Orange")]
    [Range(1, 5)]
    public float damageMultiplicatorOrange;
    [Range(1, 5)]
    public float KnockbackMultiplicatorOrange;
    [Range(1, 5)]
    public float DashMultiplicatorOrange;
    [Header("Red")]
    [Range(1, 5)]
    public float damageMultiplicatorRed;   
    [Range(1, 5)]
    public float KnockbackMultiplicatorRed;
    [Range(1, 5)]
    public float DashMultiplicatorRed;
    public float SwitchChainSlowMotionValue;
    public float SwitchChainSlowMotionDuration;



    [Header ("Swich Stats")]
    public float SwichDamage;
    [Range(0, 5)]
    public float SwichAreaRadius;
    [Header ("Thrust Stats")]
    public float SponkDamage;
    [Range(0, 5)]
    public float SponkWidth;
    [Range(0, 5)]
    public float Sponklength;

    [Header("Animations Times")]
    public float AnimationDashTime;
    public float AnimationSwichTime;
    public float AnimationSponkTime;

    [Header("Power Ups")]
    public float AttackMultiplicatorPU = 1;
    public float SpeedMultiplicatorPU = 1;
    public float DefenseMultiplicatorPU = 1;
    public float MoneyMultiplicatorPU = 1;

    [Header("Passives")]
    public float FilAresDamagesTakenMultiplicator;
    public float FilAresBerserkMultiplicator;

    [Header("ScreenShakes")]
    public float AttackShakeIntensity;
    public float AttackShakeFrequency;
    public float AttackShakeDuration;
    public float GetHitShakeIntensity;
    public float GetHitShakeFrequency;
    public float GetHitShakeDuration;
}
