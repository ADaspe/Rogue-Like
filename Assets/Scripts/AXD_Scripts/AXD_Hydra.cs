using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_Hydra : MonoBehaviour
{
    public enum BossPhase { Phase1, Phase2, Phase3}
    public BossPhase currentPhase;
    public AXD_BossSO stats;
    public AXD_HydraHead headPrefab;
    public List<AXD_HydraHead> heads;
    public List<GameObject> spawnPoints;
    public float timeToBeVulnerable;
    public bool headsToSpawn;


    private void Start()
    {
        currentPhase = BossPhase.Phase1;
        stats.invulnerable = true;
    }

    public void SpawnHeads()
    {
        switch (currentPhase) {
            case BossPhase.Phase1:
                AXD_HydraHead temp1 = Instantiate(headPrefab, this.gameObject.transform.GetChild(0));
                //Mettre la max health comme sur le GDD sur toutes les instanciations
                break;

            case BossPhase.Phase2:
                AXD_HydraHead temp2 = Instantiate(headPrefab, this.gameObject.transform.GetChild(0));
                AXD_HydraHead temp3 = Instantiate(headPrefab, this.gameObject.transform.GetChild(0));
                break;

            case BossPhase.Phase3:
                AXD_HydraHead temp4 = Instantiate(headPrefab, this.gameObject.transform.GetChild(0));
                AXD_HydraHead temp5 = Instantiate(headPrefab, this.gameObject.transform.GetChild(0));
                AXD_HydraHead temp6 = Instantiate(headPrefab, this.gameObject.transform.GetChild(0));
                AXD_HydraHead temp7 = Instantiate(headPrefab, this.gameObject.transform.GetChild(0));
                break;
        }
    }

    private void Update()
    {
        if (!stats.invulnerable)
        {
            VulnerablePhase();
        }
    }

    public void LoseHead(AXD_HydraHead head)
    {
        heads.Remove(head);
        if(heads.Count == 0)
        {
            stats.invulnerable = false;
            headsToSpawn = true;
        }
    }

    public void VulnerablePhase()
    {
        if(Time.time > timeToBeVulnerable)
        {
            stats.invulnerable = true;
            ChangePhase();
            SpawnHeads();
        }
    }

    public void ChangePhase()
    {
        if(currentPhase < BossPhase.Phase3)
        {
            currentPhase++;
        }
    }

    public void GetHit(int damage)
    {
        if (!stats.invulnerable) {
            stats.currentHealth -= damage;
            if (stats.currentHealth <= stats.healthPhase[(int)currentPhase-1])
            {
                stats.invulnerable = true;
                ChangePhase();
            }
        }
    }
}