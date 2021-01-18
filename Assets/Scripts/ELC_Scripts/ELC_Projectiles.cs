using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Projectiles : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float lifeDuration;
    public float strenght;
    public ELC_EnemySO enemy;

    public bool IsFriendly;


    void Update()
    {
        this.gameObject.transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    public IEnumerator LifeDuration()
    {
        yield return new WaitForSeconds(lifeDuration);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().GetHit(enemy, (int)strenght);
            Destroy(this.gameObject);
        }
        if(IsFriendly && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<ELC_Enemy>().GetHit((int)strenght, collision.GetComponent<ELC_Enemy>().movesTowardPlayer);
        }
    }

    public void EgideEffect()
    {
        if (!IsFriendly)
        {
            int randomNumber = Random.Range(0, 101);
            if (randomNumber < FindObjectOfType<ELC_PassivesProperties>().EgidePercentageChanceToSendBackProjectile)
            {
                IsFriendly = true;
                direction = -direction;
            }
            else Destroy(this.gameObject);
        }
    }

}
