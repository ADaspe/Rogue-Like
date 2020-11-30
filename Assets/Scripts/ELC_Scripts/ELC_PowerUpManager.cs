using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PowerUpManager : MonoBehaviour
{
    public List<GameObject> PowerUps = new List<GameObject>();

    [SerializeField]
    private List<float> durations = new List<float>();
    

    // Update is called once per frame
    void Update()
    {
        if (PowerUps.Count > 0)
        {
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if (PowerUps[i].GetComponent<ELC_PowerUpProperties>().LifeDuration <= 0)
                {
                    DeletePowerUp(i);
                    Debug.Log("passe ici");
                    return;
                }
                else
                {
                    PowerUps[i].GetComponent<ELC_PowerUpProperties>().LifeDuration -= Time.deltaTime;
                    durations[i] = PowerUps[i].GetComponent<ELC_PowerUpProperties>().LifeDuration;
                    //Debug.Log(PU.LifeDuration);
                }
            }
        }
    }

    private void DeletePowerUp(int index)
    {
        PowerUps.Remove(PowerUps[index]);
        durations.Remove(durations[index]);
        Debug.Log("PowerUp Destroyed at " + index);
    }

    public void AddPowerUp(GameObject PUObject)
    {
        //bool FoundPlace = false;
        GameObject GO;
        ELC_PowerUpProperties PU = PUObject.GetComponent<ELC_PowerUpProperties>();

        //for (int i = 0; i < PowerUps.Count; i++)
        //{
        //    if (PowerUps[i] == null && FoundPlace == false)
        //    {
        //        PowerUps[i] = PU;
        //        
        //        FoundPlace = true;
        //        return;
        //    }
        //}
        GO = new GameObject();
        GO.name = "PowerUp";
        GO.AddComponent<ELC_PowerUpProperties>();
        GO.GetComponent<ELC_PowerUpProperties>().LifeDuration = PU.LifeDuration;
        GO.GetComponent<ELC_PowerUpProperties>().PowerUpSO = PU.PowerUpSO;

        PowerUps.Add(GO);
        durations.Add(0);
        PowerUps[PowerUps.Count - 1].GetComponent<ELC_PowerUpProperties>().LifeDuration = PowerUps[PowerUps.Count - 1].GetComponent<ELC_PowerUpProperties>().PowerUpSO.duration;
        
    }

}
