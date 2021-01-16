using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ELC_RoomProperties : MonoBehaviour
{
    enum doorsDirections {Left, Right, Top, Down};
    doorsDirections directions = doorsDirections.Left;
    private ELC_RoomsGenerator generatorScript;

    public bool openLeftDoor;
    public bool openRightDoor;
    public bool openTopDoor;
    public bool openDownDoor;

    public bool IsStartRoom;
    public bool IsEndRoom;
    public bool RoomIsClosed;
    public bool HasBeenInitialised;
    public bool Loaded;

    public Tilemap tileMap;
    public List<RuleTile> WallTiles = null; //La 1ère wallTile est celle du toit et la seconde est le mur
    private Vector3Int actualTile;

    public int WavesLimit = 2;
    public int ActualWave;
    private bool roomIsClear;

    public bool isAnAngleRoom; //Si c'est une room qui correspond aux rooms aléatoires du début, ce sont les seules qui permettent de relier avec la ligne d'au dessus

    public bool thereIsRoom;

    public float DoorPlayerDetectionDistance;

    public Vector2 positionNumber;

    

    public GameObject roomObject, corridorsObject, areaObject;
    public GameObject LeftDoor, RightDoor, TopDoor, DownDoor;
    public GameObject LeftCorridor, RightCorridor, TopCorridor, DownCorridor;

    private List<GameObject> doors = new List<GameObject>(); //Liste des portes : 0 = Left, 1 = Right, 2 = Top, 3 = Down
    private List<bool> openSides = new List<bool>(); //Liste des cotés ouverts : 0 = Left, 1 = Right, 2 = Top, 3 = Down
    public List<GameObject> enemiesAlive = new List<GameObject>();
    public List<GameObject> enemiesGenerators = new List<GameObject>();
    public List<GameObject> powerUpsGenerators = new List<GameObject>();


    private void Start()
    {
        generatorScript = FindObjectOfType<ELC_RoomsGenerator>();
    }

    private void Update()
    {
        if (thereIsRoom)
        {
            if(!HasBeenInitialised) Initialisation();

            if(Loaded)
            {
                if (areaObject.GetComponent<ELC_Detector>().playerIsInside)
                {
                    PlayerEnterInRoom();
                }


                //if (Input.GetKeyDown(KeyCode.F)) DoorsState(!RoomIsClosed);
            }
        }
    }

    void DoorsState(bool close) //Permet d'ouvrir/fermer les portes qui ont un couloir
    {
        for (int i = 0; i < doors.Count ; i++)
        {
            if (openSides[i])
            {
                doors[i].transform.GetChild(0).gameObject.SetActive(false);
                doors[i].transform.GetChild(1).gameObject.SetActive(false);
                if (close) doors[i].transform.GetChild(2).gameObject.GetComponent<Collider2D>().enabled = true;
                else doors[i].transform.GetChild(2).gameObject.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(DoorsAnimations(doors[i].transform.GetChild(2).gameObject, close));
            }
            
        }

        RoomIsClosed = close;
    }

    IEnumerator DoorsAnimations(GameObject door, bool close)
    {
        if (close) 
        {
            door.GetComponent<Animator>().SetBool("isClosing", true);
            door.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            door.GetComponent<Animator>().SetBool("isOpening", true);
        }
        yield return new WaitForSeconds(0.75f);
        if (close) door.GetComponent<Animator>().SetBool("isClosing", false);
        else
        {
            door.GetComponent<Animator>().SetBool("isOpening", false);
            door.GetComponent<SpriteRenderer>().enabled = false;
        }


    }
    
    void PlayerEnterInRoom()
    {
        if (ActualWave < WavesLimit) //Si on est à une vague en dessous de la limite de vagues qu'on veut
        {
            if (!RoomIsClosed) //On ferme les portes si elles ne l'étaient pas déjà
            {
                DoorsState(true);
                foreach (GameObject PUGenerators in powerUpsGenerators) PUGenerators.GetComponent<ELC_RandomObjectGenerator>().SpawnEntity();

            }

            if (enemiesAlive.Count == 0) //Si la List d'ennemis en vie est vide
            {
                foreach (GameObject enemiesGenerator in enemiesGenerators) enemiesGenerator.GetComponent<ELC_RandomObjectGenerator>().SpawnEntity();
                foreach (GameObject generators in enemiesGenerators)
                {
                    enemiesAlive.Add(generators.transform.GetChild(0).gameObject);
                }
                ActualWave++;
            }
            EnemiesCheck();
        }
        else
        {
            EnemiesCheck();
            if(enemiesAlive.Count == 0 && !roomIsClear)
            {
                Debug.Log("Room clear !");
                DoorsState(false);
                roomIsClear = true;
            }
        }

    }

    void EnemiesCheck() //Check s'il reste des ennemis dans la list
    {
        int remainingEnemies = enemiesAlive.Count;
        for (int i = 0; i < enemiesAlive.Count; i++)
        {
            if(enemiesGenerators[i].transform.childCount == 0)
            {
                remainingEnemies--;
                if(remainingEnemies == 0) enemiesAlive.Clear(); //Clear la list d'ennemis pour la remettre à 0
            }
        }

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

        enemiesGenerators = GetAllChilds(roomObject.transform.Find("EnemiesGenerators").gameObject);
        powerUpsGenerators = GetAllChilds(roomObject.transform.Find("PowerUps").gameObject);

        tileMap = this.transform.GetComponentInChildren<Tilemap>();

        doors.Add(LeftDoor);
        doors.Add(RightDoor);
        doors.Add(TopDoor);
        doors.Add(DownDoor);

        

        HasBeenInitialised = true;
    }

    private List<GameObject> GetAllChilds(GameObject parent)
    {
        List<GameObject> childsList = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            childsList.Add(parent.transform.GetChild(i).gameObject);
        }
        return childsList;
    } //récupérer tous les enfants d'un GameObject dans une list et la return

    public void UpdateCorridors()
    {
        //Ouvrir/fermer les corridors
        //if(openLeftDoor)
        //{
        //    LeftCorridor.SetActive(true);
        //}
        //else
        //{
        //    LeftCorridor.SetActive(false);
        //}

        //if(openRightDoor)
        //{
        //    RightCorridor.SetActive(true);
        //}
        //else
        //{
        //    RightCorridor.SetActive(false);
        //}

        //if(openTopDoor)
        //{
        //    TopCorridor.SetActive(true);
        //}
        //else
        //{
        //    TopCorridor.SetActive(false);
        //}

        //if(openDownDoor)
        //{
        //    DownCorridor.SetActive(true);
        //}
        //else
        //{
        //    DownCorridor.SetActive(false);
        //}

        openSides.Add(openLeftDoor);
        openSides.Add(openRightDoor);
        openSides.Add(openTopDoor);
        openSides.Add(openDownDoor);

        if (!openSides[0])
        {
            LeftDoor.SetActive(false);
            DrawWalls(LeftDoor);
        }
        else ReplaceDoors(LeftDoor, doorsDirections.Left);

        if (!openSides[1])
        {
            RightDoor.SetActive(false);
            DrawWalls(RightDoor);
        }
        else ReplaceDoors(RightDoor, doorsDirections.Right);

        if (!openSides[2])
        {
            TopDoor.SetActive(false);
            DrawWalls(TopDoor);
        }
        else ReplaceDoors(TopDoor, doorsDirections.Top);

        if (!openSides[3])
        {
            DownDoor.SetActive(false);
            DrawWalls(DownDoor);
        }
        else ReplaceDoors(DownDoor, doorsDirections.Down);



        DoorsState(false);

    }

    private void ReplaceDoors(GameObject doorToReplace, doorsDirections direction)
    {
        float frontDoorsHeight = 0.89f;
        float lateralDoorsPositionAdjustement = 0.5f;

        Vector3 newDoorPosition = Vector3.zero;

        GameObject doorFirstPart = doorToReplace.transform.GetChild(0).gameObject;
        GameObject doorSecondPart = doorToReplace.transform.GetChild(1).gameObject;

        if(direction == doorsDirections.Left || direction == doorsDirections.Right)
        {
            if(doorFirstPart.transform.position.y > doorSecondPart.transform.position.y)
            {
                newDoorPosition = new Vector3(doorFirstPart.transform.position.x, doorFirstPart.transform.position.y - Vector3.Distance(doorFirstPart.transform.position, doorSecondPart.transform.position) / 2);
            }
            else newDoorPosition = new Vector3(doorSecondPart.transform.position.x, doorSecondPart.transform.position.y - Vector3.Distance(doorFirstPart.transform.position, doorSecondPart.transform.position) / 2);
            newDoorPosition.y += lateralDoorsPositionAdjustement;
            Instantiate(generatorScript.LateralDoors, newDoorPosition ,Quaternion.identity, doorToReplace.transform);
            
        }
        else if(direction == doorsDirections.Top || direction == doorsDirections.Down)
        {
            if(doorFirstPart.transform.position.x > doorSecondPart.transform.position.x)
            {
                newDoorPosition = new Vector3(doorFirstPart.transform.position.x - Vector3.Distance(doorFirstPart.transform.position, doorSecondPart.transform.position) / 2, doorFirstPart.transform.position.y);
            }
            else newDoorPosition = new Vector3(doorSecondPart.transform.position.x - Vector3.Distance(doorFirstPart.transform.position, doorSecondPart.transform.position) / 2, doorFirstPart.transform.position.y);

            if (direction == doorsDirections.Down) frontDoorsHeight = -frontDoorsHeight + 0.15f;
            newDoorPosition.y += frontDoorsHeight;
            Instantiate(generatorScript.FrontDoors, newDoorPosition, Quaternion.identity, doorToReplace.transform);
        }

    }

    private void DrawWalls(GameObject door)
    {
        float xCoordinates = door.transform.GetChild(0).transform.position.x;
        float yCoordinates = door.transform.GetChild(0).transform.position.y;

        int length = 0; //Pour calculer la longueur horizontale du couloir

        if (door == LeftDoor) length = Mathf.FloorToInt(FindObjectOfType<ELC_RoomsGenerator>().distanceBtwRoomsX / 2 - (this.transform.position.x - xCoordinates)) + 1;
        else if (door == RightDoor) length = Mathf.FloorToInt(FindObjectOfType<ELC_RoomsGenerator>().distanceBtwRoomsX / 2 - (xCoordinates - this.transform.position.x)) + 1;
        else if (door == DownDoor) length = Mathf.FloorToInt(FindObjectOfType<ELC_RoomsGenerator>().distanceBtwRoomsY / 2 - (this.transform.position.y - yCoordinates)) + 1; //Pour calculer la longueur verticale du couloir
        else if (door == TopDoor) length = Mathf.FloorToInt(FindObjectOfType<ELC_RoomsGenerator>().distanceBtwRoomsY / 2 - (yCoordinates - this.transform.position.y)) + 1;

        int corridorWidth = 0;
        if (door == LeftDoor || door == RightDoor) corridorWidth = 4;
        else corridorWidth = 2;


        for (int i = 0; i < corridorWidth; i++)
        {
            if (door == LeftDoor || door == RightDoor)
            {
                if (door.transform.GetChild(0).transform.position.y < door.transform.GetChild(1).transform.position.y) actualTile = tileMap.WorldToCell(door.transform.GetChild(0).transform.position) + new Vector3Int(0, i, 0); //On cherche à prendre la porte la plus en bas comme point de départ du dessin
                else actualTile = tileMap.WorldToCell(door.transform.GetChild(1).transform.position) + new Vector3Int(0, i, 0);
            }
            else
            {
                if (door.transform.GetChild(0).transform.position.x < door.transform.GetChild(1).transform.position.x) actualTile = tileMap.WorldToCell(door.transform.GetChild(0).transform.position) + new Vector3Int(i, 0, 0); //On cherche à prendre la porte la plus en bas comme point de départ du dessin
                else actualTile = tileMap.WorldToCell(door.transform.GetChild(1).transform.position) + new Vector3Int(i, 0, 0);
            }

            for (int e = 0; e < length; e++)
            {
                int tileIndex = 0;
                if (door == TopDoor && e < 2) tileIndex = 1;
                
                tileMap.SetTile(actualTile, WallTiles[tileIndex]);

                if (door == LeftDoor) actualTile.x--;
                else if (door == RightDoor) actualTile.x++;
                else if (door == DownDoor) actualTile.y--;
                else if(door == TopDoor) actualTile.y++;
            }
        }
    }

}
