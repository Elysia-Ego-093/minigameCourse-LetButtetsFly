using UnityEngine;

public class Grenade : Explosive
{
    [Header("±¨’®—” ±")]
    public float fuseTime;

    private Rigidbody2D rb;
    private float spawnTime;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }
    protected override void CheckDetonation()
    {
        if (Time.time >= spawnTime + fuseTime)
        {
            Explode();
        }
    }
    public virtual void setStatus(Vector2 force)
    {
        if (rb != null)
        {
            rb.velocity = force;
            rb.angularVelocity = Random.Range(-360f, 360f);
        }
    }
}
