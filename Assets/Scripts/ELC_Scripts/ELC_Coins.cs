using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Coins : MonoBehaviour
{
    public ELC_Detector detectorScript;
    GameObject player;
    public float attractDistance;
    private Vector3 direction;
    public float speed;
    public int value;
    float distanceSpeedMultiplicator;
    public AudioSource coinSound;

    public float initialDropSpeed = 1;
    public float decreaseSpeedOnDrop;

    bool isFalling;


    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        Drop();
    }

    void Drop()
    {
        isFalling = true;
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    
    void Update()
    {
        if(isFalling)
        {
            this.transform.Translate(direction * Time.deltaTime * initialDropSpeed);
            if (initialDropSpeed > 0) initialDropSpeed -= decreaseSpeedOnDrop;
            else isFalling = false;
        }

        if (!isFalling && Vector3.Distance(player.transform.position, this.transform.position) < attractDistance)
        {
            distanceSpeedMultiplicator = attractDistance - Vector3.Distance(player.transform.position, this.transform.position);
            direction = new Vector3( player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y);
            this.transform.Translate(direction * Time.deltaTime * speed * distanceSpeedMultiplicator);
        }

        if (!isFalling && detectorScript.playerIsInside)
        {
            FindObjectOfType<ELC_ObjectsInventory>().AddMoneyToCrates(value);
            StartCoroutine(Audio());            
        }
    }
    IEnumerator Audio()
    {
        if (!coinSound.isPlaying)
        {
            coinSound.Play();
            yield return new WaitForSeconds(0.6f);
            Destroy(this.gameObject);
        }        
    }



}
