using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_NotifPythie : MonoBehaviour
{

    public Material baseMaterial;
    public Material notifMaterial;
    public SpriteRenderer sr;

    private void OnEnable()
    {
        if(PlayerPrefs.GetInt("NotifPythie") == 1)
        {
            sr.material = notifMaterial;
            PlayerPrefs.SetInt("NotifPythie", 0);
        }
        else
        {
            sr.material = baseMaterial;
        }
    }
}
