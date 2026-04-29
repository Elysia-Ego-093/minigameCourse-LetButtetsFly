using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("玩家")]
    public PlayerController player;

    [Header("血条")]
    public Slider bloodBar;

    [Header("体力条")]
    public Slider staminaBar;

    [Header("子弹")]
    public TMP_Text ammoText;  

    void Start()
    {

    }

    void Update()
    {
        UpdateBlood();
        UpdateStamina();
        UpdateAmmo();
    }

    private void UpdateBlood()
{
    if (player == null || bloodBar == null || player.PlayerStatus == null)
        return;

    bloodBar.maxValue = player.PlayerStatus.maxHp;
    bloodBar.value = player.PlayerStatus.currentHp;
}

    private void UpdateStamina()
    {
        if (player != null && staminaBar != null)
        {
            staminaBar.maxValue = player.maxStamina;
            staminaBar.value = player.currentStamina;
        }
    }

    private void UpdateAmmo()
    {
        if (player == null || ammoText == null) return;
        
        int currentAmmo = player.GetCurrentAmmo();
        int maxAmmo = player.GetMaxAmmo();
        bool needAmmo = player.IsNeedAmmo();
        
        if (needAmmo)
        {
            ammoText.text = $"bullet: {currentAmmo} / {maxAmmo}";
            
            // Change text color when ammo is low
            if (currentAmmo == 0)
            {
                ammoText.color = Color.red;
            }
            else if (currentAmmo <= maxAmmo * 0.2f) // Yellow when ammo is below 20%
            {
                ammoText.color = Color.yellow;
            }
            else
            {
                ammoText.color = Color.white;
            }
        }
        else
        {
            // Unlimited ammo mode
            ammoText.text = "Ammo: Infinity";
            ammoText.color = Color.cyan;
        }
    }
}
