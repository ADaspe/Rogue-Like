﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PickupItems : MonoBehaviour
{
    private ELC_ObjectsInventory ObjectsInv;
    private ELC_Detector detector;
    public GameObject Object;
    public enum CollectibleTypes {Object, PowerUp};
    public CollectibleTypes Type = CollectibleTypes.Object;
    private void Start()
    {
        ObjectsInv = GameObject.Find("PlayerInventory").GetComponent<ELC_ObjectsInventory>();
        detector = this.gameObject.GetComponent<ELC_Detector>();
    }

    private void Update()
    {
        if (detector.playerIsInside && this.gameObject.CompareTag("Collectible") && Input.GetButtonDown("Interact"))
        {
            if(Type == CollectibleTypes.Object) ObjectsInv.AddObject(Object);
        }
    }
}