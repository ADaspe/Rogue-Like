using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AXD_PlayerAttack : MonoBehaviour
{

    private enum AttackType { Gash, Thrust}
    private Vector3 lastDirection;
    public Transform attackPoint;
    public ELC_PlayerMoves player;
    public int GashDamage, ThrustDamage;
    public int Combos = 0;
    [Header ("Attack Settings")]
    [Range (0,5)]
    public float gashAreaRadius;
    public float gashDashDistance;
    public float gashDashTime;
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
    void Update()
    {
        if (/*player.getPlayerMoves() != Vector3.zero*/ !player.playerIsImmobile)
        {
            attackPoint.position = (transform.position + player.getPlayerMoves().normalized);
            lastDirection = player.getPlayerMoves();
        }
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetAxisRaw("Gash") != 0)
            {
                Attack(AttackType.Gash);
            }
            else if (Input.GetAxisRaw("Thrust") != 0)
            {
                Attack(AttackType.Thrust);
            }
        }
    }

    private void Attack(AttackType type)
    {
        if (type == AttackType.Gash)
        {
            //player.stopDash = Time.time + gashDashTime;
            //player.isGashDashing = true;
            StartCoroutine(player.PlayAnimation("SwishAttack", 0.4f, false, false));
            Debug.Log("Dash Attack CD : " + (player.stopDash - Time.time));
            player.Dash(gashDashDistance, gashDashTime);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, gashAreaRadius);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<AXD_Enemy>().GetHit(CalculateDamage(AttackType.Gash));
                if (Combos < 200)
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
                if (Combos < 200)
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
            //Gizmos.DrawWireCube(attackPoint.position, new Vector3(thrustWidth, thrustlength, 0));
            Gizmos.DrawWireSphere(attackPoint.position, gashAreaRadius);
        }
    }


}
