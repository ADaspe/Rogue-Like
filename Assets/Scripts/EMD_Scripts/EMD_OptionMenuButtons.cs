using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EMD_OptionMenuButtons : MonoBehaviour
{
    public void ReturnMenu()
    {
        SceneManager.LoadScene("EMD_MenuScene");
    }

}
