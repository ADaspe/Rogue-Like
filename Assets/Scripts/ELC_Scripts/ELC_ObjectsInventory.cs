using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ELC_ObjectsInventory : MonoBehaviour
{
    public GameObject RightHandObject;
    public GameObject RightHandHUD;
    public int quantityObject1;

    public GameObject LeftHandObject;
    public GameObject LeftHandHUD;
    public int quantityObject2;

    public GameObject PassiveHUD;

    private GameObject player;

    
    static public ELC_PassiveSO ActivePassif;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetButtonDown("RightHandUse") && RightHandObject != null && quantityObject1 > 0) //Lorsqu'on clique pour utiliser l'object de main droite
        {
            GameObject InstantiatedObject = Instantiate(RightHandObject, player.transform.position, Quaternion.identity);
            InstantiatedObject.GetComponent<ELC_ObjectsUse>().StartCoroutine("Use");
            quantityObject1--;
            UpdateDisplay();
        }
        else if(Input.GetButtonDown("LeftHandUse") && LeftHandObject != null && quantityObject2 > 0)//Lorsqu'on clique pour utiliser l'object de main gauche
        {
            GameObject InstantiatedObject = Instantiate(LeftHandObject, player.transform.position, Quaternion.identity);
            InstantiatedObject.GetComponent<ELC_ObjectsUse>().StartCoroutine("Use");
            quantityObject2--;
            UpdateDisplay();
        }
        
    }

    public void UpdateDisplay()
    {
        if (RightHandObject != null)
        {
            RightHandHUD.GetComponent<Image>().enabled = true;
            RightHandHUD.GetComponent<Image>().sprite = RightHandObject.GetComponent<ELC_ObjectsUse>().ObjectsScriptableObject.HUDSprite;
        }
        else RightHandHUD.GetComponent<Image>().enabled = false;

        if (LeftHandObject != null)
        {
            LeftHandHUD.GetComponent<Image>().enabled = true;
            LeftHandHUD.GetComponent<Image>().sprite = LeftHandObject.GetComponent<ELC_ObjectsUse>().ObjectsScriptableObject.HUDSprite;
        }
        else LeftHandHUD.GetComponent<Image>().enabled = false;

        if (ActivePassif != null)
        {
            PassiveHUD.GetComponent<Image>().enabled = true;
            PassiveHUD.GetComponent<Image>().sprite = ActivePassif.HUDSprite;
        }
        else PassiveHUD.GetComponent<Image>().enabled = false;
    }

    public void AddObject(GameObject Object, int quantity)
    {
        if (RightHandObject == null || RightHandObject == Object)
        {
            RightHandObject = Object;
            quantityObject1 = quantity;
            UpdateDisplay();
        }
        else if (LeftHandObject == null || LeftHandObject == Object)
        {
            LeftHandObject = Object;
            quantityObject2 = quantity;
            UpdateDisplay();
        }
    }
}
