using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("ç©å®¶")]
    public PlayerController player;

    [Header("è¡æ¡")]
    public Slider bloodBar;

    [Header("ä½åæ¡")]
    public Slider staminaBar;

    [Header("å­å¼¹æ¾ç¤º")]
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
