using UnityEngine;

public class BulletBox : InteractiveStuff
{
    public BulletType bulletType;
    public int bulletAmount = 30;
    protected override void Awake()
    {
        isPickable = true ;
        base.Awake();
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if(isDestroyed) return;
        if (other.CompareTag("Player"))
        {
            BulletBoxPickUp();
        }
    }

    protected void BulletBoxPickUp()
    {
        isDestroyed = true;
        //具体功能---------------------------------------------------------------------
        





        //
        Destroy(gameObject);
    }

    public enum BulletType
    {
        Pistol,
        Rifle,
        Sniper
    }

}