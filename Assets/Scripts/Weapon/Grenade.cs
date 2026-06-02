using UnityEngine;

public class Grenade : Explosive
{
    [Header("±¬Õ¨ÑÓÊ±")]
    public float fuseTime;

    [Header("±¬Õ¨¾¯Ê¾È¦")]
    public GameObject Background;
    public GameObject Cover;

    private Rigidbody2D rb;
    private float spawnTime = 0f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!hasExploded)
        {
            CheckDetonation();
        }
        spawnTime += Time.deltaTime;
        Debug.Log(spawnTime);
        Background.transform.position = transform.position;
        Background.transform.localScale = new Vector2(explosionRadius * 2f / transform.localScale.x, explosionRadius * 2f / transform.localScale.y);
        Cover.transform.position = transform.position;
        float r0 = explosionRadius * (spawnTime / fuseTime);
        Cover.transform.localScale = new Vector2(r0 * 2f / transform.localScale.x, r0 * 2f / transform.localScale.y);
    }
    protected override void CheckDetonation()
    {
        if (spawnTime >= fuseTime)
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
