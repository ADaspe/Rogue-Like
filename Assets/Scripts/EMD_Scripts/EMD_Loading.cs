using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMD_Loading : MonoBehaviour
{
    public GameObject LoadingCanvas;
    private ELC_RoomsGenerator RoomsGeneratorScript;

    private void Start()
    {
        RoomsGeneratorScript = FindObjectOfType<ELC_RoomsGenerator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (RoomsGeneratorScript.GenerationFinish == true)
        {
            LoadingCanvas.SetActive(false);
        }
    }
}
