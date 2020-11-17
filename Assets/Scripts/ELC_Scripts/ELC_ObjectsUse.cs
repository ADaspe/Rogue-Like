using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_ObjectsUse : MonoBehaviour
{
    public ELC_ObjectsSO ObjectsScriptableObject;

    private GameObject player;

    private bool isThrowingObject;
    public bool ObjectIsActivated;

    private Vector2 directionToThrown;

    private int quantity;

    

    private void Update()
    {
        if (isThrowingObject) transform.Translate(directionToThrown * (ObjectsScriptableObject.distanceToThrown / ObjectsScriptableObject.timeBeforeTouchGround) * Time.deltaTime);
    }

    public IEnumerator Use()
    {
        player = GameObject.Find("Player");
        directionToThrown = player.GetComponent<ELC_PlayerMoves>().lastDirection.normalized;
        StartCoroutine("ThrownObject");

        yield return new WaitWhile(() => isThrowingObject == false);
        StartCoroutine("LifeDuration");
    }

    IEnumerator ThrownObject()
    {
        isThrowingObject = true;
        yield return new WaitForSeconds(ObjectsScriptableObject.timeBeforeTouchGround);
        isThrowingObject = false;
    }

    IEnumerator LifeDuration()
    {
        ObjectIsActivated = true;
        yield return new WaitForSeconds(ObjectsScriptableObject.timeBeforeDestruct);
        ObjectIsActivated = false;
        Destroy(this.gameObject);
    }

}
