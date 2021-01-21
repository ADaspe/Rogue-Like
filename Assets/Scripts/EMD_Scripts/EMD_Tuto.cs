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
    public Sprite ImageAAfficher1;
    public Sprite ImageAAfficher2;
    public Sprite ImageAAfficher3;

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
        ImageTuto.sprite = ImageAAfficher1;
    }
    public void Button2()
    {
        ImageTuto.sprite = ImageAAfficher2;
    }
    public void Button3()
    {
        ImageTuto.sprite = ImageAAfficher3;
    }

}
