using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EMD_PassifManager : MonoBehaviour
{
    bool IsDone = false;
    public List<ELC_PassiveSO> ListPassive = new List<ELC_PassiveSO>();
    ELC_PassiveSO[] passive = new ELC_PassiveSO[3];
    public ELC_PassiveSO SelectedPassive;
    public AXD_PlayerMoney currentMoneyScript;
    public GameObject ContinueButton;
    public GameObject PassiveButton;
    public GameObject PassiveCanvas;
    public GameObject ValidateButton;
    public GameObject DialogueCanvas;
    public GameObject QuittButton;
    public ELC_PassivesList PassivesList;
    public TextMeshProUGUI PassivePrice;
    public TextMeshProUGUI textDisplay;
    private EMD_DialogueManager DialogueManagerScript;
    private ELC_PassivesList PassiveListScript;
    //public GameObject passif1;
    public Image image1;
    public Image image2;
    public Image image3;


    void Start()
    {
        RandomPick();
        DialogueManagerScript = FindObjectOfType<EMD_DialogueManager>();
        PassiveListScript = FindObjectOfType<ELC_PassivesList>();
    }

    /*void takePassiveAndCreateArray()
    {
        passive = new ELC_PassiveSO[3];

        for (int i = 0; i < nbOfPassives + 1; i++)
        {
            bool isAlreadyThere = false;
            do
            {
                isAlreadyThere = false;
                int rdInt = Random.Range(0, nbOfPassives - 1);
                print(rdInt);

                foreach (ELC_PassiveSO passive in passive)
                {
                    if (passive == ListPassive[rdInt])
                    {
                        isAlreadyThere = true;
                    }
                }
                print(rdInt + " " + ListPassive[rdInt]);
                passive[i] = ListPassive[rdInt];
            } while (isAlreadyThere);
        }*/
    void RandomPick()
    {
        for (int i = 0; i < 3; i++)
        {
            int Index = Random.Range(0, PassivesList.PassivesList.Count);
            if (i == 0)
            {
                passive[i] = PassivesList.PassivesList[Index];
            }
            else if (i == 1)
            {
                passive[i] = PassivesList.PassivesList[Index];
                if (passive[i] == passive[i-1])
                {
                    i --;
                }
            }
           else
            {
                passive[i] = PassivesList.PassivesList[Index]; 
                if (passive[i] == passive[i-1] || passive[i] == passive[i-2])
                {
                    i --;
                }
            }
        }
    }

    public void afficherPassif()
    {
        image1.sprite = passive[0].HUDSprite;
        image2.sprite = passive[1].HUDSprite;
        image3.sprite = passive[2].HUDSprite;
        PassiveButton.SetActive(false);
        ContinueButton.SetActive(false);
        PassiveCanvas.SetActive(true);
        ValidateButton.SetActive(true);        
    }

    public void Selected1()
    {
        if (DialogueManagerScript.IsWriting == false)
        {
            SelectedPassive = passive[0];
            PassivePrice.text = "Prix : " + passive[0].PassivePrice.ToString();
        }
    }
    public void Selected2()
    {
        if (DialogueManagerScript.IsWriting == false)
        {
            SelectedPassive = passive[1];
            PassivePrice.text = "Prix : " + passive[1].PassivePrice.ToString();
        }
    }
    public void Selected3()
    {
        if (DialogueManagerScript.IsWriting == false)
        {
            SelectedPassive = passive[2];
            PassivePrice.text = "Prix : " + passive[2].PassivePrice.ToString();
        }
    }

    public void ChoosenPassive()
    {
        if (currentMoneyScript.currentMoney >= (int)SelectedPassive.PassivePrice)
        {
            currentMoneyScript.currentMoney -= (int)SelectedPassive.PassivePrice;
            ELC_ObjectsInventory.ActivePassif = SelectedPassive;
        }
        else
        {
            textDisplay.text = null;
            textDisplay.text = "Vous n'avez pas assez d'argent";
        }
    }
}

