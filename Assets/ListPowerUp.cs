using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ListPowerUp : MonoBehaviour
{

    public List<GameObject> PowerUpList = new List<GameObject>();
    public GameObject PowerUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Ajout()
    {
        PowerUpList.Add(PowerUp);
    }
}
