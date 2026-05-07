using UnityEngine;

public class CoverBox : InteractiveStuff
{
    public bool isBreakable = false;
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
        if (!isBreakable) return;

        coverHealth -= damage;

        if (coverHealth <= 0)
        {
            BreakCover();
        }
    }
}
