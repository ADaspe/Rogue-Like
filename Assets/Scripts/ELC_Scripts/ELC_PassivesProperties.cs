using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PassivesProperties : MonoBehaviour
{
    public ELC_PassiveSO ActualPassiveScriptableObject;

    public AXD_Attack AttackScript;
    public ELC_ObjectsInventory InventoryScript;

    private ELC_PlayerStatManager statsManager;

    

    [Header ("Appetit de Lycaon")]
    public float LycaonMainEnemyHeal;
    public float LycaonCollateralEnemyHeal;

    [Header("Fil D'Ares")]
    public float FilAresBerserkMultiplicator;
    public float FilAresDamageTakenMultiplicator;

    private void Start()
    {
        if (ActualPassiveScriptableObject != null)
        {
            UpdateValues();
            InventoryScript.UpdateDisplay();
        }

        statsManager = FindObjectOfType<ELC_PlayerStatManager>();
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
        if (ActualPassiveScriptableObject.PassiveName == "Appétit De Lycaon") AppetitDeLycaon();
        else if (ActualPassiveScriptableObject.PassiveName == "Fil D'Ares") FilAres();
    }

    private void AppetitDeLycaon()
    {
        AttackScript.AppetitDeLycaonIsActive = true;
        AttackScript.AppetitDeLycaonHealPerEnemies = LycaonMainEnemyHeal;
        AttackScript.AppetitDeLycaonHealPerCollateral = LycaonCollateralEnemyHeal;
    }

    private void FilAres()
    {
        statsManager.losingLife = false;
        statsManager.FilAresBerserkMultiplicator = FilAresBerserkMultiplicator;
        statsManager.FilAresDamagesTakenMultiplicator = FilAresDamageTakenMultiplicator;
    }


}
