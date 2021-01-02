using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_HydraHead : MonoBehaviour
{

    public GameObject projectile;
    public ELC_EnemySO headStats;
    public int currentHealth;

    private void Start()
    {
        headStats = GetComponent<ELC_Enemy>().enemyStats;
    }

    public void Charge()
    {

    }

    public void Attack()
    {

    }

    public void Shoot()
    {

    }

    public void GetHit(int damage, Vector3 directionToFlee, float knockbackDistance = 0, float stunTime = 0, bool invulnerable = false)
    {
        currentHealth -= damage;
        Debug.Log("Head : Ouch !");
        if(currentHealth <= 0)
        {
            Destroy(this);
        }

    }
}
