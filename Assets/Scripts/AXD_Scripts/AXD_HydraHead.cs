using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_HydraHead : MonoBehaviour
{

    public GameObject projectile;
    public ELC_EnemySO headStats;
    //[HideInInspector]
    public ELC_Enemy enemyScript;
    private void Awake()
    {
        enemyScript = GetComponent<ELC_Enemy>();
        
        headStats = enemyScript.enemyStats;
        enemyScript.currentHealth = headStats.MaxHealth;
    }

    public void Charge()
    {
        headStats.EnemyPath = ELC_EnemySO.PathBehaviour.FollowPlayer;
        headStats.DistanceAttack = false;
        headStats.CloseCombatAttack = false;
        headStats.DashOnPlayer = true;
    }

    public void Attack()
    {
        headStats.EnemyPath = ELC_EnemySO.PathBehaviour.FollowPlayer;
        headStats.DistanceAttack = false;
        headStats.CloseCombatAttack = true;
        headStats.DashOnPlayer = false;
    }

    public void Shoot()
    {
        headStats.EnemyPath = ELC_EnemySO.PathBehaviour.StayAtDistance;
        headStats.DistanceAttack = true;
        headStats.CloseCombatAttack = false;
        headStats.DashOnPlayer = false;
    }

    /*public void GetHit(int damage, Vector3 directionToFlee, float knockbackDistance = 0, float stunTime = 0, bool invulnerable = false)
    {
        enemyScript.currentHealth -= damage;
        Debug.Log("Head : Ouch !");
        if(enemyScript.currentHealth <= 0)
        {
            Destroy(this);
        }

    }*/
}
