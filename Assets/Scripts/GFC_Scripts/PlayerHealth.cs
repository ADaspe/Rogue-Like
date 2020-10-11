using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public ELC_PlayerStatManager playerStats;

    public Health healthBar;
    public Bouteille bouteille;
    

    // ça c'est pour dire que la vie commence au max (pour le joueur et sur la barre de vie
    void Start()
    {
        playerStats.currentHealth = playerStats.MaxHealth;
        healthBar.SetMaxHealth(playerStats.MaxHealth);
    }

    //ça c'est comment il prend des dégâts, et ça synchronise en live la barre de vie pour être sûr qu'elle suive 
    void GetHit(int damage)
    {
        playerStats.currentHealth = healthBar.healthSlider.value;
        playerStats.currentHealth -= damage;

        healthBar.SetHealth(playerStats.currentHealth);
    }
    //ça c'est la manière dont il se heal, tout en diminuant la réserve d'olives. Et ça se synchronise avec les deux jauges.
    public void Heal(float heal)
    {
        playerStats.currentHealth = healthBar.healthSlider.value;
        playerStats.currentStock = bouteille.bottleSlider.value;
        playerStats.currentHealth += heal;
        playerStats.currentStock -= heal;

        healthBar.SetHealth(playerStats.currentHealth);

        bouteille.SetOlive(playerStats.currentStock);

    }
}
