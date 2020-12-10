using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PassivesProperties : MonoBehaviour
{
    public ELC_PassiveSO ActualPassiveScriptableObject;

    public AXD_Attack AttackScript;
    public ELC_ObjectsInventory InventoryScript;


    public float LycaonMainEnemyHeal;
    public float LycaonCollateralEnemyHeal;

    private void Start()
    {
        if (ActualPassiveScriptableObject != null)
        {
            UpdateValues();
            InventoryScript.UpdateDisplay();
        }
    }

    private void Update()
    {
        if (ELC_ObjectsInventory.ActivePassif != null)
        {
            if ((ActualPassiveScriptableObject == null && ELC_ObjectsInventory.ActivePassif != null) || (ActualPassiveScriptableObject.PassiveName != ELC_ObjectsInventory.ActivePassif.PassiveName)) //Si j'ai pas de scriptable object mais que la valeur ActivePassif en a OU Si j'ai un scriptable object d'enregistré mais que celui dans Actual passif est différent du mien
            {
                ActualPassiveScriptableObject = ELC_ObjectsInventory.ActivePassif; //Je prend le scriptable Object actuel;
                UpdateValues();
                InventoryScript.UpdateDisplay();
            }
        }
    }

    private void UpdateValues() //Se lance une seule fois après l'obtention d'un passif un changement de passif
    {
        if(ActualPassiveScriptableObject.PassiveName == "Appétit De Lycaon") AppetitDeLycaon();
    }

    private void AppetitDeLycaon()
    {
        AttackScript.AppetitDeLycaonIsActive = true;
        AttackScript.AppetitDeLycaonHealPerEnemies = LycaonMainEnemyHeal;
        AttackScript.AppetitDeLycaonHealPerCollateral = LycaonCollateralEnemyHeal;
    }



}
