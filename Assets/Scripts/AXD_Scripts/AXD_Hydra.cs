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
    public int currentHealth;
    public float timeToBeVulnerable;
    public bool headsToSpawn;
    public int maxHealthHead;
    public float healthPercentageHeadPhase2;
    public float healthPercentageHeadPhase3;


    private void Start()
    {
        currentPhase = BossPhase.Phase1;
        stats.invulnerable = true;
        headsToSpawn = true;
        currentHealth = stats.maxHealth;
    }

    public void SpawnHeads()
    {
        Debug.Log("Spawning heads");
        switch (currentPhase) {
            case BossPhase.Phase1:
                AXD_HydraHead temp1 = Instantiate(headPrefab, this.gameObject.transform.GetChild(0));
                //Mettre la max health comme sur le GDD sur toutes les instanciations
                temp1.currentHealth = temp1.headStats.MaxHealth = maxHealthHead;
                break;

            case BossPhase.Phase2:
                AXD_HydraHead temp2 = Instantiate(headPrefab, this.gameObject.transform.GetChild(1));
                temp2.headStats.MaxHealth = temp2.currentHealth = Mathf.RoundToInt(maxHealthHead*healthPercentageHeadPhase2 / 100);
                AXD_HydraHead temp3 = Instantiate(headPrefab, this.gameObject.transform.GetChild(2));
                temp2.headStats.MaxHealth = temp2.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase2 / 100);
                break;

            case BossPhase.Phase3:
                AXD_HydraHead temp4 = Instantiate(headPrefab, this.gameObject.transform.GetChild(3));
                temp4.headStats.MaxHealth = temp4.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                AXD_HydraHead temp5 = Instantiate(headPrefab, this.gameObject.transform.GetChild(4));
                temp5.headStats.MaxHealth = temp5.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                AXD_HydraHead temp6 = Instantiate(headPrefab, this.gameObject.transform.GetChild(5));
                temp6.headStats.MaxHealth = temp6.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                AXD_HydraHead temp7 = Instantiate(headPrefab, this.gameObject.transform.GetChild(6));
                temp7.headStats.MaxHealth = temp7.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                break;
        }
        headsToSpawn = false;
    }

    private void Update()
    {
        if (!stats.invulnerable)
        {
            VulnerablePhase();
        }else if (headsToSpawn)
        {
            SpawnHeads();
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
        Debug.Log("Attack me !");
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
            Debug.Log("Changing phase");
            currentPhase++;
        }
        SpawnHeads();
    }

    public void GetHit(int damage)
    {
        if (!stats.invulnerable) {
            currentHealth -= damage;
            Debug.Log("Hydra : Ouch");
            if (currentHealth <= stats.healthPhase[(int)currentPhase-1])
            {
                stats.invulnerable = true;
                ChangePhase();
                
            }
        }
    }
}