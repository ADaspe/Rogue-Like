using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EMD_NPCIsTrigger : MonoBehaviour
{
    public bool IsInRange = false;
    public bool DialogueOn = false;


    private void Update()
    {
        //Si le player est près du npc et qu'il appuie sur "e" le dialogue démarre 
        if (Input.GetAxisRaw("Interact") == 1 && IsInRange == true)
        {
            DialogueOn = true; 
        }
    }

    //Detecter si le player est près du NPC
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsInRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IsInRange = false;
    }
    
}

