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
    public bool canAddStockUi;
    public bool canDamageGlitch;
    private void Start()
    {
        canGlitch = true;
        canStunUI = true;
        canAddStockUi = true;
        canDamageGlitch = true;
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

    public void AddStockUi(float time)
    {
        if(canAddStockUi && anim != null)
        {
            StartCoroutine(AddStockUiRoutine(time));
        }
    }

    public void DamageGlitch(float time)
    {
        if (canDamageGlitch && anim != null)
        {
            StartCoroutine(DamageGlitchRoutine(time));
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

    IEnumerator AddStockUiRoutine(float time)
    {
        canAddStockUi = false;
        anim.SetBool("AddStock", true);
        Debug.Log("AddStockUi");
        yield return new WaitForSeconds(time);
        anim.SetBool("AddStock", false);
        canAddStockUi = true;
    }

    IEnumerator DamageGlitchRoutine(float time)
    {
        canDamageGlitch = false;
        anim.SetBool("Damage", true);
        yield return new WaitForSeconds(time);
        anim.SetBool("Damage", false);
        canDamageGlitch = true;
    }
}
