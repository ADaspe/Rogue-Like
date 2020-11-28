using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_PowerUpManager : MonoBehaviour
{
    public List<ELC_PowerUpProperties> PowerUps = new List<ELC_PowerUpProperties>();

    private List<int> IndexToDeleteAfterCheck = new List<int>();

    // Update is called once per frame
    void Update()
    {
        if (PowerUps.Count > 0)
        {
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if (PowerUps[i].LifeDuration <= 0)
                {
                    DeletePowerUp(i);
                    return;
                }
                else
                {
                    PowerUps[i].LifeDuration -= Time.deltaTime;
                    //Debug.Log(PU.LifeDuration);
                }
            }
        }
    }

    private void DeletePowerUp(int index)
    {
        PowerUps.RemoveAt(index);
        Debug.Log("PowerUp Destroyed at " + index);
    }

    public void AddPowerUp(GameObject PUObject)
    {
        //bool FoundPlace = false;

        ELC_PowerUpProperties PU = PUObject.GetComponent<ELC_PowerUpProperties>();
        PU.LifeDuration = PU.PowerUpSO.duration;

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

        PowerUps.Add(PU);
        PU.index = PowerUps.Count;
    }

}
