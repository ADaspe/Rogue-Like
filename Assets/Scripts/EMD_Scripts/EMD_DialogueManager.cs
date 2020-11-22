using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EMD_DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    private int index;
    [TextArea(3,10)]
    public string[] sentences;
    public float DelayTime;
    public bool QuitTime;
    // récuperer la variable DialogueOn du script trigger
    public bool DialogueOn = false;
    public GameObject DialogueCanvas;
    public GameObject ContinueButton;
    public GameObject QuitButton;
    public List<EMD_NPCIsTrigger> NPCsList;
    

    
    private void Update()
    {
        if (DialogueOn == true)
        {
            DialogueCanvas.SetActive(true);
            StartCoroutine(Type());
            //besoin de stopper le jeux en arrière plan
        }

        ChangeButton();

        if (Input.GetKey(KeyCode.Escape))
        {
            QuitDialogue();
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(DelayTime);
        }
    }
    
    public void ChangeButton()
    {
        if (QuitTime == true)
        {
            ContinueButton.SetActive(false);
            QuitButton.SetActive(true);
        }
    }

    public void QuitDialogue()
    {
        DialogueCanvas.SetActive(false);
        DialogueOn = false;
        //besoin de redemarrer le jeux en arrière plan
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            QuitTime = true;
        }
    }
}
