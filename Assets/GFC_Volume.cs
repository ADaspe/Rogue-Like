using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFC_Volume : MonoBehaviour
{
    public AudioListener listener;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeVolume (float newValue)
    {
        float newVolume = AudioListener.volume;
        newVolume = newValue;
        AudioListener.volume = newVolume;
        PlayerPrefs.SetFloat("Volume", newVolume);
    }
}
