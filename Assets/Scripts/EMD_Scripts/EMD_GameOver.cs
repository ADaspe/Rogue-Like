using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EMD_GameOver : MonoBehaviour
{
    public GameObject GOCanvas;
    public GameObject GameOverGO;
    public GameObject MonsterWhoKilledGO;
    public GameObject RunTimeGO;
    public GameObject PlusGrosComboGO;
    public GameObject ThunesGO;
    public GameObject PassifUsedGO;
    public GameObject AchGO;
    public TextMeshProUGUI GameOver;
    public TextMeshProUGUI MonsterWhoKilled;
    public TextMeshProUGUI RunTime;
    public TextMeshProUGUI PlusGrosCombo;
    public TextMeshProUGUI Thunes;
    public TextMeshProUGUI PassifUsed;
    int length = 8;
    public float DelayTime;
    private ELC_PlayerStatManager PlayerStatManagerScript;
    private PlayerHealth PlayerHealthScript;
    private AXD_AchievementManager AchievementManagerScript;
    private ELC_ObjectsInventory ObjectinventoryScript;
    private AXD_Hydra HydraScript;
    public int MoneyHarvested;
    int TimerMin;
    int TimerSec;

    public string HUB;
    public string MainMenu;
  
    private void Start()
    {
        PlayerStatManagerScript = FindObjectOfType<ELC_PlayerStatManager>();
        AchievementManagerScript = FindObjectOfType<AXD_AchievementManager>();
        ObjectinventoryScript = FindObjectOfType<ELC_ObjectsInventory>();
        HydraScript = FindObjectOfType<AXD_Hydra>();
        GameOverGO.SetActive(false);
        MonsterWhoKilledGO.SetActive(false);
        RunTimeGO.SetActive(false);
        PlusGrosComboGO.SetActive(false);
        ThunesGO.SetActive(false);
        PassifUsedGO.SetActive(false);
        AchGO.SetActive(false);
        GOCanvas.SetActive(false);
    }
    private void Update()
    {
        if (HydraScript.EndMenuTime == true)
        {
            StartCoroutine("OneByOne");
        }
    }

    private void OnEnable()
    {
        StartCoroutine("OneByOne");
    }
    IEnumerator OneByOne()
    {
        for (int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                GOCanvas.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 1)
            {
                GameOverGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 2)
            {
                if (HydraScript.EndMenuTime == true)
                {
                    MonsterWhoKilled.text = "VOUS AVEZ TUE LE BOSS";
                }
                else
                {
                    MonsterWhoKilled.text = "TUE PAR: " + PlayerHealth.lastHitEnnemy.Name;
                }
                MonsterWhoKilledGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 3)
            {
                TimerMin = (int)PlayerStatManagerScript.gameTimer / 60;
                TimerSec = (int)PlayerStatManagerScript.gameTimer % 60;
                RunTime.text = "TEMPS DE LA RUN:    " + TimerMin + " min  " + TimerSec + " sec  ";
                RunTimeGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 4)
            {
                PlusGrosCombo.text = "LE PLUS GROS COMBO:    " + PlayerStatManagerScript.MaxRunCombo;
                PlusGrosComboGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 5)
            {
                if (ObjectinventoryScript.RightHandObject != null)
                {
                    MoneyHarvested = ObjectinventoryScript.RightHandObject.GetComponent<ELC_CrateProperties>().securedMoney + ObjectinventoryScript.LeftHandObject.GetComponent<ELC_CrateProperties>().securedMoney;
                }
                else
                {
                    MoneyHarvested = ObjectinventoryScript.LeftHandObject.GetComponent<ELC_CrateProperties>().securedMoney;
                }
                
                Thunes.text = "ARGENT RECOLTE:    " + MoneyHarvested;
                ThunesGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 6)
            {
                if (ELC_ObjectsInventory.ActivePassif == null)
                {
                    PassifUsed.text = "PAS DE PASSIF UTILISE";
                }
                else if (ELC_ObjectsInventory.ActivePassif != null)
                {
                    PassifUsed.text = "PASSIF UTILISE : " + ELC_ObjectsInventory.ActivePassif.PassiveName;
                }
                PassifUsedGO.SetActive(true);
                yield return new WaitForSeconds(DelayTime);
            }
            if (i == 7 && AchievementManagerScript.hasUnlockedAchievement == true)
            {
                AchGO.SetActive(true);
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
