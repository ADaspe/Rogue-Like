using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_ScreenShakes : MonoBehaviour
{
    public GameObject CinemachineCam;
    private CinemachineCameraOffset CamOffset;

    private Vector3 originOffset;

    private bool isScreenShaking;

    private float timeToReplaceCam;

    private void Start()
    {
        isScreenShaking = false;
        timeToReplaceCam = 0;
        CamOffset = CinemachineCam.GetComponent<CinemachineCameraOffset>();
        originOffset = CamOffset.m_Offset;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(ScreenShakes(0.1f, 0.02f, 0.2f));
        }
        if (isScreenShaking) timeToReplaceCam = Time.time + 0.4f;
        if(timeToReplaceCam < Time.time && isScreenShaking == false) CamOffset.m_Offset.Set(0, 0, 0);
    }

    public IEnumerator ScreenShakes(float intensity, float frequency, float duration)
    {
        
        isScreenShaking = false;
        CamOffset = CinemachineCam.GetComponent<CinemachineCameraOffset>();
        originOffset = CamOffset.m_Offset;

        isScreenShaking = true;

        StartCoroutine(Shakes(intensity, frequency));

        yield return new WaitForSeconds(duration);

        isScreenShaking = false;
        
    }

    IEnumerator Shakes(float intensity, float frequencyInSeconds)
    {
        while(isScreenShaking == true)
        {
            float newX = Random.Range(-intensity, intensity);
            float newY = Random.Range(-intensity, intensity);

            Vector3 newPosition = new Vector3(originOffset.x + newX, originOffset.y + newY, originOffset.z);
            CamOffset.m_Offset.Set(newPosition.x, newPosition.y, newPosition.z);
            yield return new WaitForSeconds(frequencyInSeconds);
            yield return null;
        }
        CamOffset.m_Offset.Set(originOffset.x, originOffset.y, originOffset.z);
    }


}
