using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ELC_PowerUpManager : MonoBehaviour
{
    public List<GameObject> PowerUps = new List<GameObject>();
    public ELC_PlayerStatManager playerStatsScript;
    public GameObject PowerUpsDisplay;

    public bool FauxDeChronos;
    public bool StopPUFlow;
    public float TimeToRestartPUFlow;
    public AudioSource powerUpSound;

    [SerializeField]
    private List<float> durations = new List<float>();

    public List<GameObject> PUEmplacementsUI;

    private void Start()
    {
        for (int i = 0; i < PowerUpsDisplay.transform.childCount; i++)
        {
            PUEmplacementsUI.Add(PowerUpsDisplay.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FauxDeChronos && TimeToRestartPUFlow < Time.time && StopPUFlow == true) StopPUFlow = false;
        if (PowerUps.Count > 0)
        {
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if (PowerUps[i].GetComponent<ELC_PowerUpProperties>().LifeDuration <= 0)
                {
                    DeletePowerUp(i);
                    return;
                }
                else 
                {
                    if (!StopPUFlow)
                    {
                        PowerUps[i].GetComponent<ELC_PowerUpProperties>().LifeDuration -= Time.deltaTime;
                        durations[i] = PowerUps[i].GetComponent<ELC_PowerUpProperties>().LifeDuration;
                        //Debug.Log(PU.LifeDuration);
                        ApplyPowerUp(PowerUps[i]);
                    }
                    PUEmplacementsUI[i].SetActive(true);
                    PUEmplacementsUI[i].GetComponent<Image>().sprite = PowerUps[i].GetComponent<ELC_PowerUpProperties>().PowerUpSO.HUDSprite;
                }
            }
        }
    }

    public void StopFlow()
    {
        if(FauxDeChronos)
        {
            StopPUFlow = true;
            TimeToRestartPUFlow = Time.time + FindObjectOfType<ELC_PassivesProperties>().ChronosStopPowerUpFlowDuration;
        }
    }

    private void DeletePowerUp(int index)
    {
        ELC_PowerUpSO PUSO = PowerUps[index].GetComponent<ELC_PowerUpProperties>().PowerUpSO;
        

        if (PUSO.type == ELC_PowerUpSO.Type.Attack) playerStatsScript.AttackMultiplicatorPU = 1;
        else if (PUSO.type == ELC_PowerUpSO.Type.Heal) playerStatsScript.DefenseMultiplicatorPU = 1;
        else if (PUSO.type == ELC_PowerUpSO.Type.MoneyEarn) playerStatsScript.MoneyMultiplicatorPU = 1;
        else if (PUSO.type == ELC_PowerUpSO.Type.Speed) playerStatsScript.SpeedMultiplicatorPU = 1;

        Destroy(PowerUps[index]);
        PowerUps.Remove(PowerUps[index]);
        durations.Remove(durations[index]);
        Debug.Log("PowerUp Destroyed at " + index);
        PUEmplacementsUI[PowerUps.Count].SetActive(false);
    }

    
    public void AddPowerUp(GameObject PUObject)
    {
        GameObject GO;
        ELC_PowerUpProperties PU = PUObject.GetComponent<ELC_PowerUpProperties>();

        GO = new GameObject();
        GO.name = PU.PowerUpSO.type.ToString() + " niveau " + PU.PowerUpSO.level;
        GO.transform.SetParent(this.transform);
        GO.AddComponent<ELC_PowerUpProperties>();
        GO.GetComponent<ELC_PowerUpProperties>().LifeDuration = PU.LifeDuration;
        GO.GetComponent<ELC_PowerUpProperties>().PowerUpSO = PU.PowerUpSO;
        if (PowerUps.Count > 0)
        {
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if (PowerUps[i].GetComponent<ELC_PowerUpProperties>().PowerUpSO.type == GO.GetComponent<ELC_PowerUpProperties>().PowerUpSO.type)
                {
                    Destroy(PowerUps[i]);
                    PowerUps[i] = GO;
                    PowerUps[i].GetComponent<ELC_PowerUpProperties>().LifeDuration = PowerUps[i].GetComponent<ELC_PowerUpProperties>().PowerUpSO.duration;
                    durations[i] = GO.GetComponent<ELC_PowerUpProperties>().LifeDuration;
                    return;
                }
                else if (i == PowerUps.Count - 1)
                {
                    PowerUps.Add(GO);
                    PowerUps[i + 1].GetComponent<ELC_PowerUpProperties>().LifeDuration = PowerUps[i + 1].GetComponent<ELC_PowerUpProperties>().PowerUpSO.duration;
                    durations.Add(GO.GetComponent<ELC_PowerUpProperties>().LifeDuration);
                    return;
                }
            }
        }
        else
        {
            PowerUps.Add(GO);
            PowerUps[0].GetComponent<ELC_PowerUpProperties>().LifeDuration = PowerUps[0].GetComponent<ELC_PowerUpProperties>().PowerUpSO.duration;
            durations.Add(GO.GetComponent<ELC_PowerUpProperties>().LifeDuration);
        }
    }



    private void ApplyPowerUp(GameObject PowerUp)
    {
        ELC_PowerUpSO PUSO = PowerUp.GetComponent<ELC_PowerUpProperties>().PowerUpSO;
        float multiplicatorToApply = PUSO.multiplicator;

        if (PUSO.type == ELC_PowerUpSO.Type.Attack) playerStatsScript.AttackMultiplicatorPU = multiplicatorToApply;
        else if (PUSO.type == ELC_PowerUpSO.Type.Heal) playerStatsScript.DefenseMultiplicatorPU = multiplicatorToApply;
        else if (PUSO.type == ELC_PowerUpSO.Type.MoneyEarn) playerStatsScript.MoneyMultiplicatorPU = multiplicatorToApply;
        else if (PUSO.type == ELC_PowerUpSO.Type.Speed) playerStatsScript.SpeedMultiplicatorPU = multiplicatorToApply;


    }

}
