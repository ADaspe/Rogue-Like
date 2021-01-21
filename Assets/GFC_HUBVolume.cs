using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFC_HUBVolume : MonoBehaviour
{
    
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        PlayerPrefs.SetFloat("Volume", 1f);
    }
}
