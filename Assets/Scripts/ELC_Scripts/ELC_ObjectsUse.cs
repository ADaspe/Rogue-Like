using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_ObjectsUse : MonoBehaviour
{
    public ELC_ObjectsSO ObjectsScriptableObject;
    public ELC_ObjectsProperties ObjectPropScript;

    private GameObject player;

    public bool isThrowingObject;
    public bool ObjectIsActivated;

    private Vector2 directionToThrown;

    private LayerMask layerMask;

    private void Start()
    {
        layerMask = ObjectsScriptableObject.LayerMask;
    }

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
        ObjectPropScript.Use(ObjectsScriptableObject.Name);
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

    public List<GameObject> DetectionArea(LayerMask layerMask, float radius) //Retourne une List des gameObjects qui sont dans la zone de détection
    {
        List<GameObject> TargetsList = new List<GameObject>();
        Collider2D[] colliders;

        colliders = Physics2D.OverlapCircleAll(this.transform.position, radius, layerMask);

        int i = 0;
        foreach (Collider2D cl in colliders)
        {
            TargetsList.Add(cl.gameObject);
            i++;
        }
        return TargetsList;
    }

}
