using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Detector : MonoBehaviour
{
    public bool playerIsInside;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerIsInside = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerIsInside = false;
    }


}
