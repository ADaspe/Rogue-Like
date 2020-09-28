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
    public MonoBehaviour ObjectsScript;
    public Sprite HUDSprite;
    public Animator Animator;
}
