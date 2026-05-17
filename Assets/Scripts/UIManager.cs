using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("玩家总数")]
    public int PlayerCount;

    [Header("胜利分数")]
    public int MaxScore;

    [Header("玩家")]
    public List<PlayerController> players = new List<PlayerController>();

    [Header("血条")]
    public List<Slider> bloodBars = new List<Slider>();

    [Header("体力条")]
    public List<Slider> staminaBars = new List<Slider>();

    [Header("子弹")]
    public List<TMP_Text> ammoTexts = new List<TMP_Text>();

    [Header("分数")]
    public List<int> scores= new List<int>();
    void Start()
    {
        IntInitialization();
        Update();
    }

    void IntInitialization()
    {
        //PlayerCount = playCount;
        //MaxScore = maxScore;
        for (int i = 0; i < PlayerCount; i++)
        {
            scores.Add(0);
        }
    }

    void Update()
    {
        UpdateBlood();
        UpdateStamina();
        UpdateAmmo();
    }

    private void UpdateBlood()
    {
        for(int i = 0; i < PlayerCount; i++)
        {
            if (players[i] == null || bloodBars[i] == null || players[i].PlayerStatus == null)
                return;

            bloodBars[i].maxValue = players[i].PlayerStatus.maxHp;
            bloodBars[i].value = players[i].PlayerStatus.currentHp;
        }
        
    }

    private void UpdateStamina()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            if (players[i] != null && staminaBars[i] != null)
            {
                staminaBars[i].maxValue = players[i].maxStamina;
                staminaBars[i].value = players[i].currentStamina;
            }
        }
            
    }

    private void UpdateAmmo()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            if (players[i] == null || ammoTexts[i] == null) return;

            int currentAmmo = players[i].GetCurrentAmmo();
            int AmmoNum = players[i].GetAmmoNum();
            int maxAmmo = players[i].GetMaxAmmo();
            bool needAmmo = players[i].IsNeedAmmo();
            System.Console.WriteLine(AmmoNum + "\n");
            System.Console.WriteLine(needAmmo + "\n");

            if (needAmmo != null)
            {
                ammoTexts[i].text = $"{currentAmmo} / {AmmoNum}";

                // Change text color when ammo is low
                if (currentAmmo == 0)
                {
                    ammoTexts[i].color = Color.red;
                }
                else if (currentAmmo <= maxAmmo * 0.2f) // Yellow when ammo is below 20%
                {
                    ammoTexts[i].color = Color.yellow;
                }
                else
                {
                    ammoTexts[i].color = Color.white;
                }
            }
            else
            {
                // Unlimited ammo mode
                ammoTexts[i].text = "Ammo: Infinity";
                ammoTexts[i].color = Color.cyan;
            }
        }
    }
}
