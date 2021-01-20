using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFC_Footsteps : MonoBehaviour
{
    public ELC_PlayerMoves playerMoves;
    public ELC_Enemy enemy;
    public ELC_PlayerStatManager statManager;
    public PlayerHealth health;
    public AudioSource pas;
    public AudioSource dash;
    public AudioSource dammage;
    public AudioSource death;
    public AudioSource blueSwich;
    public AudioSource orangeSwich;
    public AudioSource redSwich;
    public AudioSource blueSponk;
    public AudioSource orangeSponk;
    public AudioSource redSponk;
    public AudioSource bouteille;
    private bool canSwich;
    public AudioSource lowlife;
    
    bool deathAlreadyPlayed = false;
    public float nextSponkAttackTime;
    public float nextSwichAttackTime;

    // Update is called once per frame
    void Update()
    {
       if (playerMoves.playerIsImmobile == false && pas.isPlaying == false && dash.isPlaying == false && health.isDead == false)
        {
            pas.volume = Random.Range(0.7f, 1f);
            pas.pitch = Random.Range(0.9f, 1.1f);
            pas.Play();
        }
        
        if (Input.GetAxisRaw("Dash") != 0 && playerMoves.isDashing == true && dash.isPlaying == false)
        {  
            dash.pitch = Random.Range(0.9f, 1.1f);
            dash.Play();

        }

        if (health.isDead == true && death.isPlaying == false && deathAlreadyPlayed == false)
        {
            death.volume = 0.6f;
            death.Play();
            deathAlreadyPlayed = true;
        }

        if (Input.GetAxisRaw("Swich") == 0) canSwich = true;
        if (Input.GetAxisRaw("Swich") != 0 && canSwich && statManager.currentChain == ELC_PlayerStatManager.Chain.Blue && blueSwich.isPlaying == false)
        {
            canSwich = false;
            blueSwich.pitch = Random.Range(0.9f, 1.1f);
            blueSwich.Play();
            
        }
        if (Input.GetAxisRaw("Swich") != 0 && canSwich &&  statManager.currentChain == ELC_PlayerStatManager.Chain.Orange && orangeSwich.isPlaying == false)
        {
            canSwich = false;
            orangeSwich.pitch = Random.Range(0.9f, 1.1f);
            orangeSwich.Play();
        }
        if (Input.GetAxisRaw("Swich") != 0 && canSwich && statManager.currentChain == ELC_PlayerStatManager.Chain.Red && redSwich.isPlaying == false)
        {
            canSwich = false;
            redSwich.pitch = Random.Range(0.9f, 1.1f);
            redSwich.Play();

        }
        if (Input.GetAxisRaw("Sponk") != 0 && statManager.currentChain == ELC_PlayerStatManager.Chain.Blue && blueSponk.isPlaying == false && Time.time > nextSponkAttackTime)
        {
            blueSponk.pitch = Random.Range(0.9f, 1.1f);
            blueSponk.Play();
            nextSponkAttackTime = Time.time + 1f / statManager.SponkAttackRate;

        }
        if (Input.GetAxisRaw("Sponk") != 0 && statManager.currentChain == ELC_PlayerStatManager.Chain.Orange && orangeSponk.isPlaying == false && Time.time > nextSponkAttackTime)
        {
            orangeSponk.pitch = Random.Range(0.9f, 1.1f);
            orangeSponk.Play();
            nextSponkAttackTime = Time.time + 1f / statManager.SponkAttackRate;

        }
        if (Input.GetAxisRaw("Sponk") != 0 && statManager.currentChain == ELC_PlayerStatManager.Chain.Red && redSponk.isPlaying == false && Time.time > nextSponkAttackTime)
        {
            redSponk.pitch = Random.Range(0.9f, 1.1f);
            redSponk.Play();
            nextSponkAttackTime = Time.time + 1f / statManager.SponkAttackRate;

        }

        if (Input.GetAxisRaw("Heal") != 0 && bouteille.isPlaying == false)
        {
            bouteille.Play();
        }
        if (Input.GetAxisRaw("Heal") == 0 && bouteille.isPlaying == true)
        {
            bouteille.Stop();
        }
        if (!lowlife.isPlaying && statManager.currentHealth <= statManager.MaxHealth * statManager.LifeStopDecrease / 100)
        {
            lowlife.Play();
        }

        
    }
}
