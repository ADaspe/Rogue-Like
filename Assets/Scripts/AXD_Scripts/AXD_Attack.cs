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
    public GameObject playerInventory;
    public GameObject hitVFX;

    public bool AppetitDeLycaonIsActive;
    public float AppetitDeLycaonHealPerEnemies;
    public float AppetitDeLycaonHealPerCollateral;

    public bool Egide;

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
            StartCoroutine(GameManager.GetComponent<ELC_ScreenShakes>().ScreenShakes(playerStats.AttackShakeIntensity, playerStats.AttackShakeFrequency, playerStats.AttackShakeDuration));
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
                    if (closestEnemy != null) colateralVictims.Add(closestEnemy);
                    closestEnemy = tempEnemy;
                }
                else
                {
                    colateralVictims.Add(tempEnemy);
                }

                GameObject vfx = Instantiate(hitVFX, enemy.gameObject.transform);
                if (type.Equals(AttackType.Sponk.ToString())) vfx.GetComponent<ELC_HitVFX>().attackType = "Sponk";
                else vfx.GetComponent<ELC_HitVFX>().attackType = "Swich";

                switch (playerStats.currentChain)
                {
                    case ELC_PlayerStatManager.Chain.Blue:
                        vfx.GetComponent<ELC_HitVFX>().playerPhase = 1;
                        break;
                    case ELC_PlayerStatManager.Chain.Orange:
                        vfx.GetComponent<ELC_HitVFX>().playerPhase = 2;
                        break;
                    case ELC_PlayerStatManager.Chain.Red:
                        vfx.GetComponent<ELC_HitVFX>().playerPhase = 3;
                        break;
                    default:
                        break;
                }
            }
            //Attack main target
            if (closestEnemy != null)
            {
                CalculateReward(closestEnemy, type);
                if (type.Equals(AttackType.Swich.ToString()))
                {
                    GameManager.GetComponent<ELC_TimeScale>().ScaleTime(playerStats.SwichSlowMotionValue, playerStats.SwichSlowMotionDuration);
                    closestEnemy.GetHit(CalculateDamage(AttackType.Swich), closestEnemy.movesTowardPlayer, playerStats.SwichKnockbackDistance * (playerStats.mainTargetKnockBack / 100), playerStats.SwichStunTime);
                }
                else if (type.Equals(AttackType.Sponk.ToString()))
                {
                    GameManager.GetComponent<ELC_TimeScale>().ScaleTime(playerStats.SponkSlowMotionValue, playerStats.SponkSlowMotionDuration);
                    if (!closestEnemy.isInvulnerable && !closestEnemy.isTmpInvulnerable)
                    {
                        closestEnemy.GetHit(CalculateDamage(AttackType.Sponk), closestEnemy.movesTowardPlayer, playerStats.SponkKnockbackDistance * (playerStats.mainTargetKnockBack / 100), playerStats.SponkStunTime, true);
                        
                    }
                }
                
            }
            //Attack all secondary targets
            if (colateralVictims.Count > 0)
            {
                foreach (ELC_Enemy enemy in colateralVictims)
                {
                    CalculateReward(enemy, type);
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

        if(Egide && type == "Swich")
        {
            Collider2D[] bullets = null;
            bullets = Physics2D.OverlapCircleAll(player.attackPoint, playerStats.SwichAreaRadius, LayerMask.GetMask("Projectile"));
            if(bullets != null)
            {
                foreach (Collider2D col in bullets)
                {
                    col.gameObject.GetComponent<ELC_Projectiles>().EgideEffect();
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
                totalDamage = Mathf.RoundToInt((playerStats.SwichDamage + (playerStats.SwichDamage * (playerStats.CurrentCombo / 100))) * playerStats.AttackMultiplicatorChain *playerStats.BerserkMultiplicator * playerStats.FilAresBerserkMultiplicator * playerStats.AttackMultiplicatorPU);
            }else if(colateral == true)
            {
                totalDamage = Mathf.RoundToInt(((playerStats.SwichDamage + (playerStats.SwichDamage * (playerStats.CurrentCombo / 100))) * playerStats.AttackMultiplicatorChain * playerStats.BerserkMultiplicator * playerStats.FilAresBerserkMultiplicator * playerStats.AttackMultiplicatorPU) * playerStats.colateralDamage/100);
            }
        }
        else if (type == AttackType.Sponk)
        {
            if (colateral == false) {
                totalDamage = Mathf.RoundToInt((playerStats.SponkDamage + (playerStats.SponkDamage * (playerStats.CurrentCombo / 100))) * playerStats.AttackMultiplicatorChain * playerStats.BerserkMultiplicator * playerStats.FilAresBerserkMultiplicator * playerStats.AttackMultiplicatorPU);
                if (AppetitDeLycaonIsActive) playerStats.currentHealth += AppetitDeLycaonHealPerEnemies; //Rend de la vie avec le passif de Lycaon
            } else if (colateral == true)
            {
                totalDamage = Mathf.RoundToInt(((playerStats.SponkDamage + (playerStats.SponkDamage * (playerStats.CurrentCombo / 100))) * playerStats.AttackMultiplicatorChain * playerStats.BerserkMultiplicator * playerStats.FilAresBerserkMultiplicator * playerStats.AttackMultiplicatorPU) * playerStats.colateralDamage/100);
                if (AppetitDeLycaonIsActive) playerStats.currentHealth += AppetitDeLycaonHealPerCollateral; //Rend de la vie avec le passif de Lycaon
            }
        }
        return totalDamage;
    }

    private void CalculateReward(ELC_Enemy enemy, string type)
    {
        if (playerStats.CurrentCombo < playerStats.MaxCombo)
        {
            playerStats.CurrentCombo++;
            if(playerStats.CurrentCombo > playerStats.MaxRunCombo)
            {
                playerStats.MaxRunCombo = playerStats.CurrentCombo;
            }
            nextResetCombo = Time.time + playerStats.ComboResetTime;
        }
        if ((type == "Swich" && CalculateDamage(AttackType.Swich) >= enemy.currentHealth) || (type == "Sponk" && CalculateDamage(AttackType.Sponk) >= enemy.currentHealth))
        {
            int moneyEarn = (int)(enemy.enemyStats.MoneyEarnWhenDead * playerStats.MoneyMultiplicatorPU);//Pour arrondir en int
            //playerMoney.AddMoney(moneyEarn);
            //playerInventory.GetComponent<ELC_ObjectsInventory>().AddMoneyToCrates(moneyEarn);
            playerHealth.AddStock(enemy.enemyStats.ambrosiaEarnedWhenDead);
            Debug.Log("Death reward !");
        }
        else
        {
            int moneyEarn = (int)(enemy.enemyStats.MoneyEarnWhenHit * playerStats.MoneyMultiplicatorPU);
            //playerMoney.AddMoney( moneyEarn);
            //playerInventory.GetComponent<ELC_ObjectsInventory>().AddMoneyToCrates(moneyEarn);
            
        }
    }
}