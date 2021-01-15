using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AXD_UiGlitch : MonoBehaviour
{
    public Material defaultMat;
    public Material glitchMat;
    private Image img;
    public bool canGlitch;
    private void Start()
    {
        canGlitch = true;
        img = GetComponent<Image>();
    }
    public void Glitch(float time)
    {
        if (canGlitch)
        {
            StartCoroutine(GlitchRoutine(time));
        }
    }

    IEnumerator GlitchRoutine(float time)
    {
        canGlitch = false;
        img.material = glitchMat;
        yield return new WaitForSeconds(time);
        img.material = defaultMat;
        canGlitch = true;
    }
}
