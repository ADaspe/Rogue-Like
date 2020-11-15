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

    public bool FriendlyFire;
    public bool stun;
    public bool knockback;

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