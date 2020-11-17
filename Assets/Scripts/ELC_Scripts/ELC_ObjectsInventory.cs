using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_ObjectsInventory : MonoBehaviour
{
    public GameObject RightHandObject;
    public GameObject LeftHandObject;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetButtonDown("RightHandUse") && RightHandObject != null)
        {
            GameObject InstantiatedObject = Instantiate(RightHandObject, player.transform.position, Quaternion.identity);
            InstantiatedObject.GetComponent<ELC_ObjectsUse>().StartCoroutine("Use");
        }
        else if(Input.GetButtonDown("LeftHandUse") && LeftHandObject != null)
        {
            GameObject InstantiatedObject = Instantiate(LeftHandObject, player.transform.position, Quaternion.identity);
            InstantiatedObject.GetComponent<ELC_ObjectsUse>().StartCoroutine("Use");
        }
    }

    public void AddObject(GameObject Object)
    {
        if (RightHandObject == null) RightHandObject = Object;
        else if (LeftHandObject == null) LeftHandObject = Object;
    }
}
