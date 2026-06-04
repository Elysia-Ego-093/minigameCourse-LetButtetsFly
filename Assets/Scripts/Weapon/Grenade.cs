using UnityEngine;

public class Grenade : Explosive
{
    [Header("��ը��ʱ")]
    public float fuseTime;

    [Header("��ը��ʾȦ")]
    public GameObject Background;
    public GameObject Cover;

    private Rigidbody2D rb;
    private float spawnTime = 0f;
    public AudioClip fuseSound;   
    private AudioSource audioSource;
    public AudioClip explosionSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = fuseSound;
        audioSource.loop = true;  
        audioSource.volume = 0.3f;
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
            StopFuseSound();
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1.5f);
            }
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
