using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_TimeScale : MonoBehaviour
{
    public IEnumerator ScaleTime(float scaleValue,float durationInSeconds)
    {
        Time.timeScale = scaleValue;
        yield return new WaitForSecondsRealtime(durationInSeconds);
        Time.timeScale = 1;
    }
}
