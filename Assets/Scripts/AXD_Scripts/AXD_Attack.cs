using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_Attack : MonoBehaviour
{
    public ELC_PlayerMoves player;
    public ELC_PlayerStatManager playerStats;
    private enum AttackType { Swich, Sponk }
    public void SwichAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.attackPoint, playerStats.SwichAreaRadius, LayerMask.GetMask("Enemies"));
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<ELC_Enemy>().GetHit(CalculateDamage(AttackType.Swich), playerStats.SwichKnockbackDistance, playerStats.SwichStunTime);
            if (playerStats.CurrentCombo < playerStats.MaxCombo)
            {
                playerStats.CurrentCombo++;
            }
        }
    }
    public void ThrustAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(player.attackPoint, new Vector2(playerStats.ThrustWidth,playerStats.Thrustlength), Vector2.Angle(Vector2.up, player.lastDirection), LayerMask.GetMask("Enemies"));
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<ELC_Enemy>().GetHit(CalculateDamage(AttackType.Thrust), playerStats.SponkKnockbackDistance, playerStats.SponkStunTime);
            if (playerStats.CurrentCombo < playerStats.MaxCombo)
            {
                playerStats.CurrentCombo++;
            }
        }
    }
    private int CalculateDamage(AttackType type)
    {
        int totalDamage = 0;
        if (type == AttackType.Swich)
        {
            totalDamage = Mathf.RoundToInt((playerStats.SwichDamage + (playerStats.SwichDamage  * (playerStats.CurrentCombo / 100))) * playerStats.AttackMultiplicator);
        }
        else if (type == AttackType.Sponk)
        {
            totalDamage = Mathf.RoundToInt((playerStats.ThrustDamage + (playerStats.ThrustDamage * (playerStats.CurrentCombo / 100)))* playerStats.AttackMultiplicator);
        }
        return totalDamage;
    }
}