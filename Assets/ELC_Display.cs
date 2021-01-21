using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Display : MonoBehaviour
{
    ELC_Detector detector;
    public GameObject ObjectToDisplay;
    public Vector3 newPosition;
    public GameObject objectInstatiate;
    public GameObject ButtonDisplay;
    private bool buttonIsActive;
    private bool buttonIsInDesactivation;
    private bool buttonIsActivated;

    private void OnEnable()
    {
        if (ObjectToDisplay != null) objectInstatiate = Instantiate(ObjectToDisplay, this.transform.position + newPosition, Quaternion.identity, this.gameObject.transform);
    }

    private void Start()
    {
        detector = this.gameObject.GetComponent<ELC_Detector>();
        //if(ObjectToDisplay != null) objectInstatiate = Instantiate(ObjectToDisplay, this.transform.position + newPosition, Quaternion.identity, this.gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (detector.playerIsInside)
        {
            if( ObjectToDisplay != null) objectInstatiate.SetActive(true);
            if(!buttonIsActive && !buttonIsInDesactivation) StartCoroutine("spawnButton");
        }
        else if(buttonIsActive == true && !buttonIsInDesactivation && buttonIsActivated) StartCoroutine("stopButton");

        if (!detector.playerIsInside && ObjectToDisplay != null)
        {
            
            objectInstatiate.SetActive(false);
        }
    }

    IEnumerator spawnButton()
    {
        buttonIsActive = true;
        Debug.Log("oui");
        ButtonDisplay.GetComponent<Animator>().SetBool("Apparition", true);
        yield return new WaitForSeconds(0.5f);
        ButtonDisplay.GetComponent<Animator>().SetBool("Apparition", false);
        ButtonDisplay.GetComponent<Animator>().SetBool("Idle", true);
        buttonIsActivated = true;
    }

    IEnumerator stopButton()
    {
        Debug.Log("oui2");
        buttonIsInDesactivation = true;
        ButtonDisplay.GetComponent<Animator>().SetBool("Idle", false);
        ButtonDisplay.GetComponent<Animator>().SetBool("Disparition", true);
        yield return new WaitForSeconds(0.5f);
        ButtonDisplay.GetComponent<Animator>().SetBool("Disparition", false);
        buttonIsActive = false;
        buttonIsInDesactivation = false;
        buttonIsActivated = false;
    }
}
