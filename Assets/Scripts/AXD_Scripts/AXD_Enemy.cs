using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_Enemy : MonoBehaviour
{
    public int maxHP;
    private int HP;
    public bool isDead;
    private Collider2D col;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void GetHit(int Damage)
    {
        Debug.Log("Argh");
        HP -= Damage;
        if (HP <= 0)
        {
            col.enabled = false;
            sr.enabled = false;
            isDead = true;
        }
    }
}
