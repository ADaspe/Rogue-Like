using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public ELC_PlayerStatManager playerStats;
    private ELC_ScreenShakes screenShakeScript;
    public ELC_PlayerMoves playerMovesScript;
    public GFC_Footsteps sound;
    public GameObject playerInventory;
    public bool gettingHit = false;
    public bool isDead = false;
    public Slider healthSlider;
    public Bouteille bouteille;
    public AXD_UiGlitch[] uiToGlitch;
    public float timeToGlitchUiOnHit;
    public static ELC_EnemySO lastHitEnnemy;
    public bool sangGorgonne;



    // ça c'est pour dire que la vie commence au max (pour le joueur et sur la barre de vie
    void Start()
    {
        playerStats.currentHealth = playerStats.MaxHealth;
        SetMaxHealth(playerStats.MaxHealth);
        playerStats.currentStock = playerStats.maxStock;
        screenShakeScript = FindObjectOfType<ELC_ScreenShakes>();
        uiToGlitch = FindObjectsOfType<AXD_UiGlitch>();
    }
    private void Update()
    {
        if (playerStats.losingLife && playerStats.currentHealth > playerStats.MaxHealth * playerStats.LifeStopDecrease / 100)
        {
            playerStats.currentHealth -= playerStats.LifeDecreaseSpeed * Time.deltaTime;
        }
        else if(playerStats.currentHealth <= playerStats.MaxHealth * playerStats.LifeStopDecrease / 100)
        {
            GlitchUI();
        }
        healthSlider.value = playerStats.currentHealth;
        playerStats.BerserkMultiplicator = (1 - (playerStats.currentHealth / playerStats.MaxHealth)) + 1;
    }
    //ça c'est comment il prend des dégâts, et ça synchronise en live la barre de vie pour être sûr qu'elle suive 
    public void GetHit(ELC_EnemySO enemyLastHit,int damage, float knockack = 0, float stun = 0)
    {
        
        if (!playerStats.invulnerability)
        {
            lastHitEnnemy = enemyLastHit;
            Debug.Log("Get Hit Player");
            GlitchUI();
            StartCoroutine(playerMovesScript.ApplyShader(playerMovesScript.damageMatTime, playerMovesScript.damageMat));
            StartCoroutine(screenShakeScript.ScreenShakes(playerStats.GetHitShakeIntensity, playerStats.GetHitShakeFrequency, playerStats.GetHitShakeDuration));
            playerStats.currentHealth = healthSlider.value;
            playerStats.currentHealth -= damage / playerStats.DefenseMultiplicatorPU;
            SetHealth(playerStats.currentHealth);
            if(playerStats.currentHealth <= 0)
            {
                isDead = true;
                StartCoroutine(playerMovesScript.Death(2f));
            }
            playerInventory.GetComponent<ELC_ObjectsInventory>().GetHitCrates();            
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
        if (playerStats.currentStock > 0 && playerStats.currentHealth <= playerStats.MaxHealth-playerStats.lifeThreshold)
        {
            playerStats.currentHealth = healthSlider.value;
            playerStats.currentStock = bouteille.bottleSlider.value;
            playerStats.currentHealth += heal;
            playerStats.currentStock -= heal;

            SetHealth(playerStats.currentHealth);

            bouteille.SetStock(playerStats.currentStock);
        }
    }

    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
        if (sound.dammage.isPlaying == false && sound.death.isPlaying == false && isDead == false && Input.GetAxisRaw("Heal") == 0)
        {
            sound.dammage.Play();
        }
    }
    
    public void GlitchUI()
    {
        foreach (AXD_UiGlitch uiGlitch in uiToGlitch)
        {
            uiGlitch.Glitch(timeToGlitchUiOnHit);
        }
    }
}
