using UnityEngine;

public class CoverBox : InteractiveStuff
{
    public bool isBreakable = false;
    [Header("“Ù–ß")]
    public AudioClip sound;
    protected override void Awake()
    {
        isPickable = false;
        base.Awake();
    }

    protected override void BreakCover()
    {
        base.BreakCover();
        Destroy(gameObject,0.1f);
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDestroyed) return;
        if (sound != null)
        {
            AudioSource.PlayClipAtPoint(sound, transform.position, GameData.Instance.SoundVolume);
        }
        if (!isBreakable) return;

        coverHealth -= damage;

        if (coverHealth <= 0)
        {
            BreakCover();
        }
    }
}
