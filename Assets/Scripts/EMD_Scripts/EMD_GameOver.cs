using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class EMD_GameOver : MonoBehaviour
{
    public TextMeshProUGUI GameOver;
    public TextMeshProUGUI MonsterWhoKilled;
    public TextMeshProUGUI RunTime;
    public TextMeshProUGUI PlusGrosCombo;
    public TextMeshProUGUI Thunes;
    public TextMeshProUGUI PassifUsed;

    // Start is called before the first frame update

    public string HUB;
    public string MainMenu;

    private void Start()
    {
        //met le GameOver
        Thread.Sleep(3000);
        _MonsterWhoKilled();
        Thread.Sleep(3000);
        _RunTime();
        Thread.Sleep(3000);
        _PlusGrosCombo();
        Thread.Sleep(3000);
        _Thunes();
        Thread.Sleep(3000);
        _PassifUsed();
    }

    public void GoHUB()
    {
        SceneManager.LoadScene(HUB);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }

    public void _PassifUsed()
    {
        PassifUsed.text = "PASSIF UTILISE " + ELC_ObjectsInventory.ActivePassif;
        //l'afficher
    }
    public void _MonsterWhoKilled()
    {
        //MonsterWhoKilled.text = "TUE PAR: " + DernierMonstre;
        //l'afficher
    }
    public void _RunTime()
    {
        //RunTime.text = "TEMPS DE LA RUN:    " + la variable du timer;
        //l'afficher
    }
    public void _PlusGrosCombo()
    {
        //PluGrosCombo.text = "LE PLUS GROS COMBO:    " + la variable du combo;
        //l'afficher
    }
    public void _Thunes()
    {
        //Thunes.text = "ARGENT RECOLTE:    " + L'argent des caisses;
        //l'afficher
    }
}
