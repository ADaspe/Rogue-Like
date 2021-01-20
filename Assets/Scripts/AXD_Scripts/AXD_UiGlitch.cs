using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AXD_UiGlitch : MonoBehaviour
{
    public Material defaultMat;
    public Material glitchMat;
    private Image img;
    public Animator anim;
    public bool canGlitch;
    public bool canStunUI;
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

    public void StunUI(float time)
    {
        if (canStunUI && anim !=null)
        {
            StartCoroutine(StunPerso(time));
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

    IEnumerator StunPerso(float time)
    {
        canStunUI = false;
        anim.SetBool("Stun", true);
        yield return new WaitForSeconds(time);
        anim.SetBool("Stun", false);
        canStunUI = true;
    }
}
