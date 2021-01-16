using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AXD_Hydra : MonoBehaviour
{
    public enum BossPhase { Phase1, Phase2, Phase3}
    public BossPhase currentPhase;
    public AXD_BossSO stats;
    public AXD_HydraHead head1Prefab;
    public AXD_HydraHead head2Prefab;
    public AXD_HydraHead head3Prefab;
    public AXD_HydraHead head4Prefab;
    public List<AXD_HydraHead> heads;
    public List<GameObject> spawnPoints;
    public ELC_Enemy enemy;
    public float[] distances;
    public float timeToChangeStrat;
    public float timeToBeVulnerable;
    public bool headsToSpawn;
    public int maxHealthHead;
    public Material glowHeadPhase3;
    public float healthPercentageHeadPhase2;
    public float healthPercentageHeadPhase3;
    public GameObject forceField;
    private TilemapRenderer tmr;
    public Material glowEnviro1;
    public Material glowEnviro2;
    public Material glowEnviro3;
    private bool vulnerableCoroutine = false;


    private void Start()
    {
        enemy = GetComponent<ELC_Enemy>();
        currentPhase = BossPhase.Phase1;
        enemy.isInvulnerable = true;
        forceField = this.gameObject.transform.GetChild(7).gameObject;
        tmr = FindObjectOfType<TilemapRenderer>();
        tmr.material = glowEnviro1;
        LetsFight();
    }
    private void Update()
    {
        if(Time.time >= timeToChangeStrat && timeToChangeStrat > 0)
        {
            ChangeStratRandom();
        }

        if (!enemy.isInvulnerable)
        {
            VulnerablePhase();
        }
        else if (headsToSpawn)
        {
            SpawnHeads();
        }
    }
    public void SpawnHeads()
    {
        Debug.Log("Spawning heads");
        switch (currentPhase) {
            case BossPhase.Phase1:
                AXD_HydraHead temp1 = Instantiate(head1Prefab, this.gameObject.transform.GetChild(0));
                temp1.enemyScript.currentHealth = temp1.headStats.MaxHealth = maxHealthHead;
                temp1.GetHydraRef(this);
                heads.Add(temp1);
                break;

            case BossPhase.Phase2:
                AXD_HydraHead temp2 = Instantiate(head1Prefab, this.gameObject.transform.GetChild(1));
                temp2.headStats.MaxHealth = temp2.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase2 / 100);
                temp2.GetHydraRef(this);
                heads.Add(temp2);
                AXD_HydraHead temp3 = Instantiate(head2Prefab, this.gameObject.transform.GetChild(2));
                temp3.headStats.MaxHealth = temp3.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase2 / 100);
                temp3.GetHydraRef(this);
                heads.Add(temp3);
                break;

            case BossPhase.Phase3:
                AXD_HydraHead temp4 = Instantiate(head1Prefab, this.gameObject.transform.GetChild(3));
                temp4.headStats.MaxHealth = temp4.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                temp4.GetHydraRef(this);
                temp4.GetComponent<ELC_Enemy>().basicMat= glowHeadPhase3;
                heads.Add(temp4);
                AXD_HydraHead temp5 = Instantiate(head2Prefab, this.gameObject.transform.GetChild(4));
                temp5.headStats.MaxHealth = temp5.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                temp5.GetHydraRef(this);
                temp4.GetComponent<ELC_Enemy>().basicMat = glowHeadPhase3;
                heads.Add(temp5);
                AXD_HydraHead temp6 = Instantiate(head3Prefab, this.gameObject.transform.GetChild(5));
                temp6.headStats.MaxHealth = temp6.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                temp6.GetHydraRef(this);
                temp4.GetComponent<ELC_Enemy>().basicMat = glowHeadPhase3;
                heads.Add(temp6);
                AXD_HydraHead temp7 = Instantiate(head4Prefab, this.gameObject.transform.GetChild(6));
                temp7.headStats.MaxHealth = temp7.enemyScript.currentHealth = Mathf.RoundToInt(maxHealthHead * healthPercentageHeadPhase3 / 100);
                temp7.GetHydraRef(this);
                temp4.GetComponent<ELC_Enemy>().basicMat = glowHeadPhase3;
                heads.Add(temp7);
                break;
        }
        timeToChangeStrat = Time.time + stats.stratTime;
        headsToSpawn = false;
    }

    public void LoseHead(AXD_HydraHead head)
    {
        heads.Remove(head);
        if(heads.Count == 0)
        {
            enemy.isInvulnerable = false;
            vulnerableCoroutine = true;
            timeToBeVulnerable = Time.time + stats.vulnerableTime;
        }
        else
        {
            ChangeStratRandom();
        }
    }

    public void VulnerablePhase()
    {
        Debug.Log("Attack me !");
        forceField.SetActive(false);
        if (Time.time > timeToBeVulnerable || enemy.currentHealth < stats.healthPhase[(int)currentPhase])
        {
            if (enemy.currentHealth < stats.healthPhase[(int)currentPhase])
            {
                enemy.currentHealth = stats.healthPhase[(int)currentPhase];
            }
            if (vulnerableCoroutine)
            {
                StartCoroutine(BecomeInvunerable());
            }
        }
    }

    public void ChangePhase()
    {
        if(currentPhase < BossPhase.Phase3)
        {
            Debug.Log("Changing phase");
            currentPhase++;
            if(currentPhase == BossPhase.Phase2)
            {
                tmr.material = glowEnviro2;
            }
            else if(currentPhase == BossPhase.Phase3)
            {
                tmr.material = glowEnviro3;
            }
        }
    }

    public void ChangeStratRandom()
    {
        
        int ran;
        foreach(AXD_HydraHead head in heads)
        {
            ran = Mathf.RoundToInt(Random.Range(0, 2));
            switch (ran)
            {
                case 0:
                    head.Charge();
                    break;
                case 1:
                    head.Shoot();
                    break;
            }
        }
        timeToChangeStrat = Time.time + stats.stratTime;
    }

    public void LetsFight()
    {
        headsToSpawn = true;
    }

    IEnumerator BecomeInvunerable()
    {
        vulnerableCoroutine = false;
        FindObjectOfType<PlayerHealth>().GetHit(0, 15, 0);
        yield return new WaitForSeconds(1);
        forceField.SetActive(true); ;
        enemy.isInvulnerable = true;
        headsToSpawn = true;
        ChangePhase();
        vulnerableCoroutine = true;
    }
}