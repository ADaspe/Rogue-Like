using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFC_HydraSound : MonoBehaviour
{
    public ELC_Enemy enemy;
    public AudioSource attaque;
    public AudioSource death;
    public AudioSource dammage;
    public AudioSource distance;
    public bool deathAlreadyPlayed = false;


    // Update is called once per frame
    void Update()
    {
        if (enemy.isAttacking == true && attaque.isPlaying == false && !enemy.isDistanceAttacking)
        {
            attaque.Play();
        }
        if (enemy.isDistanceAttacking == true && attaque.isPlaying == false && !distance.isPlaying)
        {
            distance.Play();
        }
        if (enemy.currentHealth <= 0 && death.isPlaying == false && deathAlreadyPlayed == false)
        {
            death.Play();
            deathAlreadyPlayed = true;
        }
        if (enemy.isHit == true && dammage.isPlaying == false)
        {
            dammage.Play();
        }
    }
}
