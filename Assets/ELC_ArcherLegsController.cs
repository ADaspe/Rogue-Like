using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_ArcherLegsController : MonoBehaviour
{
    public GameObject mainObject;
    public Animator thisAnimator;
    private ELC_Enemy enemyScript;
    private Animator mainObjectAnimator;

    private void Start()
    {
        mainObjectAnimator = mainObject.GetComponent<Animator>();
        enemyScript = mainObject.GetComponent<ELC_Enemy>();
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyScript.isSpawning) this.GetComponent<SpriteRenderer>().enabled = false;
        else this.GetComponent<SpriteRenderer>().enabled = true;
        thisAnimator.SetFloat("MovesX", mainObjectAnimator.GetFloat("MovesX"));
        thisAnimator.SetFloat("MovesY", mainObjectAnimator.GetFloat("MovesY"));

        if (enemyScript.lastDirection.x > 0) this.GetComponent<SpriteRenderer>().flipX = true;
        else this.GetComponent<SpriteRenderer>().flipX = false;
    }
}
