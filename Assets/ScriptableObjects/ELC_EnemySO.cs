using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
public class ELC_EnemySO : ScriptableObject
{
    [Header ("Name")]
    public string Name;

    [Header("Health")]
    public float MaxHealth;

    [Header("Movements")]
    public float MovementSpeed;
    public enum PathBehaviour { FollowPlayer, FleePlayer, StayAtDistance, FollowPath, IsImmobile };
    public PathBehaviour EnemyPath;
    public float LimitDistanceToStay;
    public bool NeedTimeToTurn;
    public float TurnSpeed;

    [Header("Attack Capacity")]

    public bool FriendlyFire;

    public bool DistanceAttack;
    public GameObject Projectile; //Le projectile doit avoir un ScriptableObject
    public float DistanceAttackArea;
    public float DistanceCooldown;

    public bool CorpseAttack;
    public float AttackCooldown;
    public float AttackRange;
    public float WaitBeforeAttack;
    public float AttackStrenght;
    public bool AttackStun;

    [Header("Dash")]
    public bool DashOnPlayer;
    public float DashTime;
    public float DistanceToRun;
    public float DashStrenght;
    public bool DashStun;

    [Header("Spawn Entities")]
    public bool SpawnEntitiesRegulary;
    public bool SpawnEntitiesAtDeath;
    public float SpawnCooldown;
    public float NumberOfEntitiesToSpawn;
    public GameObject EntitiesToSpawn;

    [Header("Money")]
    public float MoneyEarnWhenHit;
    public float MoneyEarnWhenDead;

    //Essential for instantiate Enemy
    public Sprite sprite;
    //public MonoBehaviour AIScript;
    public Animator Animator;
}