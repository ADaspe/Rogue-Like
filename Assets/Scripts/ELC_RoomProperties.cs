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

    public bool isAnAngleRoom; //Si c'est une room qui correspond aux rooms aléatoires du début, ce sont les seules qui permettent de relier avec la ligne d'au dessus

    public bool thereIsRoom;

    public Vector2 positionNumber;

    public GameObject roomObject, corridorsObject;
    public GameObject LeftDoor, RightDoor, TopDoor, DownDoor;
    public GameObject LeftCorridor, RightCorridor, TopCorridor, DownCorridor;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (thereIsRoom)
        {
            roomObject = this.transform.GetChild(0).gameObject;
            corridorsObject = roomObject.transform.Find("Corridors").gameObject;
            LeftDoor = corridorsObject.transform.Find("Left Door").gameObject;
            RightDoor = corridorsObject.transform.Find("Right Door").gameObject;
            TopDoor = corridorsObject.transform.Find("Top Door").gameObject;
            DownDoor = corridorsObject.transform.Find("Down Door").gameObject;

            LeftCorridor = corridorsObject.transform.Find("Left Corridor").gameObject;
            RightCorridor = corridorsObject.transform.Find("Right Corridor").gameObject;
            TopCorridor = corridorsObject.transform.Find("Top Corridor").gameObject;
            DownCorridor = corridorsObject.transform.Find("Down Corridor").gameObject;

            UpdateDoorsAndCorridors();
        }
    }

    public void UpdateDoorsAndCorridors()
    {
        if(openLeftDoor)
        {
            LeftDoor.SetActive(false);
            LeftCorridor.SetActive(true);
        }
        else
        {
            LeftDoor.SetActive(true);
            LeftCorridor.SetActive(false);
        }

        if(openRightDoor)
        {
            RightDoor.SetActive(false);
            RightCorridor.SetActive(true);
        }
        else
        {
            RightDoor.SetActive(true);
            RightCorridor.SetActive(false);
        }

        if(openTopDoor)
        {
            TopDoor.SetActive(false);
            TopCorridor.SetActive(true);
        }
        else
        {
            TopDoor.SetActive(true);
            TopCorridor.SetActive(false);
        }

        if(openDownDoor)
        {
            DownDoor.SetActive(false);
            DownCorridor.SetActive(true);
        }
        else
        {
            DownDoor.SetActive(true);
            DownCorridor.SetActive(false);
        }
    }
}
