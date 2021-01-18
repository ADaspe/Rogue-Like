using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_SpawnBoss : MonoBehaviour
{
    public ELC_Detector detection;
    public GameObject Hydra;
    private void Update()
    {
        if (detection.playerIsInside)
        {
            Hydra.SetActive(true);
            this.enabled = false;
        }
    }
}
