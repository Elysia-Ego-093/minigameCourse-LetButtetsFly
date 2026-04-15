using UnityEngine;

public class InteractiveStuff : MonoBehaviour
{
    public bool isPickable = false;
    public float coverHealth = 3000f;
    public bool isDestroyed = false;

    // 修改：适配2D碰撞体
    protected Collider2D objectCollider2D;

    protected virtual void Awake()
    {
        // 获取2D碰撞体
        objectCollider2D = GetComponent<Collider2D>();
        SetupCollision();
    }

    /// <summary>
    /// 自动设置2D碰撞体：
    /// 可拾取 → 不阻挡（Trigger）
    /// 不可拾取 → 阻挡玩家、挡子弹（非Trigger）
    /// </summary>
    protected virtual void SetupCollision()
    {
        if (objectCollider2D == null) return;

        if (isPickable)
        {
            // 可拾取：Trigger，不阻挡
            objectCollider2D.isTrigger = true;
            objectCollider2D.enabled = true;
        }
        else
        {
            // 掩体：非Trigger，阻挡
            objectCollider2D.isTrigger = false;
            objectCollider2D.enabled = true;
        }
    }

    /// <summary>
    /// 受伤害（仅掩体可用）
    /// </summary>
    public virtual void TakeDamage(float damage)
    {
        // 可拾取物品不能被打
        if (isPickable) return;
        if (isDestroyed) return;

        coverHealth -= damage;
        Debug.Log($"掩体剩余血量：{coverHealth}");

        if (coverHealth <= 0)
        {
            BreakCover();
        }
    }

    /// <summary>
    /// 掩体被打碎
    /// </summary>
    protected virtual void BreakCover()
    {
        isDestroyed = true;
        Debug.Log("掩体被打碎");

        // 打碎后关闭碰撞，不再阻挡
        if (objectCollider2D != null)
            objectCollider2D.enabled = false;
    }

    /// <summary>
    /// 玩家交互（子类重写）
    /// </summary>
    public virtual void OnPlayerInteract()
    {
        // 空实现
    }
}