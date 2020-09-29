using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider slider;
    public float lifeLost;
    public float stopLosing;
    
    //tout ça c'est pour que le slider marche bien, commence avec les bonnes valeurs (et qu'on puisse le tweeker dans playerhealth)
    public void SetMaxHealth(int health)
    {
       slider.maxValue = health;
       slider.value = health;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    // Et ça c'est pour que la vie coule en continu si elle est pas trop basse
    void Update()
    {
        if(slider.value > stopLosing)
        {
            slider.value -= Time.deltaTime * lifeLost;
        }
        
    }
}
