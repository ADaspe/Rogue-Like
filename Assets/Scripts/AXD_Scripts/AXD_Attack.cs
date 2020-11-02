using System.Collections;
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
        List<ELC_Enemy> colateralVictims = null;
        ELC_Enemy closestEnemy = null;
        //Get all enemies to attack
        if (hitEnemies != null)
        {
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

            CalculateReward(closestEnemy);
            closestEnemy.GetHit(CalculateDamage(AttackType.Swich), playerStats.SwichKnockbackDistance * (playerStats.mainTargetKnockBack / 100), playerStats.SwichStunTime);

            //Attack all secondary targets
            foreach (ELC_Enemy enemy in colateralVictims)
            {
                CalculateReward(enemy);
                enemy.GetHit(CalculateDamage(AttackType.Swich), playerStats.SwichKnockbackDistance);
            }

        }
    }
    public void SponkAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(player.attackPoint, new Vector2(playerStats.ThrustWidth,playerStats.Thrustlength), Vector2.Angle(Vector2.up, player.lastDirection), LayerMask.GetMask("Enemies"));
        List<ELC_Enemy> colateralVictims = null;
        ELC_Enemy closestEnemy = null;
        if (hitEnemies != null)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                //Get all enemies to attack
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

            CalculateReward(closestEnemy);
            closestEnemy.GetHit(CalculateDamage(AttackType.Sponk), playerStats.SponkKnockbackDistance * (playerStats.mainTargetKnockBack / 100), playerStats.SponkStunTime);

            //Attack colateral victims

            foreach (ELC_Enemy enemy in colateralVictims)
            {
                CalculateReward(enemy);
                enemy.GetHit(CalculateDamage(AttackType.Sponk), playerStats.SponkKnockbackDistance);
            }
            
        }
    }
    private int CalculateDamage(AttackType type, bool colateral = false)
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
        Debug.Log("Damage dealt : " + totalDamage);
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