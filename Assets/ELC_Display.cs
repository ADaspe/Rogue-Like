using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Display : MonoBehaviour
{
    ELC_Detector detector;
    public GameObject ObjectToDisplay;
    public Vector3 newPosition;
    private GameObject objectInstatiate;

    private void Start()
    {
        detector = this.gameObject.GetComponent<ELC_Detector>();
        if(ObjectToDisplay != null) objectInstatiate = Instantiate(ObjectToDisplay, this.transform.position + newPosition, Quaternion.identity, this.gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (detector.playerIsInside && ObjectToDisplay != null) objectInstatiate.SetActive(true);
        else if( ObjectToDisplay != null) objectInstatiate.SetActive(false);
    }
}
