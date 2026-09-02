using UnityEngine;

public class Grenade : Explosive
{
    [Header("爆炸延时")]
    public float fuseTime;

    [Header("爆炸警示圈")]
    public GameObject Background;
    public GameObject Cover;

    private Rigidbody2D rb;
    private float spawnTime = 0f;
    public AudioClip fuseSound;

    [Header("音效")]
    private AudioSource audioSource;

    void Awake()
    {
        Damage_id = 7;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = fuseSound;
        audioSource.loop = true;
        audioSource.volume = GameData.Instance.SoundVolume * 0.3f;
        audioSource.Play();
    }
    public void PlayFuseSound()
    {
        if (audioSource != null && fuseSound != null)
        {
            audioSource.clip = fuseSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (!hasExploded)
        {
            CheckDetonation();
        }
        spawnTime += Time.deltaTime;
        Background.transform.position = transform.position;
        Background.transform.localScale = new Vector2(explosionRadius * 2f / transform.localScale.x, explosionRadius * 2f / transform.localScale.y);
        Cover.transform.position = transform.position;
        float r0 = explosionRadius * (spawnTime / fuseTime);
        Cover.transform.localScale = new Vector2(r0 * 2f / transform.localScale.x, r0 * 2f / transform.localScale.y);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach(var col in cols)
        {
            BOSS boss = col.GetComponent<BOSS>();
            if (boss != null) spawnTime = fuseTime;
        }
    }
    protected override void CheckDetonation()
    {
        if (spawnTime >= fuseTime)
        {
            StopFuseSound();
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
    public void StopFuseSound()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }
}
