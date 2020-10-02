using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "ScriptableObjects/ObjectsScriptableObject", order = 1)]
public class ELC_Objects : ScriptableObject
{
    public string Name;
    public float Strenght;
    public float cooldown;
    public int quantity;

    public bool thrownableObject;
    public float speedOfTheObject;
    public float timeBeforeTouchingGround;
    public bool placableObject;

    public bool FriendlyFire;
    public bool stun;
    public bool knockback;

    public bool timeBeforeDestruct;
    public bool destructWhenSomeoneTouchIt;

    public bool explodeOnDestruct;

    public bool instantiateSomething;
    public GameObject objectToInstantiate;
    public float numberOfObjectsToInstantiate;

    public bool damageAreaConstantly;
    public float rangeOfDamages;



    public MonoBehaviour ObjectsScript;
    public Sprite HUDSprite;
    public Animator Animator;
}