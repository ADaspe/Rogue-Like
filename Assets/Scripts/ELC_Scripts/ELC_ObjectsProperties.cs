using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_ObjectsProperties : MonoBehaviour
{
    public ELC_ObjectsUse ObjectsUseScript;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ObjectsUseScript.ObjectIsActivated) Debug.Log("Object is Active");
    }
}
