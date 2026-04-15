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
        if (player != null && bloodBar != null)
        {
            bloodBar.maxValue = player.maxBlood;
            bloodBar.value = player.blood;
        }
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
            ammoText.text = $"â¡â¡: {currentAmmo} / {maxAmmo}";
            
            // å­å¼¹ä¸è¶³æ¶æ¹åé¢è²
            if (currentAmmo == 0)
            {
                ammoText.color = Color.red;
            }
            else if (currentAmmo <= maxAmmo * 0.2f) // å©ä½20%ä»¥ä¸åé»
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
            // æ éå¼¹è¯æ¨¡å¼
            ammoText.text = $"å­å¼¹: â";
            ammoText.color = Color.cyan;
        }
    }
}
