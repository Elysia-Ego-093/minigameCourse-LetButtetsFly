using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Boomb_2 : Explosive
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Collider2D ownerCollider;
    private const float speed = 35;
    private int cnt = 0;
    protected override void CheckDetonation() { return; }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        Damage_id = 16;
        float Angle = 180f + 70f;
        transform.Rotate(0, 0, Angle);
    }
    private void Update()
    {
        float angle = (180f + 70f) / 180f * Mathf.PI;
        rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
        if (transform.position.x > 300
            || transform.position.x < -300
            || transform.position.y < -10
            || transform.position.y > 100)
            Destroy(gameObject);
    }
    public void SetStatus(int cnt, Collider2D owner)
    {
        this.cnt = cnt;
        if (owner != null)
        {
            ownerCollider = owner;
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ownerCollider, true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlatformEffector2D>() != null)
        {
            if (cnt > 0)
            {
                cnt--;
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ownerCollider, true);
            }
            else
            {
                Explode();
            }
            return;
        }
        if (collision.CompareTag("Player"))
        {
            Explode();
            return;
        }
    }
}
