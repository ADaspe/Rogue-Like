using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Projectiles : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float lifeDuration;
    public float strenght;


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
            collision.GetComponent<PlayerHealth>().GetHit((int)strenght);
            Destroy(this.gameObject);
        }
    }

}
