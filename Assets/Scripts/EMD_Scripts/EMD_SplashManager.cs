using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EMD_SplashManager : MonoBehaviour
{
    public string MenuScene;
    public void GoMainMenu()
    {
        SceneManager.LoadScene(MenuScene);
    }
}
