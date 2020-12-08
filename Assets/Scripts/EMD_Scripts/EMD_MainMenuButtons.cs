using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EMD_MainMenuButtons : MonoBehaviour
{
    public string GameScene;
    public string OptionsScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void PlayOptions()
    {
        SceneManager.LoadScene(OptionsScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
