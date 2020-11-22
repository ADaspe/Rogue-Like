using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_TimeScale : MonoBehaviour
{
    public bool isPaused;
    private float lastActiveValue;
    private float lastDuration;
    [SerializeField]
    private int activesCoroutines;


    private void Update()
    {
        if (Input.GetButtonDown("Pause")) PauseGame();
    }


    public void ScaleTime(float scaleValue, float durationInSeconds)
    {
        
        StopCoroutine(RescaleTime(lastActiveValue, lastDuration));
        StartCoroutine(RescaleTime(scaleValue, durationInSeconds));
        lastActiveValue = scaleValue;
        lastDuration = durationInSeconds;

    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    private IEnumerator RescaleTime(float scale,float duration)
    {
        if(scale != 0 && duration != 0 && activesCoroutines ==0)
        {
            activesCoroutines++;
            Time.timeScale = scale;
            yield return new WaitForSecondsRealtime(duration);
            if(!isPaused) Time.timeScale = 1;
            activesCoroutines--;
        }
    }
}
