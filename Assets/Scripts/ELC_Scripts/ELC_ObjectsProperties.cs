using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_ObjectsProperties : MonoBehaviour
{
    public ELC_ObjectsUse ObjectsUseScript;
    private ELC_ObjectsSO ObjectSO;

    private void Start()
    {
        ObjectSO = this.gameObject.GetComponent<ELC_ObjectsUse>().ObjectsScriptableObject;
    }


    // Update is called once per frame
    void Update()
    {
        if(ObjectsUseScript.ObjectIsActivated) Debug.Log("Object is Active");

        
    }

    public void Use(string name)
    {
        if (name == "Trident de Poséidon")
        {
            StartCoroutine("Poseidon");
        }
    }

    IEnumerator Poseidon()
    {
        List<GameObject> Targets = ObjectsUseScript.DetectionArea(ObjectSO.LayerMask, ObjectSO.actionArea);
        foreach (GameObject t in Targets)
        {
            Vector3 direction = this.transform.position - t.transform.position;
            t.GetComponent<ELC_Enemy>().GetHit(0, direction, ObjectSO.knockbackDistance, ObjectSO.stunTime);
        }
        yield return new WaitForSeconds(ObjectSO.timeBeforeDestruct / ObjectSO.numberOfTriggers);
        StartCoroutine("Poseidon");
    }


}
