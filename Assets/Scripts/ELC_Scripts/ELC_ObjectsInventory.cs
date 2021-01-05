using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ELC_ObjectsInventory : MonoBehaviour
{
    public enum hands { LeftHand, RightHand};
    public hands selectedHand = hands.LeftHand;
    public GameObject RightHandObject;
    public GameObject RightHandHUD;
    //public int quantityObject1;

    public GameObject LeftHandObject;
    public GameObject LeftHandHUD;
    //public int quantityObject2;

    public GameObject PassiveHUD;

    private GameObject player;

    
    static public ELC_PassiveSO ActivePassif;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetButtonDown("RightHandUse")) selectedHand = hands.RightHand;
        else if (Input.GetButtonDown("LeftHandUse")) selectedHand = hands.LeftHand;

        //if (Input.GetButtonDown("RightHandUse") && RightHandObject != null && quantityObject1 > 0) //Lorsqu'on clique pour utiliser l'object de main droite
        //{
        //    GameObject InstantiatedObject = Instantiate(RightHandObject, player.transform.position, Quaternion.identity);
        //    InstantiatedObject.GetComponent<ELC_ObjectsUse>().StartCoroutine("Use");
        //    quantityObject1--;
        //    UpdateDisplay();
        //}
        //else if(Input.GetButtonDown("LeftHandUse") && LeftHandObject != null && quantityObject2 > 0)//Lorsqu'on clique pour utiliser l'object de main gauche
        //{
        //    GameObject InstantiatedObject = Instantiate(LeftHandObject, player.transform.position, Quaternion.identity);
        //    InstantiatedObject.GetComponent<ELC_ObjectsUse>().StartCoroutine("Use");
        //    quantityObject2--;
        //    UpdateDisplay();
        //}
        
    }

    public void UpdateDisplay()
    {
        if (RightHandObject != null)
        {
            RightHandHUD.GetComponent<Image>().enabled = true;
            RightHandHUD.GetComponent<Image>().sprite = RightHandObject.GetComponent<ELC_CrateProperties>().CratesSO.HUDSprite;
        }
        else RightHandHUD.GetComponent<Image>().enabled = false;

        if (LeftHandObject != null)
        {
            LeftHandHUD.GetComponent<Image>().enabled = true;
            LeftHandHUD.GetComponent<Image>().sprite = LeftHandObject.GetComponent<ELC_CrateProperties>().CratesSO.HUDSprite;
        }
        else LeftHandHUD.GetComponent<Image>().enabled = false;

        if (ActivePassif != null)
        {
            PassiveHUD.GetComponent<Image>().enabled = true;
            PassiveHUD.GetComponent<Image>().sprite = ActivePassif.HUDSprite;
        }
        else PassiveHUD.GetComponent<Image>().enabled = false;
    }

    public void AddObject(GameObject Object)
    {
        if (selectedHand == hands.RightHand)
        {
            RightHandObject = InstantiateCrates(Object);
            //quantityObject1 = quantity;
            UpdateDisplay();
        }
        else if (selectedHand == hands.LeftHand)
        {
            LeftHandObject = InstantiateCrates(Object);
            //quantityObject2 = quantity;
            UpdateDisplay();
        }
    }

    public void AddMoneyToCrates(int money)
    {
        ELC_CrateProperties leftCrateProp; 
        ELC_CrateProperties rightCrateProp;

        //if (LeftHandObject != null && RightHandObject != null)
        //{
        //    leftCrateProp = LeftHandObject.GetComponent<ELC_CrateProperties>();
        //    rightCrateProp = RightHandObject.GetComponent<ELC_CrateProperties>();

        //    int remainingMoneyToStock = money;

        //    if (leftCrateProp.actualMoneyStockCapacity >= Mathf.CeilToInt(money / 2))
        //    {
        //        leftCrateProp.actualMoney += Mathf.CeilToInt(money / 2);
        //        remainingMoneyToStock -= Mathf.CeilToInt(money / 2);
        //    }
        //    else
        //    {
        //        leftCrateProp.actualMoney += leftCrateProp.actualMoneyStockCapacity;
        //        remainingMoneyToStock -= leftCrateProp.actualMoneyStockCapacity;
        //    }

        //    if (rightCrateProp.actualMoneyStockCapacity >= remainingMoneyToStock) rightCrateProp.actualMoney += remainingMoneyToStock;
        //    else rightCrateProp.actualMoney += rightCrateProp.actualMoneyStockCapacity;
        //    Debug.Log("Caisse gauche = " + leftCrateProp.actualMoney);
        //    Debug.Log("Caisse droite = " + rightCrateProp.actualMoney);
        //}
        //else if (LeftHandObject != null)
        //{
        //    leftCrateProp = LeftHandObject.GetComponent<ELC_CrateProperties>();
        //    if (leftCrateProp.actualMoneyStockCapacity >= money) leftCrateProp.actualMoney += money;
        //    else leftCrateProp.actualMoney += leftCrateProp.actualMoneyStockCapacity;
        //    Debug.Log("Caisse gauche = " + leftCrateProp.actualMoney);
        //}
        //else if (RightHandObject != null)
        //{
        
        //}

        if(RightHandObject != null && (selectedHand == hands.RightHand || LeftHandObject == null))
        {
            rightCrateProp = RightHandObject.GetComponent<ELC_CrateProperties>();
            if (rightCrateProp.actualMoneyStockCapacity >= money) rightCrateProp.actualMoney += money;
            else
            {
                rightCrateProp.actualMoney += rightCrateProp.actualMoneyStockCapacity;
                Debug.Log("Caisse gauche full !");
            }
            Debug.Log("Caisse droite = " + rightCrateProp.actualMoney);
        }
        else if(LeftHandObject != null && (selectedHand == hands.LeftHand || RightHandObject == null))
        {
            leftCrateProp = LeftHandObject.GetComponent<ELC_CrateProperties>();
            if (leftCrateProp.actualMoneyStockCapacity >= money) leftCrateProp.actualMoney += money;
            else
            {
                leftCrateProp.actualMoney += leftCrateProp.actualMoneyStockCapacity;
                Debug.Log("Caisse gauche full !");
            }
            Debug.Log("Caisse gauche = " + leftCrateProp.actualMoney);
        }
        else Debug.Log("Pas de caisses pour stocker l'argent");
    }

    public void GetHitCrates()
    {
        if (LeftHandObject != null) LeftHandObject.GetComponent<ELC_CrateProperties>().HitCrate();
        if (RightHandObject != null) RightHandObject.GetComponent<ELC_CrateProperties>().HitCrate();
    }

    public GameObject InstantiateCrates(GameObject prefab)
    {
        GameObject GO = null;
        GO = new GameObject();
        GO.AddComponent<ELC_CrateProperties>();
        GO.GetComponent<ELC_CrateProperties>().CratesSO = prefab.GetComponent<ELC_CrateProperties>().CratesSO;
        GO.name = GO.GetComponent<ELC_CrateProperties>().CratesSO.Name;
        GO.transform.SetParent(this.transform);
        return GO;
    }
}
