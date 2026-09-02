using FantasyBattlegroundsPixelArtOriginal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Explosive : MonoBehaviour
{
    [Header("爆炸半径")]
    public float explosionRadius;
    [Header("爆炸伤害")]
    public float Damage;
    protected int Damage_id;
    [Header("爆炸力度")]
    public float Force;
    [Header("音效")]
    public AudioClip explosionSound;
    [Header("爆炸动画预制体")]
    public GameObject AnimationPrefab;
    protected float AnimationSize;

    public LayerMask targetLayer;

    protected bool hasExploded = false;

    protected abstract void CheckDetonation();

    void Update()
    {
        if (!hasExploded)
        {
            CheckDetonation();
        }
        // 自动销毁
        if (transform.position.x > 300
            || transform.position.x < -300
            || transform.position.y < -10
            || transform.position.y > 300)
            Destroy(gameObject);
    }

    protected virtual void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, GameData.Instance.SoundVolume);
        }
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach(Collider2D col in cols)
        {
            PlayerController player = col.GetComponent<PlayerController>();
            BOSS boss = col.GetComponent<BOSS>();
            if (player != null && !player.IsSprinting())
            {
                float distance = Vector2.Distance(transform.position, player.CenterPosition);
                Vector2 knockBackDirection = (player.CenterPosition - (Vector2)transform.position).normalized;
                float percent = Mathf.Max(1 - distance / explosionRadius, 0.01f);
                player.Attacked(Damage_id, Damage, knockBackDirection * Force * percent);
            }
            else if (boss != null)
            {
                boss.attcked(Damage_id, Damage);
            }
            else
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    float distance = Vector2.Distance(transform.position, col.transform.position);
                    Vector2 knockBackDirection = (col.transform.position - transform.position).normalized;
                    float percent = Mathf.Max(1 - distance / explosionRadius, 0.01f);
                    rb.velocity = knockBackDirection * Force * percent;
                    if (rb.gravityScale == 0) rb.gravityScale = 1.0f;
                }
            }
        }
        GameObject newAnimation = Instantiate(AnimationPrefab, transform.position, Quaternion.identity);
        newAnimation.transform.localScale = new Vector2(3f * explosionRadius, 3f * explosionRadius);
        Destroy(gameObject);
    }
}
