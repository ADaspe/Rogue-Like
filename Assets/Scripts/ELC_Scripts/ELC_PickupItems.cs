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


    private void Start()
    {
        ObjectsInv = GameObject.Find("PlayerInventory").GetComponent<ELC_ObjectsInventory>();
        PUManager = GameObject.Find("PowerUpsManager").GetComponent<ELC_PowerUpManager>();
        detector = this.gameObject.GetComponent<ELC_Detector>();
    }

    private void Update()
    {
        if (detector.playerIsInside && this.gameObject.CompareTag("Collectible") && Input.GetButtonDown("Interact"))
        {
            if(Type == CollectibleTypes.Object) ObjectsInv.AddObject(Object, Object.GetComponent<ELC_ObjectsUse>().ObjectsScriptableObject.quantity);

            if (Type == CollectibleTypes.PowerUp) PUManager.AddPowerUp(PowerUp);

            if (Type == CollectibleTypes.Passive) ELC_ObjectsInventory.ActivePassif = Passive;
        }
        
    }
}
