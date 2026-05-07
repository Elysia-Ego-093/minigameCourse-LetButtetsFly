using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Explosive : MonoBehaviour
{
    [Header("±¬Õ¨°ë¾¶")]
    public float explosionRadius;
    [Header("±¬Õ¨ÉËº¦")]
    public float Damage;
    [Header("±¬Õ¨»÷ÍËÁ¦¶È")]
    public float Force;

    public LayerMask targetLayer;
    protected bool hasExploded = false;

    protected abstract void CheckDetonation();

    void Update()
    {
        if (!hasExploded)
        {
            CheckDetonation();
        }
    }
    protected virtual void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach(Collider2D col in cols)
        {
            PlayerController player = col.GetComponent<PlayerController>();
            float distance = Vector2.Distance(transform.position, col.transform.position);
            Vector2 knockBackDirection = (col.transform.position - transform.position).normalized;
            float percent = Mathf.Max(1 - distance / explosionRadius, 0);
            if (player != null)
            {
                player.Attacked(Damage * percent, knockBackDirection * Force * percent);
            }
            else
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = knockBackDirection * Force * percent;
                    if (rb.gravityScale == 0) rb.gravityScale = 1.0f;
                }
            }
        }
        Destroy(gameObject);
    }
}
