using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_HitVFX : MonoBehaviour
{
    public int playerPhase;
    public float lifeDuration;

    void Start()
    {
        this.GetComponent<Animator>().SetInteger("PlayerPhase", playerPhase);
        StartCoroutine("lifeTime");
    }

    IEnumerator lifeTime()
    {
        yield return new WaitForSeconds(lifeDuration);
        Destroy(this.gameObject);
    }
}
