using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider healthSlider;
    public float lifeLost;
    public float stopLosing;
    
    //tout ça c'est pour que le slider marche bien, commence avec les bonnes valeurs (et qu'on puisse le tweeker dans playerhealth)
    public void SetMaxHealth(int health)
    {
       healthSlider.maxValue = health;
       healthSlider.value = health;
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }


}
