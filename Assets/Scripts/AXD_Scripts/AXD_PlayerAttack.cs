﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AXD_PlayerAttack : MonoBehaviour
{

    private enum AttackType { Gash, Thrust}
    private Vector3 lastDirection;
    public Transform attackPoint;
    public ELC_PlayerMoves player;
    [SerializeField]
    private LayerMask attackLayerMask;
    public int GashDamage, ThrustDamage;
    public int maxCombo;
    public int Combos = 0;
    public bool PlayerIsAttacking;
    [Header ("Attack Settings")]
    [Range (0,5)]
    public float gashAreaRadius;
    public float swichDashDistance;
    public float swichDashTime;
    [Range(0, 5)]
    public float thrustWidth;
    public float thrustDashDistance;
    public float thrustDashTime;
    [Range(0, 5)]
    public float thrustlength;
    public float attackRate;
    public float nextAttackTime;


    void Start()
    {
        nextAttackTime = Time.time;
    }
    

    private void Attack(AttackType type)
    {
        if (type == AttackType.Gash)
        {
            //player.stopDash = Time.time + gashDashTime;
            //player.isGashDashing = true;
            StartCoroutine(player.PlayAnimation("SwishAttack", 0.4f, false, false));
            //Debug.Log("Dash Attack CD : " + (player.stopDash - Time.time));
            Debug.Log("Dash Attack");
            player.Dash(gashDashDistance, gashDashTime);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, gashAreaRadius, attackLayerMask);
            Debug.Log("Dash Attack CD : " + (player.stopDash - Time.time));
            player.Dash(swichDashDistance, swichDashTime);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, gashAreaRadius);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<AXD_Enemy>().GetHit(CalculateDamage(AttackType.Gash));
                if (Combos < maxCombo)
                {
                    Combos++;
                }
            }

        } else if (type == AttackType.Thrust)
        {
            //player.stopDash = Time.time + thrustDashTime;
            //player.isThrustDashing = true;
            Debug.Log("Dash Attack CD : " + (player.stopDash - Time.time));
            player.Dash(thrustDashDistance, thrustDashTime);
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(thrustWidth, thrustlength), Vector2.Angle(Vector2.up,lastDirection));
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<AXD_Enemy>().GetHit(CalculateDamage(AttackType.Thrust));
                if (Combos < maxCombo)
                {
                    Combos++;
                }
            }
        }
        nextAttackTime = Time.time + 1f / attackRate;
    }

    private int CalculateDamage(AttackType type)
    {
        int totalDamage = 0;
        if(type == AttackType.Gash)
        {
            totalDamage = GashDamage * (Combos / 100);
        }else if(type == AttackType.Thrust)
        {
            totalDamage = ThrustDamage * (Combos / 100);
        }
        return totalDamage;
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            //Gizmos.DrawWireCube(attackPoint, new Vector3(thrustWidth, thrustlength, 0));
            Gizmos.DrawWireSphere(player.attackPoint, gashAreaRadius);
        }
    }


}
