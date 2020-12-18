using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EMD_DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public bool DialogueIsActive;
    [SerializeField]
    private int index;
    // récuperer les données du npc (phrase et nom)
    public float DelayTime;
    // récuperer la variable DialogueOn du script trigger
    private bool IsWriting;
    public GameObject DialogueCanvas;
    public GameObject ContinueButton;
    public GameObject QuitButton;
    public List<EMD_NPCIsTrigger> NPCsList;
    public string[] sentences;
    public GameObject ActualNPC;
    

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Dash"))
        {
            QuitDialogue();
        }
        if ( DialogueIsActive && Input.GetButtonDown("Interact"))
        {
            //Debug.Log("yes");
            NextSentence();
        }
    }


    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(DelayTime);
        }
        IsWriting = false;
    }
    
    public void ChangeButton()
    {
        ContinueButton.SetActive(false);
        QuitButton.SetActive(true);
    }

    public void QuitDialogue()
    {
        DialogueCanvas.SetActive(false);
        index = 0;
        textDisplay.text = "";
        DialogueIsActive = false;
        //besoin de remettre les inputs du joueur
    }

    public void NextSentence()
    {
        if (!IsWriting)
        {
            if (index < sentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
                StartCoroutine("StartDialogue");
            }
            else
            {
                //textDisplay.text = "";
                //ChangeButton();
                QuitDialogue();
            }
        }
    }

    public IEnumerator StartDialogue()
    {
        DialogueCanvas.SetActive(true);
        sentences = ActualNPC.GetComponent<EMD_Sentences>().sentences;
        DialogueIsActive = true;
        StartCoroutine("Type");
        IsWriting = true;
        //yield return new WaitWhile(() => IsWriting == true);
        yield return null;
    }
}
