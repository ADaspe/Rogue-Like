using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EMD_GameOver : MonoBehaviour
{
    public GameObject GameOverGO;
    public GameObject MonsterWhoKilledGO;
    public GameObject RunTimeGO;
    public GameObject PlusGrosComboGO;
    public GameObject ThunesGO;
    public GameObject PassifUsedGO;
    public TextMeshProUGUI GameOver;
    public TextMeshProUGUI MonsterWhoKilled;
    public TextMeshProUGUI RunTime;
    public TextMeshProUGUI PlusGrosCombo;
    public TextMeshProUGUI Thunes;
    public TextMeshProUGUI PassifUsed;
    int length = 6;
    public float DelayTime;

    // Start is called before the first frame update

    public string HUB;
    public string MainMenu;
  
    private void Start()
    {
        StartCoroutine("OneByOne");
    }

    IEnumerator OneByOne()
    {
        for (int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                GameOverGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 1)
            {
                //MonsterWhoKilled.text = "TUE PAR: " + PlayerHealth.lastHitEnnemy.Name;
                MonsterWhoKilledGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 2)
            {
                //RunTime.text = "TEMPS DE LA RUN:    " + Timer;
                RunTimeGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 3)
            {
                //PluGrosCombo.text = "LE PLUS GROS COMBO:    " + la variable du combo;
                PlusGrosComboGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 4)
            {
                //Thunes.text = "ARGENT RECOLTE:    " + L'argent des caisses;
                ThunesGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 5)
            {
                PassifUsed.text = "PASSIF UTILISE : " + ELC_ObjectsInventory.ActivePassif;
                PassifUsedGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
        }
    }

    public void GoHUB()
    {
        SceneManager.LoadScene(HUB);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }
}
