using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_BossMusic : MonoBehaviour
{
    public GameObject bossMusic;
    public ELC_Detector detector;

    void Update()
    {
        if (detector.playerIsInside)
        {
            bossMusic.SetActive(true);
            //FindObjectOfType<ELC_RoomsGenerator>().Music.SetActive(false);
        }
    }
}
