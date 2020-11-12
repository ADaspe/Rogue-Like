using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GFC_PowerUpScript : MonoBehaviour
{
    public ListPowerUp Liste;
    public GameObject PowerUp;
    public Collider2D Collider2D;

    private void OnTriggerEnter2D(Collider2D Collider2D)
    {
        if(Collider2D.gameObject.tag.Equals("Player"))
        {
            Liste.PowerUp = PowerUp;
            Liste.Ajout();
            Debug.Log("Ajout du PowerUp dans la liste");
            Destroy(gameObject);
        }
        
    }

}
