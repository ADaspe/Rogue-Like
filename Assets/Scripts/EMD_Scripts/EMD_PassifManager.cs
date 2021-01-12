using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EMD_PassifManager : MonoBehaviour
{
    bool IsDone = false;
    public List<ELC_PassiveSO> ListPassive = new List<ELC_PassiveSO>();
    ELC_PassiveSO[] passifs = new ELC_PassiveSO[3];
    /*ELC_PassiveSO Passif1;
    ELC_PassiveSO Passif2;
    ELC_PassiveSO Passif3;*/
    ELC_PassiveSO ActualPassiveScriptableObject;
    public GameObject ContinueButton;
    public GameObject QuitButton;
    public GameObject PassifButton;
    public GameObject PassifCanvas;
    public TextMeshProUGUI textDisplay;

    public SpriteRenderer image1;
    public SpriteRenderer image2;
    public SpriteRenderer image3;
    int nbOfPassives = 3;


    void Start()
    {
        //RandomPick();
        takePassiveAndCreateArray();
        //afficherPassif();
        Debug.Log(passifs[0]);
        Debug.Log(passifs[1]);
        Debug.Log(passifs[2]);
    }

    void takePassiveAndCreateArray()
    {
        passifs = new ELC_PassiveSO[3];

        for (int i = 0; i < nbOfPassives + 1; i++)
        {
            bool isAlreadyThere = false;
            do
            {
                isAlreadyThere = false;
                int rdInt = Random.Range(0, nbOfPassives - 1);
                print(rdInt);

                foreach (ELC_PassiveSO passive in passifs)
                {
                    if (passive == ListPassive[rdInt])
                    {
                        isAlreadyThere = true;
                    }
                }
                print(rdInt + " " + ListPassive[rdInt]);
                passifs[i] = ListPassive[rdInt];
            } while (isAlreadyThere);
        }
    }
    void RandomPick()
    {
        /*int nombre = 0;
        while (IsDone == false)
        {
            int Index = Random.Range(0, ListPassive.Count + 1);
            passifs[nombre] = ListPassive[Index];
            
            nombre++;
        }
        for (int i = 0; i < 3; i++)
        {
            int Index = Random.Range(0, ListPassive.Count + 1);
            Debug.Log(i);
            if (i == 0)
            {
                
                passifs[i] = ListPassive[Index];
            }
            else if (i == 1)
            {
                passifs[i] = ListPassive[Index];
                if (passifs[i] == passifs[i-1])
                {
                    i --;
                }
            }
           else
            {
                passifs[i] = ListPassive[Index]; 
                if (passifs[i] == passifs[i-1] || passifs[i] == passifs[i-2])
                {
                    i --;
                }
            }
        }*/
    }

    public void afficherPassif()
    {
        //Afficher les images des passifs
        ContinueButton.SetActive(false);
        image1.sprite = passifs[0].HUDSprite;
        image2.sprite = passifs[1].HUDSprite;
        image3.sprite = passifs[2].HUDSprite;
        PassifCanvas.SetActive(true);
    }

        //Selon passif séléctionné: afficher le nom et le descriptif légé
        /*if (selected(jysépa))
        {
            
        }
    }
    public void PassifChoisie1()
    {
        PassifCanvas.SetActive(false);
        //gérer l'argent par rapport au prix du passif
        ActualPassiveScriptableObject = Passif1;
    }
    public void PassifChoisie2()
    {
        PassifCanvas.SetActive(false);
        //gérer l'argent par rapport au prix du passif
        ActualPassiveScriptableObject = Passif2;
    }
    public void PassifChoisie3()
    {
        PassifCanvas.SetActive(false);
        //gérer l'argent par rapport au prix du passif
        //remplacer par le passif choisi
        ActualPassiveScriptableObject = Passif3;
    }*/
}

