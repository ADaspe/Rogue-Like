using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EMD_NPCIsTrigger : MonoBehaviour
{
    public bool IsInRange = false;
    public bool DialogueOn = false;
    public GameObject DialogueManager;
    private EMD_DialogueManager DialogueManagerScript;
    private bool DialogueIsAlreadyActive;

    private void Start()
    {
        DialogueManagerScript = DialogueManager.GetComponent<EMD_DialogueManager>();
    }

    private void Update()
    {
        DialogueIsAlreadyActive = DialogueManagerScript.DialogueIsActive;
        //Si le player est près du npc et qu'il appuie sur "e" le dialogue démarre 
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Interact")) && IsInRange == true && !DialogueIsAlreadyActive && DialogueManagerScript.DialogueEnding == false)
        {
            DialogueManagerScript.ActualNPC = this.gameObject;
            DialogueManagerScript.StartCoroutine("StartDialogue");
        }
        else if (DialogueManagerScript.DialogueEnding == true)
        {
            DialogueManagerScript.DialogueEnding = false;
        }
        if (DialogueIsAlreadyActive && DialogueManagerScript.ActualNPC == this.gameObject && IsInRange == false) DialogueManagerScript.QuitDialogue();
    }

    //Detecter si le player est près du NPC
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = true;
        }
        else
        {
            Debug.Log("Mais c'est pas un joueur !");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IsInRange = false;
    }
    
}

