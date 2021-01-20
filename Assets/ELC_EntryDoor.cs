using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_EntryDoor : MonoBehaviour
{
    public ELC_Detector detector;
    public ELC_SceneManager sceneManager;

    private void Update()
    {
        if (detector.playerIsInside && Input.GetButtonDown("Interact")) sceneManager.SwitchScene("ELC_ProceduralGeneration");
    }
}
