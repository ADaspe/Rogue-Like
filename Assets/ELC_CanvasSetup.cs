using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ELC_CanvasSetup : MonoBehaviour
{
    public GameObject canvas;
    private Canvas canvasComponent;
    void Start()
    {
        canvasComponent = canvas.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canvasComponent.worldCamera == null) canvasComponent.worldCamera = FindObjectOfType<Camera>();
    }
}
