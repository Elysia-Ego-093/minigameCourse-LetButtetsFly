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

    [Header("子弹显示")]
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
            ammoText.text = $"□□: {currentAmmo} / {maxAmmo}";
            
            // 子弹不足时改变颜色
            if (currentAmmo == 0)
            {
                ammoText.color = Color.red;
            }
            else if (currentAmmo <= maxAmmo * 0.2f) // 剩余20%以下变黄
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
            // 无限弹药模式
            ammoText.text = $"子弹: ∞";
            ammoText.color = Color.cyan;
        }
    }
}