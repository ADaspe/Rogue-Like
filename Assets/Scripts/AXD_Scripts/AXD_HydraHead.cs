using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_HydraHead : MonoBehaviour
{

    public GameObject projectile;
    public ELC_EnemySO headStats;
    
    //[HideInInspector]
    public AXD_Hydra hydra;
    //[HideInInspector]
    public ELC_Enemy enemyScript;
    private void Awake()
    {

        enemyScript = GetComponent<ELC_Enemy>();
        //enemyScript.enemyStats = headStats;
        enemyScript.currentHealth = enemyScript.enemyStats.MaxHealth;
        enemyScript.speed = enemyScript.enemyStats.MovementSpeed;
        enemyScript.distanceToStay = enemyScript.enemyStats.LimitDistanceToStay;
        enemyScript.currentHealth = headStats.MaxHealth;
    }

    public void Charge()
    {
        headStats.EnemyPath = ELC_EnemySO.PathBehaviour.FollowPlayer;
        headStats.DistanceAttack = false;
        headStats.CloseCombatAttack = false;
        headStats.DashOnPlayer = true;
    }


    public void Shoot()
    {
        headStats.EnemyPath = ELC_EnemySO.PathBehaviour.FollowPlayer;
        headStats.DistanceAttack = true;
        headStats.CloseCombatAttack = false;
        headStats.DashOnPlayer = false;
    }

    public void GetHydraRef(AXD_Hydra hydraArg)
    {
        hydra = hydraArg;
    }

    private void OnDestroy()
    {
        hydra.LoseHead(this);
    }
}
