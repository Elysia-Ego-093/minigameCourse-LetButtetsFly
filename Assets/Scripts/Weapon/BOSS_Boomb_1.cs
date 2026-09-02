using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Boomb_1 : Explosive
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Collider2D ownerCollider;
    private GameObject Player1;
    private GameObject Player2;
    private const float speed = 50;
    protected override void CheckDetonation() { return; }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        Damage_id = 15;

        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
    }

    void Update()
    {
        
    }
    public void SetStatus(Collider2D owner)
    {
        if (owner != null)
        {
            ownerCollider = owner;
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ownerCollider, true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Explode();
        }
    }
    public void Attack()
    {
        Vector2 direction = Vector2.zero;
        if (Player1 != null && Player2 != null)
        {
            float distance1 = Vector2.Distance(transform.position, Player1.transform.position);
            float distance2 = Vector2.Distance(transform.position, Player2.transform.position);
            if(distance1 > distance2)
            {
                direction = (Player2.transform.position - transform.position).normalized;
            }
            else
            {
                direction = (Player1.transform.position - transform.position).normalized;
            }
        }
        else if(Player1 != null)
        {
            direction = (Player1.transform.position - transform.position).normalized;
        }
        else if(Player2 != null)
        {
            direction = (Player1.transform.position - transform.position).normalized;
        }

        rb.velocity = direction * speed;
    }
}
