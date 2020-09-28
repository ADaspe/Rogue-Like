using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_RandomObjectGenerator : MonoBehaviour
{
    public List<GameObject> objectsList = new List<GameObject>();
    public List<float> spawnChance = new List<float>();
    
    

    void Start()
    {
        float totalChances = 0;
        for (int i = 0; i < spawnChance.Count; i++)
        {
            totalChances += spawnChance[i];
        }

        for (int i = 0; i < spawnChance.Count; i++)
        {
            spawnChance[i] /= totalChances;
        }

        float randomNumber = Random.value;
        Debug.Log(randomNumber);

        float count = 0;
        for (int i = 0; i < spawnChance.Count; i++)
        {
            count += spawnChance[i];
            Debug.Log(count);

            if (count >= randomNumber)
            {
                Object.Instantiate(objectsList[i], this.transform.position, Quaternion.identity);
                Debug.Log(objectsList[i] + " has been instantiate.");
                break;
            }
        }

    }
}
