using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_PlayerAttack : MonoBehaviour
{

    private enum AttackType { Gash, Thrust}
    private Vector3 lastDirection;
    public Transform attackPoint;
    public ELC_PlayerMoves player;
    public int GashDamage, ThrustDamage;
    [Header ("Attack Settings")]
    [Range (0,5)]
    public float gashAreaRadius;
    [Range(0, 5)]
    public float thrustWidth;
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
        if (player.getPlayerMoves() != Vector3.zero)
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
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, gashAreaRadius);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<AXD_Enemy>().GetHit(GashDamage);
            }

        } else if (type == AttackType.Thrust)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(thrustWidth, thrustlength), Vector2.Angle(Vector2.up,lastDirection));
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<AXD_Enemy>().GetHit(ThrustDamage);
            }
        }
        nextAttackTime = Time.time + 1f / attackRate;
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
