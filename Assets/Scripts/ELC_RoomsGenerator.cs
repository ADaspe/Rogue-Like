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

    private enum Directions { Right, Left, Top, Down};

    public GameObject roomChecker;

    public List<GameObject> roomsList;
    public List<GameObject> testsRoomsList;

    public GameObject[,] checkersArray; //Liste des checkers et leur position

    public List<int> randomPoints = new List<int>(); //L'index correspond au Y et le nombre dans cet index correspond au X


    
    void Start()
    {
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
            SpawnRandomRoom(checkersArray[randomPoints[i], i], roomsList, true, true, true, true, true);
        }

        StartCoroutine("JunctionsBtwRandomPoints");

        yield return new WaitWhile(() => isInACoroutine == true); //Tant que isInACoroutine = true, on met la fonction en suspens
        StartCoroutine("DoorsCheck");

        yield return new WaitWhile(() => isInACoroutine == true);
        Debug.Log("Finish !");
    }

    private void PlaceRandomPoints()
    {
        for (int i = 0; i < arrayDimentionY; i++) //A chaque ligne
        {
            int randomNumber = UnityEngine.Random.Range(0, arrayDimentionX); //Prendre un nombre aléatoire


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
                numberOfRoomsToInstanciate = -distance; //Pour que la valeur soit toujours positive
                direction = -1; //Si la distance est < 0 ça veut dire que le prochain point est sur la gauche donc on passe la direction en négatif
            }

            for (int e = 0; e < numberOfRoomsToInstanciate; e++) //On fait cette action jusqu'à ce qu'on ait atteint le nombre de rooms à instancier
            {
                yield return new WaitForSeconds(timeToWait);
                SpawnRandomRoom(checkersArray[randomPoints[i] + direction * (e + 1), i], testsRoomsList, true, true, true, true, false); //On fait spawner une salle sur le checker qui a en x : le random point (qui constitue le point de départ), auquel on ajoute e pour avancer de 1 case à chaque fois (et il nous faut décaler de 1 case au début pour éviter d'instancier sur la room déjà existante d'où le (e+1)) et on multiplie par la valeur direction pour indiquer si on se déplace d'une case vers la gauche ou la droite
            }
        }
        isInACoroutine = false;
    } //Construit les salles entre chaque points

    private void SpawnRandomRoom(GameObject checkerObject, List<GameObject> rl, bool openRight, bool openLeft, bool openTop, bool openDown, bool isRoomFromRandomNumber)
    {
        int arrayX = (int)checkerObject.GetComponent<ELC_RoomProperties>().positionNumber.x;
        int arrayY = (int)checkerObject.GetComponent<ELC_RoomProperties>().positionNumber.y;
        ELC_RoomProperties roomScript = checkerObject.GetComponent<ELC_RoomProperties>();

        if (!roomScript.thereIsRoom) //vérifie si une salle n'a pas déjà été enregistrée ici
        {
            int randomNumber = UnityEngine.Random.Range(0, rl.Count); //prend un nombre aléatoire dans la List
            GameObject.Instantiate(rl[randomNumber], checkerObject.transform).transform.SetParent(checkerObject.transform); //Met la salle aléatoire en fonction du nombre donné, et la fait enfant du checker
            roomScript.thereIsRoom = true; //Dit au checker de l'emplacement de la salle qu'il y a une salle maintenant
            roomScript.isAnAngleRoom = isRoomFromRandomNumber;

            //On renseigne les portes qui doivent être ouvertes/fermées
            roomScript.openDownDoor = openDown; 
            roomScript.openTopDoor = openTop;
            roomScript.openLeftDoor = openLeft;
            roomScript.openRightDoor = openRight;
        }
        else Debug.Log("Script is trying to create a room at " + arrayX + ", " + arrayY + " but a room already exist here.");
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
                Debug.Log("(LOOKS LIKE A PROBLEM BUT THIS IS NOT, LEAVE ME ALONE) Trying to return a value in function ReturnAdjacentChecker but there is no checker at the " + direction + " of " + checkersArray[refPosX,refPosY].name + "");
                return null;
            }
        }
        else
        {
            Debug.Log("Error when trying to return a value in function ReturnAdjacentChecker : refPosY or refPosX are greater than the arrayDimentions");
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
                    yield return new WaitForSeconds(timeToWait/2); //On veut qu'entre les 4 checks il y ait 2 fois moins de temps que d'habitude
                    GameObject adjacentChecker = ReturnAdjacentChecker(checkerPosX, checkerPosY, dir); //On prend le checker qui est dans direction désirée
                    if (adjacentChecker != null) //S'il y a un checker dans la direction
                    {
                        Debug.Log("Room detected at the " + dir + " of " + checker.name);
                        //Dans la direction voulue : s'il y a une room ET si c'est à gauche ou droite ou que (la room actuelle a "isAnAngleRoom" en true et (que la room dans la direction a "isAnAngleRoom" en true ou qu'on soit en train de vérifier le Top)) alors on ouvre, ça permet de pas avoir d'ouverture en haut/bas lorque c'est un couloir
                        if (adjacentChecker.GetComponent<ELC_RoomProperties>().thereIsRoom && (dir == Directions.Left || dir == Directions.Right || (adjacentChecker.GetComponent<ELC_RoomProperties>().isAnAngleRoom && dir == Directions.Down) ||(checker.GetComponent<ELC_RoomProperties>().isAnAngleRoom && (dir == Directions.Top || adjacentChecker.GetComponent<ELC_RoomProperties>().isAnAngleRoom)))) DoorState(checker, true, dir); 
                        else DoorState(checker, false, dir); //On supprime la porte à cet endroit
                    }
                    else //S'il n'y a pas de checker
                    {
                        DoorState(checker, false, dir); //On supprime la porte à cet endroit
                        Debug.Log("There is no checker at the " + dir + " of " + checker.name);
                    }
                }
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
