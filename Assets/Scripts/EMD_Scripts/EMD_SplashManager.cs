using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EMD_SplashManager : MonoBehaviour
{
    public string MenuScene;

    private void Update()
    {
        if (Input.GetButtonDown("Interact")) GoMainMenu();
    }
    public void GoMainMenu()
    {
        SceneManager.LoadScene(MenuScene);
    }
}
