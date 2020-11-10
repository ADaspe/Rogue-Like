using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EMD_MainMenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("EMD_GameScene");
    }

    public void PlayOptions()
    {
        SceneManager.LoadScene("EMD_OptionScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
