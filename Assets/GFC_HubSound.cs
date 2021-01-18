using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFC_HubSound : MonoBehaviour
{
    public AudioSource spawn;
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        spawn.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawn.isPlaying && !music.isPlaying)
        {
            music.Play();
        }
    }
}
