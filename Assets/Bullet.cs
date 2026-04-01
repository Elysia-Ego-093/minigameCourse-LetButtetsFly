using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float speed;
    private Rigidbody2D rb;

    public float fireRate;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void SetSpeed(float s,float lastMoveDirection)
    {
        speed = s;
        rb.velocity =new Vector2(lastMoveDirection * speed,0);
    }

    public virtual void setFireRate(float rate)
    {
        fireRate = rate;
    }

    void Update()
    {
        // 自动销毁
        if (transform.position.x > 12 || transform.position.x < -12)
            Destroy(gameObject);
    }
}