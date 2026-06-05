using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    //生命值---------------------------------------------------------
    public float maxHp;
    public float currentHp;

    //盾----------------------------------------------------------------

    public float currentShield;
    public float ShieldAmount;

    [Header("受击音效")]
    public AudioClip AttackSound;
    public AudioClip ShieldSound;

    void Awake()
    {   

        maxHp = 1000f;
        ShieldAmount = maxHp*0.3f;
        currentHp = maxHp;
        currentShield = 0;
    }

    // 加血
    public void HealthRecovery(float amount) { currentHp = Mathf.Min(currentHp+amount,maxHp); }
    //加盾
    public void ShieldRecovery(float amount) { currentShield += amount; }

    
    public void GetShield()
    {   
            
            currentShield = ShieldAmount;
        
    }

    public void TakeDamage(float damage)
    {
        if (currentShield > 0)
        {
            if(damage <= currentShield)
            {
                currentShield -= damage;
                damage = 0;
            }

            else
            {
                damage -= currentShield;
                currentShield = 0;
            }
        }
        
        currentHp = Mathf.Max(currentHp-damage , 0);

        if (currentShield > 0)
        {
            if (ShieldSound != null)
            {
                AudioSource.PlayClipAtPoint(ShieldSound, transform.position, 1.0f);
            }
        }
        else
        {
            if (AttackSound != null)
            {
                AudioSource.PlayClipAtPoint(AttackSound, transform.position, 1.0f);
            }
        }
        
    }
}