using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_UI_Manager : MonoBehaviour
{
    [Header("玩家")]
    public PlayerController player;

    [Header("血条")]
    public Slider bloodBar;

    [Header("体力条")]
    public Slider staminaBar;

    [Header("盾条")]
    public Slider shieldBar;

    [Header("手榴弹数量")]
    public TMP_Text grenadeCount;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBlood();
        UpdateStamina();
        UpdateShield();
        UpdateGrenade();
    }

    private void UpdateBlood()
    {
        if (player == null || bloodBar == null) return;
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

    private void UpdateShield()
    {
        if (player != null && shieldBar != null)
        {
            shieldBar.maxValue = player.PlayerStatus.maxHp;
            shieldBar.value = Mathf.Min(shieldBar.maxValue, player.PlayerStatus.currentShield);
        }
    }

    private void UpdateGrenade()
    {
        if (player != null && grenadeCount != null)
        {
            grenadeCount.text = $"*{player.GrenadeCount}";
        }
    }

}
