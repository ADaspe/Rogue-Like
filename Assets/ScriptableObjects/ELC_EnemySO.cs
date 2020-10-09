using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
public class ELC_EnemySO : ScriptableObject
{
    //Name
    public string Name;

    //Health
    public float MaxHealth;

    //Movements
    public float MovementSpeed;
    public enum PathBehaviour { FollowPlayer, FleePlayer, StayAtDistance, FollowPath, IsImmobile };
    public PathBehaviour EnemyPath;
    public float LimitDistanceToStay;
    public bool NeedTimeToTurn;
    public float TurnSpeed;

    //AttackCapacity

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

    //Dash
    public bool DashOnPlayer;
    public float DashSpeed;
    public float DistanceToRun;
    public float DashStrenght;
    public bool DashStun;

    //Spawn Entities
    public bool SpawnEntitiesRegulary;
    public bool SpawnEntitiesAtDeath;
    public float SpawnCooldown;
    public float NumberOfEntitiesToSpawn;
    public GameObject EntitiesToSpawn;

    //Money
    public float MoneyEarnWhenHit;
    public float MoneyEarnWhenDead;

    //Essential for instantiate Enemy
    public Sprite sprite;
    //public MonoBehaviour AIScript;
    public Animator Animator;
}