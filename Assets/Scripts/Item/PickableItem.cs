using UnityEngine;

public abstract class PickableItem : InteractiveStuff
{
    [Header("触发器")]
    public Collider2D triggerCollider;

    [Header("拾取音效")]
    public AudioClip PickUpSound;

    private PlayerController currentPlayer;

    protected override void Awake()
    {
        isPickable = true;
        base.Awake();
        if (triggerCollider != null) triggerCollider.isTrigger = true;
    }

    // 2D 触发（必须用这个！）
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Effect(player);
            }
        }
    }
    protected abstract void Effect(PlayerController player);
}