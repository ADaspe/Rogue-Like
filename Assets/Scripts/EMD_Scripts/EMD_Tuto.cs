using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EMD_Tuto : MonoBehaviour
{
    public GameObject ContinueButton;
    public GameObject InfoContinueButton;
    public GameObject InfoButton;
    public GameObject InfoCanvas;
    public GameObject DialogueCanvas;
    public GameObject QuittButton;
    public GameObject image;
    public GameObject text;
    public Image ImageTuto;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AfficherTuto()
    {
        image.SetActive(false);
        text.SetActive(false);
        ContinueButton.SetActive(false);
        InfoButton.SetActive(false);
        InfoCanvas.SetActive(true);
    }

}
