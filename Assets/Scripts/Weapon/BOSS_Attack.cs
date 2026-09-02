using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Attack : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D col;
    private Collider2D ownerCollider;
    private int Damage_id = 13;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        Damage_id = 13;
    }
    void Update()
    {
        // ×Ô¶¯Ïú»Ù
        if (transform.position.x > 300
            || transform.position.x < -300
            || transform.position.y < -10
            || transform.position.y > 100)
            Destroy(gameObject);

    }
    public void SetStatus(Collider2D owner)
    {
        if (owner != null)
        {
            ownerCollider = owner;
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ownerCollider, true);
        }
        rb.velocity = new Vector2(-150f, 0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && !player.IsSprinting())
            {
                player.Attacked(Damage_id, 100, new Vector2(-30f, 10f));
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
            }
        }
    }
}
