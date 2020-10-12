using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bouteille : MonoBehaviour
{
    public Slider bottleSlider;

    //tout ça c'est pour que le slider marche bien, commence avec les bonnes valeurs (et qu'on puisse le tweeker dans playerhealth)
    public void SetMaxOlive(int olive)
    {
        bottleSlider.maxValue = olive;
        bottleSlider.value = olive;
    }

    public void SetStock(float olive)
    {
        bottleSlider.value = olive;
    }
}