using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_HitVFX : MonoBehaviour
{
    public string attackType;
    public int playerPhase;
    public float lifeDuration;
    private int fxNumber;


    void Start()
    {
        fxNumber = Mathf.RoundToInt(Random.Range(1, 3));
        this.GetComponent<Animator>().SetInteger("PlayerPhase", playerPhase);
        this.GetComponent<Animator>().SetInteger("FxNumber", fxNumber);

        if (attackType == "Sponk")
        {
            this.GetComponent<Animator>().SetBool("Sponk", true);
            this.GetComponent<Animator>().SetBool("Swich", false);
        }
        else if (attackType == "Swich")
        {
            this.GetComponent<Animator>().SetBool("Sponk", false);
            this.GetComponent<Animator>().SetBool("Swich", true);
        }

        
        StartCoroutine("lifeTime");

    }

    IEnumerator lifeTime()
    {
        yield return new WaitForSeconds(lifeDuration);
        Destroy(this.gameObject);
    }
}
