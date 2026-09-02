using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    //生命值---------------------------------------------------------
    public float maxHp;
    public float currentHp;

    //盾----------------------------------------------------------------

    public float currentShield;

    [Header("受击音效")]
    public AudioClip AttackSound;
    public AudioClip ShieldSound;

    void Awake()
    {
        currentShield = 0;
    }

    // 加血
    public void HealthRecovery(float amount) { currentHp = Mathf.Min(currentHp + amount, maxHp); }
    //加盾
    public void ShieldRecovery(float amount) { currentShield += amount; }

    public void StatusResponse(float currentHP, float currentShield)
    {
        currentHp = currentHP;
        this.currentShield = currentShield;
    }

    public void TakeDamage(float atk)
    {
        float tot = currentShield + currentHp;
        tot -= atk;
        if (tot > currentHp)
        {
            currentShield -= atk;
        }
        else
        {
            currentHp -= (atk - currentShield);
            if (currentHp < 0) currentHp = 0;
            currentShield = 0;
        }

        if (currentShield > 0)
        {
            if (ShieldSound != null)
            {
                AudioSource.PlayClipAtPoint(ShieldSound, transform.position, GameData.Instance.SoundVolume);
            }
        }
        else
        {
            if (AttackSound != null)
            {
                AudioSource.PlayClipAtPoint(AttackSound, transform.position, GameData.Instance.SoundVolume);
            }
        }
        
    }
}