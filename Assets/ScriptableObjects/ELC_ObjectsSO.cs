using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/ObjectsScriptableObject", order = 1)]
public class ELC_ObjectsSO : ScriptableObject
{
    public string Name;
    public float Strenght;
    public float cooldown;
    public int quantity;

    public LayerMask LayerMask;
    public bool stun;
    public float stunTime;
    public bool knockback;
    public float knockbackDistance;

    public float numberOfTriggers;
    public float timeBeforeTouchGround;
    public float distanceToThrown;
    public float timeBeforeDestruct;
    public bool destructWhenSomeoneTouchIt;

    public bool explodeOnDestruct;

    public float actionArea;

    public MonoBehaviour ObjectsScript;
    public Sprite HUDSprite;
    public Animator Animator;
}