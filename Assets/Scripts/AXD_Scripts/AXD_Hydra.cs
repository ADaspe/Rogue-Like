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
    public ELC_Enemy enemy;
    public float timeToBeVulnerable;
    public bool headsToSpawn;
    public int maxHealthHead;
    public float healthPercentageHeadPhase2;
    public float healthPercentageHeadPhase3;


    private void Start()
    {
        enemy = GetComponent<ELC_Enemy>();
        currentPhase = BossPhase.Phase1;
        enemy.isInvulnerable = true;
        headsToSpawn = true;
    }

    public void SpawnHeads()
    {
        Debug.Log("Spawning heads");
        switch (currentPhase) {
            case BossPhase.Phase1:
                AXD_HydraHead temp1 = Instantiate(headPrefab, this.gameObject.transform.GetChild(0));
                temp1.enemyScript.currentHealth = temp1.headStats.MaxHealth = maxHealthHead;
                temp1.GetHydraRef(this);
                heads.Add(temp1);
                break;

            case BossPhase.Phase2:
                AXD_HydraHead temp2 = Instantiate(headPrefab, this.gameObject.transform.GetChild(1));
                temp2.headStats.MaxHealth = temp2.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase2 / 100);
                temp2.GetHydraRef(this);
                heads.Add(temp2);
                AXD_HydraHead temp3 = Instantiate(headPrefab, this.gameObject.transform.GetChild(2));
                temp3.headStats.MaxHealth = temp3.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase2 / 100);
                temp3.GetHydraRef(this);
                heads.Add(temp3);
                break;

            case BossPhase.Phase3:
                AXD_HydraHead temp4 = Instantiate(headPrefab, this.gameObject.transform.GetChild(3));
                temp4.headStats.MaxHealth = temp4.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                temp4.GetHydraRef(this);
                heads.Add(temp4);
                AXD_HydraHead temp5 = Instantiate(headPrefab, this.gameObject.transform.GetChild(4));
                temp5.headStats.MaxHealth = temp5.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                temp5.GetHydraRef(this);
                heads.Add(temp5);
                AXD_HydraHead temp6 = Instantiate(headPrefab, this.gameObject.transform.GetChild(5));
                temp6.headStats.MaxHealth = temp6.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                temp6.GetHydraRef(this);
                heads.Add(temp6);
                AXD_HydraHead temp7 = Instantiate(headPrefab, this.gameObject.transform.GetChild(6));
                temp7.headStats.MaxHealth = temp7.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                temp7.GetHydraRef(this);
                heads.Add(temp7);
                break;
        }
        headsToSpawn = false;
    }

    private void Update()
    {
        if (enemy.currentHealth <= stats.healthPhase[(int)currentPhase])
        {
            enemy.currentHealth = stats.healthPhase[(int)currentPhase]; 
        }
        if (!enemy.isInvulnerable)
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
            enemy.isInvulnerable = false;
            timeToBeVulnerable = Time.time + stats.vulnerableTime;
        }
    }

    public void VulnerablePhase()
    {
        Debug.Log("Attack me !");
        if (Time.time > timeToBeVulnerable || enemy.currentHealth <= stats.healthPhase[(int)currentPhase])
        {
            enemy.isInvulnerable = true;
            headsToSpawn = true;
            ChangePhase();
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
}