using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PickupItems : MonoBehaviour
{
    private ELC_ObjectsInventory ObjectsInv;
    private ELC_PowerUpManager PUManager;
    private ELC_Detector detector;


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


    private void Start()
    {
        
        detector = this.gameObject.GetComponent<ELC_Detector>();

        if(chooseRandomPowerUp)
        {
            int randomNumber = Random.Range(0, PowerUpsList.Count);
            PowerUp = PowerUpsList[randomNumber];
            this.GetComponent<SpriteRenderer>().sprite = PowerUp.GetComponent<ELC_PowerUpProperties>().PowerUpSO.HUDSprite;

        }
        if(chooseRandomObject)
        {
            int randomNumber = Random.Range(0, ObjectsList.Count);
            Object = ObjectsList[randomNumber];
        }
        if(chooseRandomPassive)
        {
            int randomNumber = Random.Range(0, passivesList.Count);
            Passive = passivesList[randomNumber];
        }
    }

    private void Update()
    {
        if(ObjectsInv == null) ObjectsInv = GameObject.Find("PlayerInventory").GetComponent<ELC_ObjectsInventory>();
        if(PUManager == null) PUManager = GameObject.Find("PowerUpsManager").GetComponent<ELC_PowerUpManager>();
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
}
