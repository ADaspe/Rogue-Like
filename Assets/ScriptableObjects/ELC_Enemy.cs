using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyScriptableObject", order = 1)]
public class ELC_Enemy : ScriptableObject
{
    public float maxHeal;
    public float strenght;
    public float speed;
    public float moneyWhenHit;
    public float moneyWhenDead;

    public bool followPlayer;
    public bool fleePlayer;
    public bool stayAtDistance;
    public float distance;

    public bool fireBullets;
    public GameObject bullet;

    public bool instantiateSomethingAtDeath;
    public GameObject objectToInstantiate;

    public bool dash;
    public float dashSpeed;
    public float dashTime;

    public Sprite sprite;
    public MonoBehaviour AIScript;
    public Animator Animator;
}