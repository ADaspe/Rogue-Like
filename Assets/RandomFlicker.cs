using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlicker : MonoBehaviour
{
    //If you use Int in the Random.Range() then it will
    //return an Int
    public float RanTimerHigh = 14f;
    public float RanTimerLow = 6f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        float Randomfloat = Random.Range(RanTimerLow, RanTimerHigh);
        rend.material.SetFloat("_speedValue", Randomfloat);
    }
}
