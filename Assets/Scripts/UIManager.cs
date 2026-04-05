using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("玩家")]
    public PlayerController player;

    [Header("血条")]
    public Slider bloodBar;

    [Header("体力条")]
    public Slider StaminaBar;

    void Start()
    {

    }
    void Update()
    {
        UpdateBlood();
        UpdateStamina();
    }
    private void UpdateBlood()
    {
        bloodBar.maxValue = player.maxBlood;
        bloodBar.value = player.blood;
    }
    private void UpdateStamina()
    {
        StaminaBar.maxValue = player.maxStamina;
        StaminaBar.value = player.currentStamina;
    }
}
