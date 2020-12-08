using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EMD_PorteHub : MonoBehaviour
{
    public string GameScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))SceneManager.LoadScene(GameScene);
    }
}
