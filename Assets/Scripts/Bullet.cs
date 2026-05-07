using Unity.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float ATK;
    private Vector2 force;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void SetStatus(float s,float lastMoveDirection,float atk,float force_x,float force_y)
    {
        speed = s;
        rb.velocity =new Vector2(lastMoveDirection * speed,0);
        ATK = atk;
        force = new Vector2(force_x * lastMoveDirection, force_y);
    }

    void Update()
    {
        // 自动销毁
        if (transform.position.x > 12 || transform.position.x < -12)
            Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController Player = collision.GetComponent<PlayerController>();
            if (Player != null)
            {
                Player.Attacked(ATK, force);
                Destroy(gameObject);
            }
        }
    }
}