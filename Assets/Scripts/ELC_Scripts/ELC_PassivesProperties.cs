using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PassivesProperties : MonoBehaviour
{
    public ELC_PassiveSO ActualPassiveScriptableObject;

    public AXD_Attack AttackScript;
    public ELC_ObjectsInventory InventoryScript;
    ELC_PlayerMoves playerMovesScript;

    private ELC_PlayerStatManager statsManager;

    

    [Header ("Appetit de Lycaon")]
    public float LycaonMainEnemyHeal;
    public float LycaonCollateralEnemyHeal;

    [Header("Fil D'Ares")]
    public float FilAresBerserkMultiplicator;
    public float FilAresDamageTakenMultiplicator;


    [Header("Don D'Atalante")]
    public float AtalanteMaxSpeedAdd;

    [Header("Bottes D'Hermes")]
    public float HermesKnockback;
    public float HermesStunTime;

    [Header("Corne D'Abondance")]
    public int CorneAbondancePercentageChanceDropPowerUp;
    public GameObject PowerUpsGenerator;

    [Header("Sang De Gorgonne")]
    public float GorgonneStrengthBonusMultiplicator;

    [Header("Faux De Chronos")]
    public float ChronosStopPowerUpFlowDuration; //Cb de temps les power ups vont arrêter de découler

    [Header("Egide")]
    public float EgidePercentageChanceToSendBackProjectile;




    private void Start()
    {
        playerMovesScript = FindObjectOfType<ELC_PlayerMoves>();
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
        else if (ActualPassiveScriptableObject.PassiveName == "Don D'Atalante") DonAtalante();
        else if (ActualPassiveScriptableObject.PassiveName == "Bottes D'Hermes") BottesHermes();
        else if (ActualPassiveScriptableObject.PassiveName == "Faux De Chronos") FauxDeChronos();
        else if (ActualPassiveScriptableObject.PassiveName == "Egide") Egide();
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

    private void DonAtalante()
    {
        playerMovesScript.DonAtalante = true;
        playerMovesScript.DonAtalanteSpeedAdd = AtalanteMaxSpeedAdd;
    }

    private void BottesHermes()
    {
        playerMovesScript.BottesHermes = true;
    }

    private void FauxDeChronos()
    {
        FindObjectOfType<ELC_PowerUpManager>().FauxDeChronos = true;
    }

    private void Egide()
    {
        AttackScript.Egide = true;
    }

}
