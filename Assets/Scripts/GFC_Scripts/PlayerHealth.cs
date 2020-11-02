using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public ELC_PlayerStatManager playerStats;

    public bool isDead = false;
    public Health healthBar;
    public Bouteille bouteille;

    private void Update()
    {
        playerStats.currentHealth = healthBar.healthSlider.value;
    }

    // ça c'est pour dire que la vie commence au max (pour le joueur et sur la barre de vie
    void Start()
    {
        playerStats.currentHealth = playerStats.MaxHealth;
        healthBar.SetMaxHealth(playerStats.MaxHealth);
        playerStats.currentStock = playerStats.maxStock;
    }

    //ça c'est comment il prend des dégâts, et ça synchronise en live la barre de vie pour être sûr qu'elle suive 
    void GetHit(int damage)
    {
        if (!playerStats.invulnerability)
        {
            playerStats.currentHealth = healthBar.healthSlider.value;
            playerStats.currentHealth -= damage;
            healthBar.SetHealth(playerStats.currentHealth);
            if(playerStats.currentHealth <= 0)
            {
                isDead = true;
            }
        }
    }
    public void AddStock(int stockToAdd)
    {
        if (bouteille.bottleSlider.value + stockToAdd >= bouteille.bottleSlider.maxValue)
        {
            bouteille.bottleSlider.value = bouteille.bottleSlider.maxValue;
            playerStats.currentStock = bouteille.bottleSlider.value;
        }
        else
        {
            bouteille.bottleSlider.value += stockToAdd;
            playerStats.currentStock = bouteille.bottleSlider.value;
        }
    }
    //ça c'est la manière dont il se heal, tout en diminuant la réserve d'olives. Et ça se synchronise avec les deux jauges.
    public void Heal(float heal)
    {
        if (playerStats.currentStock > 0)
        {
            playerStats.currentHealth = healthBar.healthSlider.value;
            playerStats.currentStock = bouteille.bottleSlider.value;
            playerStats.currentHealth += heal;
            playerStats.currentStock -= heal;

            healthBar.SetHealth(playerStats.currentHealth);

            bouteille.SetStock(playerStats.currentStock);
        }
    }
}
