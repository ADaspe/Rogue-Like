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
    public List<GameObject> testsRoomsList;

    public GameObject[,] checkersArray; //Liste des checkers et leur position

    public List<int> randomPoints = new List<int>(); //L'index correspond au Y et le nombre dans cet index correspond au X


    
    void Start()
    {
        checkersArray = new GameObject[arrayDimentionX, arrayDimentionY];
        //mainRoomsArray = new GameObject[arrayDimentionX, arrayDimentionY];

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

        //Instancier la room de départ
        //SpawnRandomRoom(checkersArray[(int)entryPosition.x, (int)entryPosition.y]);

        PlaceRandomPoints(); //On calcule les points randoms à prendre

        for (int i = 0; i < randomPoints.Count; i++) //Sur chacun des points on créé une salle
        {
            SpawnRandomRoom(checkersArray[randomPoints[i], i], roomsList); //On prend la valeur X dans la liste à l'emplacement i, i correspondant ici au Y des coordonnées
        }

        JunctionsBtwRandomPoints();
    }


    private void PlaceRandomPoints()
    {
        for (int i = 0; i < arrayDimentionY; i++) //A chaque ligne
        {
            int randomNumber = Random.Range(0, arrayDimentionX); //Prendre un nombre aléatoire


            if (!checkersArray[randomNumber, i].GetComponent<ELC_RoomProperties>().thereIsRoom)//Vérifier si une salle n'existe pas déjà sur la ligne à l'emplacement du nombre aléatoire
            {
                randomPoints.Add(randomNumber); //On rajoute alors le point random dans la List
            }
            else i--; //Sinon on le refait tourner pour celui-ci
        }
    }

    private void JunctionsBtwRandomPoints()
    {
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
                SpawnRandomRoom(checkersArray[randomPoints[i] + direction * (e + 1), i], testsRoomsList); //On fait spawner une salle sur le checker qui a en x : le random point (qui constitue le point de départ), auquel on ajoute e pour avancer de 1 case à chaque fois (et il nous faut décaler de 1 case au début pour éviter d'instancier sur la room déjà existante d'où le (e+1)) et on multiplie par la valeur direction pour indiquer si on se déplace d'une case vers la gauche ou la droite
            }
        }
    }

    private void SpawnRandomRoom(GameObject checkerObject, List<GameObject> rl)
    {
        int arrayX = (int)checkerObject.GetComponent<ELC_RoomProperties>().positionNumber.x;
        int arrayY = (int)checkerObject.GetComponent<ELC_RoomProperties>().positionNumber.y;

        if (!checkersArray[arrayX, arrayY].GetComponent<ELC_RoomProperties>().thereIsRoom) //vérifie si une salle n'a pas déjà été enregistrée ici
        {
            int randomNumber = Random.Range(0, rl.Count);
            GameObject.Instantiate(rl[randomNumber], checkerObject.transform).transform.SetParent(checkerObject.transform);
            checkersArray[arrayX, arrayY].GetComponent<ELC_RoomProperties>().thereIsRoom = true;
        }
        else Debug.Log("Script is trying to create a room at " + arrayX + ", " + arrayY + " but a room already exist here.");
    }
}
