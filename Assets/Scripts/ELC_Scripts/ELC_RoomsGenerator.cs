using System;
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

    public float timeToWait;
    public bool isInACoroutine;

    public GameObject CoreElements;

    private enum Directions { Right, Left, Top, Down};

    public GameObject roomChecker;
    public GameObject startRoom;
    public GameObject endRoom;

    //public List<GameObject> roomsList;
    //public List<GameObject> testsRoomsList;
    public List<GameObject> easyRoomsList;
    public List<GameObject> MediumRoomsList;
    public List<GameObject> HardRoomsList;
    public List<GameObject> ObjectsRoomsList;

    public int EasyFloors; //A partir de quel étage commence les salles easy
    public int MediumFloors; //A partir de quel étage commence les salles easy
    public int HardFloors; //A partir de quel étage commence les salles easy
    public int ObjectRoomPercentChanceToSpawn;

    public GameObject[,] checkersArray; //Liste des checkers et leur position

    public List<int> randomPoints = new List<int>(); //L'index correspond au Y et le nombre dans cet index correspond au X
    public List<int> secondaryRandomPoints = new List<int>();//L'index correspond au Y et le nombre dans cet index correspond au X
    public List<int> numberOfEmptyRooms = new List<int>();//L'index correspond au Y et le nombre dans cet index correspond au X, si le nombre est -1 alors c'est que la room peut pas être posée



    void Start()
    {
        for (int i = 0; i < arrayDimentionY; i++) //Pour instancier la Liste des rooms libres
        {
            numberOfEmptyRooms.Add(arrayDimentionX);
        }

        StartCoroutine("Generator");
    }

    IEnumerator Generator()
    {
        checkersArray = new GameObject[arrayDimentionX, arrayDimentionY];

        //Instancier les checkers
        for (int i = 0; i < arrayDimentionX; i++)
        {
            for (int e = 0; e < arrayDimentionY; e++)
            {
                GameObject go = GameObject.Instantiate(roomChecker, new Vector3(i * distanceBtwRoomsX, -e * distanceBtwRoomsY), Quaternion.identity);
                go.GetComponent<ELC_RoomProperties>().positionNumber.x = i;
                go.GetComponent<ELC_RoomProperties>().positionNumber.y = e;
                checkersArray[i, e] = go;
            }
        }

        PlaceRandomPoints(); //On calcule les points randoms à prendre

        for (int i = 0; i < randomPoints.Count; i++) //Sur chacun des points on créé une salle
        {
            yield return new WaitForSeconds(timeToWait);
            if (arrayDimentionY - EasyFloors < i + 1)
            {
                SpawnRandomRoom(checkersArray[randomPoints[i], i], easyRoomsList, true, true, true, true, true);
                Debug.Log("facile");
            }
            else if (arrayDimentionY - EasyFloors - MediumFloors < i + 1)
            {
                SpawnRandomRoom(checkersArray[randomPoints[i], i], MediumRoomsList, true, true, true, true, true);
                Debug.Log("moyen");
            }
            else
            {
                SpawnRandomRoom(checkersArray[randomPoints[i], i], easyRoomsList, true, true, true, true, true);
                Debug.Log("difficile");
            }

            if (i == 0) //La première room
            {
                endRoom = checkersArray[randomPoints[i], i]; //On assigne la première room en tant que startRoom
                endRoom.GetComponent<ELC_RoomProperties>().IsStartRoom = true;
            }
            else if(i == randomPoints.Count -1)
            {
                startRoom = checkersArray[randomPoints[i], i];
                startRoom.GetComponent<ELC_RoomProperties>().IsEndRoom = true;
            }
        }

        StartCoroutine("JunctionsBtwRandomPoints");

        yield return new WaitWhile(() => isInACoroutine == true);
        StartCoroutine("PlaceSecondaryRandomPoints");

        yield return new WaitWhile(() => isInACoroutine == true);
        StartCoroutine("JunctionBtwSecondaryPoints");

        yield return new WaitWhile(() => isInACoroutine == true); //Tant que isInACoroutine = true, on met la fonction en suspens
        StartCoroutine("DoorsCheck");

        yield return new WaitWhile(() => isInACoroutine == true);
        Instantiate(CoreElements, startRoom.transform.position - new Vector3(0, distanceBtwRoomsY / 2), Quaternion.identity);
        Debug.Log("Finish !");
    }

    private void PlaceRandomPoints()
    {
        for (int i = 0; i < arrayDimentionY; i++) //A chaque ligne
        {
            int randomNumber = UnityEngine.Random.Range(0, arrayDimentionY); //Prendre un nombre aléatoire entre 0 et le nombre de rooms vides disponibles sur la ligne


            if (!checkersArray[randomNumber, i].GetComponent<ELC_RoomProperties>().thereIsRoom)//Vérifier si une salle n'existe pas déjà sur la ligne à l'emplacement du nombre aléatoire
            {
                randomPoints.Add(randomNumber); //On rajoute alors le point random dans la List
            }
            else i--; //Sinon on le refait tourner pour celui-ci
        }
    } //Place un point random sur chaque ligne et les enregistre dans une List

    IEnumerator JunctionsBtwRandomPoints()
    {
        isInACoroutine = true;
        for (int i = 0; i < randomPoints.Count - 1; i++) //Sur chaque ligne sauf la dernière ligne parcequ'on en a pas besoin c'est la fin
        {
            int distance = randomPoints[i + 1] - randomPoints[i]; //Calcul de la distance X qui sépare le point random de son prochain point random

            int numberOfRoomsToInstanciate = distance; //Cette valeur compte le nombre de rooms à instancier pour rejoindre les 2 points
            int direction = 1; //Cette valeur va servir de multiplicateur lorsqu'on va créer les room, s'il est positif on avancera vers la droite, et si il est négatif on avancera vers la gauche

            if (distance < 0)
            {
                numberOfRoomsToInstanciate = -distance; //Pour que la numberOfRoomsToInstanciate soit toujours positive
                direction = -1; //Si la distance est < 0 ça veut dire que le prochain point est sur la gauche donc on passe la direction en négatif
            }

            for (int e = 0; e < numberOfRoomsToInstanciate; e++) //On fait cette action jusqu'à ce qu'on ait atteint le nombre de rooms à instancier
            {
                yield return new WaitForSeconds(timeToWait);
                if(arrayDimentionY - EasyFloors < i + 1) SpawnRandomRoom(checkersArray[randomPoints[i] + direction * (e + 1), i], easyRoomsList, true, true, true, true, false); //On fait spawner une salle sur le checker qui a en x : le random point (qui constitue le point de départ), auquel on ajoute e pour avancer de 1 case à chaque fois (et il nous faut décaler de 1 case au début pour éviter d'instancier sur la room déjà existante d'où le (e+1)) et on multiplie par la valeur direction pour indiquer si on se déplace d'une case vers la gauche ou la droite
                else if(arrayDimentionY - EasyFloors - MediumFloors < i + 1) SpawnRandomRoom(checkersArray[randomPoints[i] + direction * (e + 1), i], MediumRoomsList, true, true, true, true, false);
                else SpawnRandomRoom(checkersArray[randomPoints[i] + direction * (e + 1), i], HardRoomsList, true, true, true, true, false);
            }
        }

        isInACoroutine = false;
    } //Construit les salles entre chaque points

    IEnumerator JunctionBtwSecondaryPoints()
    {
        isInACoroutine = true;

        for (int i = 0; i < secondaryRandomPoints.Count; i++)
        {
            int distance = randomPoints[i] - secondaryRandomPoints[i];
            int dir = 1;
            if (distance < 0)//Si la salle du random point principal de la ligne est à gauche on passe la dir en négatif
            {
                dir = -1;
                distance = -distance; //Si distance est négative on la mets en positif, pas fou
            }

            for (int e = 0; e < distance - 1; e++)
            {
                if (!checkersArray[secondaryRandomPoints[i] + dir * (e + 1), i].GetComponent<ELC_RoomProperties>().thereIsRoom)
                {
                    yield return new WaitForSeconds(timeToWait);
                    if (arrayDimentionY - EasyFloors < i + 1) SpawnRandomRoom(checkersArray[secondaryRandomPoints[i] + dir * (e + 1), i], easyRoomsList, true, true, true, true, false, true);
                    else if(arrayDimentionY - EasyFloors - MediumFloors < i + 1) SpawnRandomRoom(checkersArray[secondaryRandomPoints[i] + dir * (e + 1), i], MediumRoomsList, true, true, true, true, false, true);
                    else SpawnRandomRoom(checkersArray[secondaryRandomPoints[i] + dir * (e + 1), i], HardRoomsList, true, true, true, true, false, true);
                }
            }

        }

        isInACoroutine = false;
    } //Construit les salles pour relier les salles secondaires
    IEnumerator PlaceSecondaryRandomPoints() //Placer les seconds poins aléatoirement dans les zones vides et ensuite les relier
    {
        isInACoroutine = true;
        for (int i = 0; i < arrayDimentionY; i++)
        {
            if (numberOfEmptyRooms[i] > 0)
            {
                int randomNum = UnityEngine.Random.Range(0, numberOfEmptyRooms[i]);
                int checkPositionX;
                checkPositionX = 0;
                yield return new WaitForSeconds(timeToWait);
                for (int e = randomNum; e >= 0; e--)//Prends le nombre pris aléatoirement pour faire tourner un certain nombre de fois la boucle
                {
                    if (checkersArray[checkPositionX, i].GetComponent<ELC_RoomProperties>().thereIsRoom) //Si y'a une room à l'endroit où tu dois check, on decrease pas e pour qu'on relance le check mais avec une position x+1 (qu'on détermine après), c'est un peu comme si on sautait la case
                    {
                        e++;
                    }
                    else if (e == 0) //Si on arrive à 0 c'est qu'on a parcourus le nombre de cases qu'on voulait parmis les cases vides
                    {
                        secondaryRandomPoints.Add(checkPositionX); //Du coup on add la coordonnée à laquelle on veut le nouveau point
                    }
                    checkPositionX += 1;
                }
                if(arrayDimentionY - EasyFloors < i + 1) SpawnRandomRoom(checkersArray[secondaryRandomPoints[i], i], easyRoomsList, true, true, true, true, false, true);
                else if(arrayDimentionY - EasyFloors - MediumFloors < i + 1) SpawnRandomRoom(checkersArray[secondaryRandomPoints[i], i], MediumRoomsList, true, true, true, true, false, true);
                else SpawnRandomRoom(checkersArray[secondaryRandomPoints[i], i], HardRoomsList, true, true, true, true, false, true);
            }
            else secondaryRandomPoints.Add(-1);
            
        }
        isInACoroutine = false;
        
    }

    private void SpawnRandomRoom(GameObject checkerObject, List<GameObject> rl, bool openRight, bool openLeft, bool openTop, bool openDown, bool isRoomFromRandomNumber, bool secondaryPath = false)
    {
        int arrayX = (int)checkerObject.GetComponent<ELC_RoomProperties>().positionNumber.x;
        int arrayY = (int)checkerObject.GetComponent<ELC_RoomProperties>().positionNumber.y;
        ELC_RoomProperties roomScript = checkerObject.GetComponent<ELC_RoomProperties>();

        if (!roomScript.thereIsRoom) //vérifie si une salle n'a pas déjà été enregistrée ici
        {
            numberOfEmptyRooms[arrayY]--;
            int chances = UnityEngine.Random.Range(0, 100);

            if (secondaryPath && chances < ObjectRoomPercentChanceToSpawn)
            {
                int randomNumber = UnityEngine.Random.Range(0, ObjectsRoomsList.Count); //prend un nombre aléatoire dans la List des objectRooms
                GameObject.Instantiate(ObjectsRoomsList[randomNumber], checkerObject.transform).transform.SetParent(checkerObject.transform);
            }
            else
            {
                int randomNumber = UnityEngine.Random.Range(0, rl.Count); //prend un nombre aléatoire dans la List
                GameObject.Instantiate(rl[randomNumber], checkerObject.transform).transform.SetParent(checkerObject.transform); //Met la salle aléatoire en fonction du nombre donné, et la fait enfant du checker
            }
            roomScript.thereIsRoom = true; //Dit au checker de l'emplacement de la salle qu'il y a une salle maintenant
            roomScript.isAnAngleRoom = isRoomFromRandomNumber;

            //On renseigne les portes qui doivent être ouvertes/fermées
            roomScript.openDownDoor = openDown; 
            roomScript.openTopDoor = openTop;
            roomScript.openLeftDoor = openLeft;
            roomScript.openRightDoor = openRight;
            
        }
        //else Debug.Log("Script is trying to create a room at " + arrayX + ", " + arrayY + " but a room already exist here.");
    } //Pour faire spawner une room prise aléatoirement dans une List, à un emplacement défini

    private GameObject ReturnAdjacentChecker(int refPosX, int refPosY, Directions direction) //Va servir à récupérer et renvoyer le checker ajdacent (de droite/gauche/haut/bas au choix)
    {
        if (refPosY <= arrayDimentionY -1 && refPosX <= arrayDimentionX -1) //Vérifie que les valeurs ne dépassent pas celles de la dimension de la map, on mets -1 après prcq le checkerArray compte le 0 comme une valeur (par exemple dans le ArrayDimentionX on va mettre 10 pour la longueur, mais le 1 correspond à 0 pour le checker Array et donc on a 0,1,2...8,9 pour un arrayDim de 10)
        {
            if (direction == Directions.Down && refPosY + 1 <= arrayDimentionY -1 ) return checkersArray[refPosX, refPosY + 1];
            else if (direction == Directions.Top && refPosY - 1 >= 0) return checkersArray[refPosX, refPosY - 1];
            else if (direction == Directions.Right && refPosX + 1 <= arrayDimentionX -1) return checkersArray[refPosX + 1, refPosY];
            else if (direction == Directions.Left && refPosX - 1 >= 0) return checkersArray[refPosX - 1, refPosY];
            else
            {
                //Debug.Log("(LOOKS LIKE A PROBLEM BUT THIS IS NOT, LEAVE ME ALONE) Trying to return a value in function ReturnAdjacentChecker but there is no checker at the " + direction + " of " + checkersArray[refPosX,refPosY].name + "");
                return null;
            }
        }
        else
        {
            //Debug.Log("Error when trying to return a value in function ReturnAdjacentChecker : refPosY or refPosX are greater than the arrayDimentions");
            return null;
        }
    }

    IEnumerator DoorsCheck()
    {
        isInACoroutine = true;
        foreach (GameObject checker in checkersArray) //On vérifie dans tous les checkers de la map
        {
            int checkerPosX = (int)checker.GetComponent<ELC_RoomProperties>().positionNumber.x;
            int checkerPosY = (int)checker.GetComponent<ELC_RoomProperties>().positionNumber.y;

            if (checker.GetComponent<ELC_RoomProperties>().thereIsRoom) //Lorsqu'on tombe sur un checker qui a une room
            {
                yield return new WaitForSeconds(timeToWait);
                foreach (Directions dir in Enum.GetValues(typeof(Directions))) //Pour chacun des directions présentes dans Directions (haut/bas/droite/gauche)
                {
                    yield return new WaitForSeconds(timeToWait/4); //On veut qu'entre les 4 checks il y ait 4 fois moins de temps que d'habitude
                    GameObject adjacentChecker = ReturnAdjacentChecker(checkerPosX, checkerPosY, dir); //On prend le checker qui est dans direction désirée
                    if (adjacentChecker != null) //S'il y a un checker dans la direction
                    {
                        //Debug.Log("Room detected at the " + dir + " of " + checker.name);
                        //Dans la direction voulue : s'il y a une room ET si c'est à gauche OU droite OU que la room adjacente a "isAnAngleRoom" en true et qu'on vérifie le bas OU que la room actuelle est une AngleRoom et qu'on vérifie la direction Top alors on ouvre, ça permet de pas avoir d'ouverture en haut/bas lorque c'est un couloir
                        if (adjacentChecker.GetComponent<ELC_RoomProperties>().thereIsRoom && (dir == Directions.Left || dir == Directions.Right || (adjacentChecker.GetComponent<ELC_RoomProperties>().isAnAngleRoom && dir == Directions.Down) || (checker.GetComponent<ELC_RoomProperties>().isAnAngleRoom && dir == Directions.Top))) DoorState(checker, true, dir); 
                        else DoorState(checker, false, dir); //On supprime la porte à cet endroit
                    }
                    else if (checker.GetComponent<ELC_RoomProperties>().IsStartRoom && dir == Directions.Top || checker.GetComponent<ELC_RoomProperties>().IsEndRoom && dir == Directions.Down) DoorState(checker, true, dir); //SI c'est la salle Start on laisse la porte nord ouverte ou si c'est la salle End on laisse la porte sud ouverte
                    else //S'il n'y a pas de checker
                    {
                        DoorState(checker, false, dir); //On supprime la porte à cet endroit
                        //Debug.Log("There is no checker at the " + dir + " of " + checker.name);
                    }
                }
                checker.GetComponent<ELC_RoomProperties>().UpdateCorridors(); //On demande au script d'update ses couloirs
                checker.GetComponent<ELC_RoomProperties>().Loaded = true;
            }
        }
        isInACoroutine = false;
    }

    private void DoorState(GameObject roomChecker, bool openDoor, Directions dir)
    {
        ELC_RoomProperties roomCheckerScript = roomChecker.GetComponent<ELC_RoomProperties>();
        if (dir == Directions.Down) roomCheckerScript.openDownDoor = openDoor;
        else if (dir == Directions.Top) roomCheckerScript.openTopDoor = openDoor;
        else if (dir == Directions.Left) roomCheckerScript.openLeftDoor = openDoor;
        else roomCheckerScript.openRightDoor = openDoor;

    }
}
