using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bouteille : MonoBehaviour
{
    public Slider slideux;

    //tout ça c'est pour que le slider marche bien, commence avec les bonnes valeurs (et qu'on puisse le tweeker dans playerhealth)
    public void SetMaxOlive(int olive)
    {
        slideux.maxValue = olive;
        slideux.value = olive;
    }

    public void SetOlive(float olive)
    {
        slideux.value = olive;
    }
}