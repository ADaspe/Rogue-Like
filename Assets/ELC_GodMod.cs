using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_GodMod : MonoBehaviour
{
    private GameObject player;
    public GameObject basicCamera;

    public float destroyerRadius;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) Teleport();

        if (Input.GetKeyDown(KeyCode.M)) DestroyEnemies();

        if (Input.GetKeyDown(KeyCode.I)) AddMoney();
    }

    private void Teleport()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mouseWorldPosition = basicCamera.GetComponent<Camera>().ScreenToWorldPoint(mousePos);

        player.transform.position = mouseWorldPosition;
    }

    private void AddMoney()
    {
        FindObjectOfType<EMD_PassifManager>().cheat = !FindObjectOfType<EMD_PassifManager>().cheat;
    }

    private void DestroyEnemies()
    {
        Collider2D[] colliders;
        colliders = Physics2D.OverlapCircleAll(player.transform.position, destroyerRadius, LayerMask.GetMask("Enemies"));
        foreach(Collider2D col in colliders)
        {
            Destroy(col.gameObject);
        }
    }

}
