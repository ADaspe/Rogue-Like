using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_Attack : MonoBehaviour
{

    public ELC_PlayerMoves player;
    public ELC_PlayerStatManager playerStats;
    public AXD_PlayerMoney playerMoney;
    public PlayerHealth playerHealth;
    public GameObject GameManager;

    public float nextResetCombo;
    public enum AttackType { Swich, Sponk }

    private void Update()
    {
        if(Time.time >= nextResetCombo)
        {
            playerStats.CurrentCombo = 0;
        }
    }

    public void Attack(string type)
    {
        Collider2D[] hitEnemies = null;
        if (type.Equals(AttackType.Swich.ToString()))
        {
            hitEnemies = Physics2D.OverlapCircleAll(player.attackPoint, playerStats.SwichAreaRadius, LayerMask.GetMask("Enemies"));

        }else if(type.Equals(AttackType.Sponk.ToString()))
        {
            hitEnemies = Physics2D.OverlapBoxAll(player.attackPoint, new Vector2(playerStats.SponkWidth, playerStats.Sponklength), Vector2.Angle(Vector2.up, player.lastDirection), LayerMask.GetMask("Enemies"));

        }
        //Get all enemies to attack
        if (hitEnemies != null && hitEnemies.Length != 0)
        {
            player.attackLanded = true;
            List<ELC_Enemy> colateralVictims = new List<ELC_Enemy>();
            ELC_Enemy closestEnemy = null;
            playerStats.currentHitChain++;
            player.attackLanded = true;
            player.timeToResetChain = Time.time + playerStats.chainTime;
            foreach (Collider2D enemy in hitEnemies)
            {

                ELC_Enemy tempEnemy = enemy.GetComponent<ELC_Enemy>();
                if (closestEnemy == null || (Vector3.Distance(player.transform.position, tempEnemy.transform.position) < Vector3.Distance(player.transform.position, closestEnemy.transform.position)))
                {
                    closestEnemy = tempEnemy;
                }
                else
                {
                    colateralVictims.Add(tempEnemy);
                }

            }
            //Attack main target
            if (closestEnemy != null)
            {
                CalculateReward(closestEnemy);
                if (type.Equals(AttackType.Swich.ToString()))
                {
                    GameManager.GetComponent<ELC_TimeScale>().ScaleTime(playerStats.SwichSlowMotionValue, playerStats.SwichSlowMotionDuration);
                    closestEnemy.GetHit(CalculateDamage(AttackType.Swich), closestEnemy.movesTowardPlayer, playerStats.SwichKnockbackDistance * (playerStats.mainTargetKnockBack / 100), playerStats.SwichStunTime);
                }
                else if (type.Equals(AttackType.Sponk.ToString()))
                {
                    GameManager.GetComponent<ELC_TimeScale>().ScaleTime(playerStats.SponkSlowMotionValue, playerStats.SponkSlowMotionDuration);
                    closestEnemy.GetHit(CalculateDamage(AttackType.Sponk), closestEnemy.movesTowardPlayer, playerStats.SponkKnockbackDistance * (playerStats.mainTargetKnockBack / 100), playerStats.SponkStunTime, true);
                }
                
            }
            //Attack all secondary targets
            if (colateralVictims.Count > 0)
            {
                foreach (ELC_Enemy enemy in colateralVictims)
                {
                    CalculateReward(enemy);
                    //Debug.Log(enemy.name+" est une victime colatérale");
                    
                    if (type.Equals(AttackType.Swich.ToString()))
                    {
                        enemy.GetHit(CalculateDamage(AttackType.Swich, true), enemy.movesTowardPlayer, playerStats.SwichKnockbackDistance);
                    }
                    else if (type.Equals(AttackType.Sponk.ToString()))
                    {
                        enemy.GetHit(CalculateDamage(AttackType.Sponk, true), enemy.movesTowardPlayer, playerStats.SponkKnockbackDistance);
                    }
                }
            }

        }
    }
    private int CalculateDamage(AttackType type, bool colateral = false)
    {
        int totalDamage = 0;
        if (type == AttackType.Swich)
        {
            if (colateral == false)
            {
                totalDamage = Mathf.RoundToInt((playerStats.SwichDamage + (playerStats.SwichDamage * (playerStats.CurrentCombo / 100))) * playerStats.AttackMultiplicator);
            }else if(colateral == true)
            {
                totalDamage = Mathf.RoundToInt(((playerStats.SwichDamage + (playerStats.SwichDamage * (playerStats.CurrentCombo / 100))) * playerStats.AttackMultiplicator)*playerStats.colateralDamage/100);
            }
        }
        else if (type == AttackType.Sponk)
        {
            if (colateral == false) {
                totalDamage = Mathf.RoundToInt((playerStats.SponkDamage + (playerStats.SponkDamage * (playerStats.CurrentCombo / 100))) * playerStats.AttackMultiplicator);
            } else if (colateral == true)
            {
                totalDamage = Mathf.RoundToInt(((playerStats.SponkDamage + (playerStats.SponkDamage * (playerStats.CurrentCombo / 100))) * playerStats.AttackMultiplicator)*playerStats.colateralDamage/100);
            }
        }
        return totalDamage;
    }

    private void CalculateReward(ELC_Enemy enemy)
    {
        if (playerStats.CurrentCombo < playerStats.MaxCombo)
        {
            playerStats.CurrentCombo++;
            nextResetCombo = Time.time + playerStats.ComboResetTime;
        }
        if (CalculateDamage(AttackType.Swich) >= enemy.currentHealth)
        {
            playerMoney.AddMoney(enemy.enemyStats.MoneyEarnWhenDead);
            playerHealth.AddStock(enemy.enemyStats.ambrosiaEarnedWhenDead);
        }
        else
        {
            playerMoney.AddMoney(enemy.enemyStats.MoneyEarnWhenHit);
        }
    }
}