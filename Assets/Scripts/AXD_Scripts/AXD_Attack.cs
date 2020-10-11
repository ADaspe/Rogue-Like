using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_Attack : MonoBehaviour
{
    public ELC_PlayerMoves player;
    public ELC_PlayerStatManager playerStats;
    private enum AttackType { Swich, Thrust }
    public void SwichAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.attackPoint, playerStats.SwichAreaRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<AXD_Enemy>().GetHit(CalculateDamage(AttackType.Swich));
            if (playerStats.CurrentCombo < playerStats.MaxCombo)
            {
                playerStats.CurrentCombo++;
            }
        }
    }
    public void ThrustAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(player.attackPoint, new Vector2(playerStats.ThrustWidth,playerStats.Thrustlength), Vector2.Angle(Vector2.up, player.lastDirection), LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<AXD_Enemy>().GetHit(CalculateDamage(AttackType.Thrust));
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
        else if (type == AttackType.Thrust)
        {
            totalDamage = Mathf.RoundToInt((playerStats.ThrustDamage + (playerStats.ThrustDamage * (playerStats.CurrentCombo / 100)))* playerStats.AttackMultiplicator);
        }
        return totalDamage;
    }
}