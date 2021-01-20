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
    public Image ImageAAfficher1;
    public Image ImageAAfficher2;
    public Image ImageAAfficher3;

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
    public void Button1()
    {
        ImageTuto = ImageAAfficher1;
    }
    public void Button2()
    {
        ImageTuto = ImageAAfficher2;
    }
    public void Button3()
    {
        ImageTuto = ImageAAfficher3;
    }

}
