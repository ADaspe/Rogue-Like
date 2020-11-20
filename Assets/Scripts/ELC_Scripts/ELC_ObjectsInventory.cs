using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_ObjectsInventory : MonoBehaviour
{
    public GameObject RightHandObject;
    public int quantityObject1;
    public GameObject LeftHandObject;
    public int quantityObject2;
    private GameObject player;

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
        }
        else if(Input.GetButtonDown("LeftHandUse") && LeftHandObject != null && quantityObject2 > 0)//Lorsqu'on clique pour utiliser l'object de main gauche
        {
            GameObject InstantiatedObject = Instantiate(LeftHandObject, player.transform.position, Quaternion.identity);
            InstantiatedObject.GetComponent<ELC_ObjectsUse>().StartCoroutine("Use");
            quantityObject2--;
        }
    }

    public void AddObject(GameObject Object, int quantity)
    {
        if (RightHandObject == null || RightHandObject == Object)
        {
            RightHandObject = Object;
            quantityObject1 = quantity;
        }
        else if (LeftHandObject == null || LeftHandObject == Object)
        {
            LeftHandObject = Object;
            quantityObject2 = quantity;
        }
    }
}
