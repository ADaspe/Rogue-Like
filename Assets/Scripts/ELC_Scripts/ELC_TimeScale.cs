using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_TimeScale : MonoBehaviour
{
    private float lastActiveValue;
    private float lastDuration;
    [SerializeField]
    private int activesCoroutines;

    public void ScaleTime(float scaleValue, float durationInSeconds)
    {
        
        StopCoroutine(RescaleTime(lastActiveValue, lastDuration));
        StartCoroutine(RescaleTime(scaleValue, durationInSeconds));
        lastActiveValue = scaleValue;
        lastDuration = durationInSeconds;

    }

    private IEnumerator RescaleTime(float scale,float duration)
    {
        if(scale != 0 && duration != 0 && activesCoroutines ==0)
        {
            activesCoroutines++;
            Time.timeScale = scale;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1;
            activesCoroutines--;
        }
    }
}
