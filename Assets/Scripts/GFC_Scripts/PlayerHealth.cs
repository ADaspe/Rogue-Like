using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public float maxOlive = 30;
    public float currentOlive;
    public Health healthBar;
    public Bouteille bouteille;

    // ça c'est pour dire que la vie commence au max (pour le joueur et sur la barre de vie
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // ça c'est les inputs pour infliger des dégâts au joueur (normalement ça va tej) et pour le soigner s'il a de l'olive en réserve
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Damage(20);
        }
        if (Input.GetKey(KeyCode.H) && bouteille.slideux.value > 0)
        {
            Heal(10);
        }
    }
    //ça c'est comment il prend des dégâts, et ça synchronise en live la barre de vie pour être sûr qu'elle suive 
    void Damage(int damage)
    {
        currentHealth = healthBar.slider.value;
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
    //ça c'est la manière dont il se heal, tout en diminuant la réserve d'olives. Et ça se synchronise avec les deux jauges.
    public void Heal(int heal)
    {
        currentHealth = healthBar.slider.value;
        currentOlive = bouteille.slideux.value;
        currentHealth += heal;
        currentOlive -= heal;

        healthBar.SetHealth(currentHealth);

        bouteille.SetOlive(currentOlive);

    }
}
