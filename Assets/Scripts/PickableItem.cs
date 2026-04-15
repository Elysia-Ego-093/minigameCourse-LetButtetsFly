using UnityEngine;

public class PickableItem : InteractiveStuff
{
    public ItemType itemType;
    private PlayerStatus currentPlayer;

    protected override void Awake()
    {
        isPickable = true;
        base.Awake();
    }

    // 2D 触发（必须用这个！）
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ 玩家碰到道具了！");

            PlayerStatus player = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                currentPlayer = player;
                Invoke(nameof(DelayedPickUp), 0.3f);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CancelInvoke(nameof(DelayedPickUp));
            currentPlayer = null;
            Debug.Log("⚠️ 玩家离开道具");
        }
    }

    void DelayedPickUp()
    {
        if (currentPlayer != null)
            PickUp(currentPlayer);
    }

    void PickUp(PlayerStatus player)
    {
        Debug.Log("🎒 成功拾取道具！");

        isDestroyed = true;

        switch (itemType)
        {
            case ItemType.HealBox:
                Debug.Log("❤️ 触发回血");
                player.HealthRecovery(300);
                break;

            case ItemType.Shield:
                Debug.Log("🛡️ 触发护盾");
                player.GetShield();
                break;
        }

        Destroy(gameObject);
    }
}

public enum ItemType
{
    Weapon,
    Shield,
    HealBox
}