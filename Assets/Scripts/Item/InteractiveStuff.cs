using UnityEngine;

public class InteractiveStuff : MonoBehaviour
{
    protected bool isPickable = false;
    protected float coverHealth = 3000f;
    protected bool isDestroyed = false;

    [Header("物理碰撞体")]
    public Collider2D objectCollider2D;

    protected virtual void Awake()
    {
        objectCollider2D = GetComponent<Collider2D>();
        if (objectCollider2D != null) objectCollider2D.isTrigger = false;
    }
    private void Update()
    {
        if (transform.position.y < -20) Destroy(gameObject);
    }

    

    protected virtual void BreakCover()
    {
        isDestroyed = true;

        // 打碎后关闭碰撞，不再阻挡
        if (objectCollider2D != null)
            objectCollider2D.enabled = false;
    }

    public virtual void OnPlayerInteract()
    {
        // 空实现
    }
}