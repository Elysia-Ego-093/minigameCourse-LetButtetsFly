using Unity.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float ATK;
    private Vector2 force;
    private Rigidbody2D rb;
    private BoxCollider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
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
        col.size = new Vector2(Mathf.Abs(rb.velocity.x * 0.2f), col.size.y);
        // 自动销毁
        if (transform.position.x > 100 || transform.position.x < -100|| transform.position.y < -10)
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

        else if (collision.CompareTag("Obstacle"))
        {
            // 找到掩体的InteractiveStuff组件（CoverBox是其子类）
            CoverBox cover = collision.GetComponent<CoverBox>();
            if (cover != null)
            {
                // 调用掩体扣血方法，子弹攻击力作为伤害值
                cover.TakeDamage(ATK);
            }
            Destroy(gameObject); // 击中掩体后销毁子弹
        }
    }
}