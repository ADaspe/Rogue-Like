using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider slider;
    public float lifeLost;
    public float stopLosing;

    public void SetMaxHealth(int health)
    {
       slider.maxValue = health;
       slider.value = health;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value > stopLosing)
        {
            slider.value -= Time.deltaTime * lifeLost;
        }
        
    }
}
