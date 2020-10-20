using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ELC_RoomsGenerator : MonoBehaviour
{
    public int arrayDimentionX;
    public int arrayDimentionY;


    public int distanceBtwRoomsX;
    public int distanceBtwRoomsY;

    public Vector2 entryPosition;

    public GameObject roomChecker;

    public List<GameObject> roomsList;

    public GameObject[,] checkersArray;
    public GameObject[,] roomsArray;


    
    void Start()
    {
        checkersArray = new GameObject[arrayDimentionX, arrayDimentionY];
        roomsArray = new GameObject[arrayDimentionX, arrayDimentionY];

        for (int i = 0; i < arrayDimentionX; i++)
        {
            for (int e = 0; e < arrayDimentionY; e++)
            {
                GameObject go = GameObject.Instantiate(roomChecker, new Vector3(i * distanceBtwRoomsX, -e * distanceBtwRoomsY), Quaternion.identity);
                checkersArray[i, e] = go;
            }
        }
        SpawnRandomRoom(checkersArray[(int)entryPosition.x, (int)entryPosition.y]);
    }



    private void SpawnRandomRoom(GameObject checkerObject)
    {
        int randomNumber = Random.Range(0, roomsList.Count);
        GameObject.Instantiate(roomsList[randomNumber], checkerObject.transform);
    }
}
