using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_Hydra : MonoBehaviour
{
    public enum BossPhase { Phase1, Phase2, Phase3}
    public BossPhase currentPhase;
    public ELC_EnemySO stats;
    public AXD_HydraHead headPrefab;
    public List<AXD_HydraHead> heads;
    public List<GameObject> spawnPoints;
    public bool invulnerable;
    public int currentHealth;
    public float timeToBeVulnerable;
    public bool headsToSpawn;
    public int maxHealthHead;
    public float healthPercentageHeadPhase2;
    public float healthPercentageHeadPhase3;
    public int[] healthPhase;


    private void Start()
    {
        currentPhase = BossPhase.Phase1;
        invulnerable = true;
        headsToSpawn = true;
        currentHealth = stats.MaxHealth;
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
        if (!invulnerable)
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
            invulnerable = false;
            headsToSpawn = true;
        }
    }

    public void VulnerablePhase()
    {
        Debug.Log("Attack me !");
        if(Time.time > timeToBeVulnerable)
        {
            invulnerable = true;
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

    public void GetHit(int damage, Vector3 directionToFlee, float knockbackDistance = 0, float stunTime = 0, bool invulnerable = false)
    {
        if (invulnerable) {
            currentHealth -= damage;
            Debug.Log("Hydra : Ouch");
            if (currentHealth <= healthPhase[(int)currentPhase-1])
            {
                invulnerable = true;
                ChangePhase();
                
            }
        }
        else
        {
            Debug.Log("Hydra : Pas Ouch");
        }
    }
}