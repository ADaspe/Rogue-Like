using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EMD_PauseMenuButtons : MonoBehaviour
{
    public void ReturnMenu()
    {
        SceneManager.LoadScene("EMD_MenuScene");
    }
    public void PlayOptions()
    {
        SceneManager.LoadScene("EMD_OptionScene");
    }
}
