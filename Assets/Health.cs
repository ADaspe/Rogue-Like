using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider slider;
    public float lifeLost;

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
        slider.value -= Time.deltaTime * lifeLost;
    }
}
