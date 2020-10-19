﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_Attack : MonoBehaviour
{
    public ELC_PlayerMoves player;
    public ELC_PlayerStatManager playerStats;
    public AXD_PlayerMoney playerMoney;
    public PlayerHealth playerHealth;

    public float nextResetCombo;
    private enum AttackType { Swich, Sponk }

    private void Update()
    {
        if(Time.time >= nextResetCombo)
        {
            playerStats.CurrentCombo = 0;
        }
    }

    
    public void SwichAttack()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.attackPoint, playerStats.SwichAreaRadius, LayerMask.GetMask("Enemies"));
        foreach (Collider2D enemy in hitEnemies)
        {
            ELC_Enemy tempEnemy = enemy.GetComponent<ELC_Enemy>();
            if(CalculateDamage(AttackType.Swich) >= tempEnemy.currentHealth)
            {
                playerMoney.AddMoney(tempEnemy.enemyStats.MoneyEarnWhenDead);
                playerHealth.AddStock(tempEnemy.enemyStats.ambrosiaEarnedWhenDead);
            }
            else
            {
                playerMoney.AddMoney(tempEnemy.enemyStats.MoneyEarnWhenHit);
            }
            tempEnemy.GetHit(CalculateDamage(AttackType.Swich), playerStats.SwichKnockbackDistance, playerStats.SwichStunTime);
            if (playerStats.CurrentCombo < playerStats.MaxCombo)
            {
                playerStats.CurrentCombo++;
                nextResetCombo = Time.time + playerStats.ComboResetTime;
            }
        }
    }
    public void SponkAttack()
    {
        Debug.Log("Je passe par là");
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(player.attackPoint, new Vector2(playerStats.ThrustWidth,playerStats.Thrustlength), Vector2.Angle(Vector2.up, player.lastDirection), LayerMask.GetMask("Enemies"));
        if (hitEnemies != null)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                ELC_Enemy tempEnemy = enemy.GetComponent<ELC_Enemy>();
                if (CalculateDamage(AttackType.Sponk) >= tempEnemy.currentHealth)
                {
                    playerMoney.AddMoney(enemy.GetComponent<ELC_Enemy>().enemyStats.MoneyEarnWhenDead);
                    playerHealth.AddStock(tempEnemy.enemyStats.ambrosiaEarnedWhenDead);
                }
                else
                {
                    playerMoney.AddMoney(enemy.GetComponent<ELC_Enemy>().enemyStats.MoneyEarnWhenHit);
                }
                tempEnemy.GetHit(CalculateDamage(AttackType.Sponk), playerStats.SponkKnockbackDistance, playerStats.SponkStunTime);
                if (playerStats.CurrentCombo < playerStats.MaxCombo)
                {
                    playerStats.CurrentCombo++;
                    nextResetCombo = Time.time + playerStats.ComboResetTime;
                }
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