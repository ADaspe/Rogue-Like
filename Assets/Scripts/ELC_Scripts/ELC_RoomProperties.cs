using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class ELC_RoomProperties : MonoBehaviour
{
    public bool openLeftDoor;
    public bool openRightDoor;
    public bool openTopDoor;
    public bool openDownDoor;

    public bool RoomIsClosed;
    public bool HasBeenInitialised;

    public bool isAnAngleRoom; //Si c'est une room qui correspond aux rooms aléatoires du début, ce sont les seules qui permettent de relier avec la ligne d'au dessus

    public bool thereIsRoom;

    public float DoorPlayerDetectionDistance;

    public Vector2 positionNumber;

    public GameObject roomObject, corridorsObject, areaObject;
    public GameObject LeftDoor, RightDoor, TopDoor, DownDoor;
    public GameObject LeftCorridor, RightCorridor, TopCorridor, DownCorridor;

    private List<GameObject> doors = new List<GameObject>(); //Liste des portes : 0 = Left, 1 = Right, 2 = Top, 3 = Down
    private List<bool> openSides = new List<bool>(); //Liste des cotés ouverts : 0 = Left, 1 = Right, 2 = Top, 3 = Down


    private void Update()
    {
        if (thereIsRoom)
        {
            if(!HasBeenInitialised) Initialisation();


            if (areaObject.GetComponent<ELC_Detector>().playerIsInside) DoorsState(true);

            if (Input.GetKeyDown(KeyCode.F)) DoorsState(!RoomIsClosed);
        }
    }

    void DoorsState(bool close) //Permet d'ouvrir/fermer les portes qui ont un couloir
    {
        for (int i = 0; i < doors.Count; i++)
        {
            if (openSides[i]) doors[i].SetActive(close);
        }

        RoomIsClosed = close;
    }
    
    void Initialisation()
    {
        roomObject = this.transform.GetChild(0).gameObject;
        corridorsObject = roomObject.transform.Find("Corridors").gameObject;
        areaObject = roomObject.transform.Find("Area").gameObject;
        

        LeftDoor = corridorsObject.transform.Find("Left Door").gameObject;
        RightDoor = corridorsObject.transform.Find("Right Door").gameObject;
        TopDoor = corridorsObject.transform.Find("Top Door").gameObject;
        DownDoor = corridorsObject.transform.Find("Down Door").gameObject;

        LeftCorridor = corridorsObject.transform.Find("Left Corridor").gameObject;
        RightCorridor = corridorsObject.transform.Find("Right Corridor").gameObject;
        TopCorridor = corridorsObject.transform.Find("Top Corridor").gameObject;
        DownCorridor = corridorsObject.transform.Find("Down Corridor").gameObject;



        doors.Add(LeftDoor);
        doors.Add(RightDoor);
        doors.Add(TopDoor);
        doors.Add(DownDoor);


        HasBeenInitialised = true;
    }

    public void UpdateCorridors()
    {
        if(openLeftDoor)
        {
            LeftCorridor.SetActive(true);
        }
        else
        {
            LeftCorridor.SetActive(false);
        }

        if(openRightDoor)
        {
            RightCorridor.SetActive(true);
        }
        else
        {
            RightCorridor.SetActive(false);
        }

        if(openTopDoor)
        {
            TopCorridor.SetActive(true);
        }
        else
        {
            TopCorridor.SetActive(false);
        }

        if(openDownDoor)
        {
            DownCorridor.SetActive(true);
        }
        else
        {
            DownCorridor.SetActive(false);
        }

        openSides.Add(openLeftDoor);
        openSides.Add(openRightDoor);
        openSides.Add(openTopDoor);
        openSides.Add(openDownDoor);

        DoorsState(false);

    }
}
