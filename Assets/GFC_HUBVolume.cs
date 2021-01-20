using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFC_HUBVolume : MonoBehaviour
{
    
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
