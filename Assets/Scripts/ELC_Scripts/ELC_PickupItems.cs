using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PickupItems : MonoBehaviour
{
    private ELC_ObjectsInventory ObjectsInv;
    private ELC_PowerUpManager PUManager;
    private ELC_Detector detector;

    public AudioSource powerUpSound;

    public GameObject Object;
    public GameObject PowerUp;
    public ELC_PassiveSO Passive;


    public enum CollectibleTypes {Object, PowerUp, Passive};
    public CollectibleTypes Type = CollectibleTypes.Object;

    public bool chooseRandomPowerUp;
    public List<GameObject> PowerUpsList = null;

    public bool chooseRandomObject;
    public List<GameObject> ObjectsList = null;

    public bool chooseRandomPassive;
    public List<ELC_PassiveSO> passivesList = null;

    private int commonPercentageChanceObject = 100;
    private int rarePercentageChanceObject = 50;
    private int epicPercentageChanceObject = 30;

    public GameObject displayer;


    private void Start()
    {
        
        detector = this.gameObject.GetComponent<ELC_Detector>();

        if(chooseRandomPowerUp)
        {
            int randomNumber = Random.Range(0, PowerUpsList.Count);
            PowerUp = PowerUpsList[randomNumber];
            this.GetComponent<SpriteRenderer>().sprite = PowerUp.GetComponent<ELC_PowerUpProperties>().PowerUpSO.GroundSprite;

        }
        if(chooseRandomObject)
        {
            chooseRandomObjectFunction();
            this.GetComponent<SpriteRenderer>().sprite = Object.GetComponent<ELC_CrateProperties>().CratesSO.GroundSprite;
            displayer.GetComponent<ELC_Display>().objectInstatiate.GetComponent<SpriteRenderer>().sprite = Object.GetComponent<ELC_CrateProperties>().CratesSO.InfoPannel;
        }
        if(chooseRandomPassive)
        {
            int randomNumber = Random.Range(0, passivesList.Count);
            Passive = passivesList[randomNumber];
        }
    }

    private void chooseRandomObjectFunction()
    {
        int randomNumber = Random.Range(0, ObjectsList.Count);
        if (ObjectsList[randomNumber].GetComponent<ELC_CrateProperties>().CratesSO.spawnFrequency.ToString() == "Common")
        {
            if (Random.Range(0, 101) <= commonPercentageChanceObject) Object = ObjectsList[randomNumber];
            else chooseRandomObjectFunction();
        }
        else if (ObjectsList[randomNumber].GetComponent<ELC_CrateProperties>().CratesSO.spawnFrequency.ToString() == "Rare")
        {
            if (Random.Range(0, 101) <= rarePercentageChanceObject) Object = ObjectsList[randomNumber];
            else chooseRandomObjectFunction();
        }
        else
        {
            if (Random.Range(0, 101) <= epicPercentageChanceObject) Object = ObjectsList[randomNumber];
            else chooseRandomObjectFunction();
        }
    }

    private void Update()
    {
        if(ObjectsInv == null) ObjectsInv = GameObject.Find("PlayerInventory").GetComponent<ELC_ObjectsInventory>();
        if(PUManager == null) PUManager = GameObject.Find("PowerUpsManager").GetComponent<ELC_PowerUpManager>();
        if (detector.playerIsInside && this.gameObject.CompareTag("Collectible") && Type == CollectibleTypes.PowerUp)
        { 
            PUManager.AddPowerUp(PowerUp);
            StartCoroutine(Audio());
        }
        if (detector.playerIsInside && this.gameObject.CompareTag("Collectible") && Input.GetButtonDown("Interact"))
        {
            if(Type == CollectibleTypes.Object) ObjectsInv.AddObject(Object);

            if (Type == CollectibleTypes.PowerUp)
            {
                PUManager.AddPowerUp(PowerUp);
            }

            if (Type == CollectibleTypes.Passive) ELC_ObjectsInventory.ActivePassif = Passive;

            Destroy(this.gameObject);
        }
        
    }
    IEnumerator Audio()
    {
        if (!powerUpSound.isPlaying)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            powerUpSound.Play();
            yield return new WaitForSeconds(0.4f);
            Destroy(this.gameObject);
        }
    }
}
