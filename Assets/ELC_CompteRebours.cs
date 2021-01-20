using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_CompteRebours : MonoBehaviour
{
    public float timeBeforeDestruct;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("compte");
    }

    IEnumerator compte()
    {
        yield return new WaitForSeconds(timeBeforeDestruct);
        Destroy(this.gameObject);
    }
}
