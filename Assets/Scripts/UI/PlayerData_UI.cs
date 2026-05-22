using FantasyBattlegroundsPixelArtOriginal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData_UI : MonoBehaviour
{
    [Header("角色数据")]
    public PlayerSelector players;

    [Header("角色头像")]
    public Image PlayerImage;
    private readonly string playerImageFloderPath = "picture/Player";

    [Header("血量")]
    public Slider HpBar;
    public TMP_Text HpValue;

    [Header("体力上限")]
    public Slider MpBar;
    public TMP_Text MpValue;

    [Header("速度")]
    public Slider SpeedBar;
    public TMP_Text SpeedValue;

    [Header("跳跃力度")]
    public Slider JumpBar;
    public TMP_Text JumpValue;

    [Header("换弹速度")]
    public Slider ReloadBar;
    public TMP_Text ReloadValue;

    [Header("枪械图标")]
    public Image GunImage;
    private readonly string gunImageFloderPath = "picture/Weapon";

    [Header("滑动速度")]
    public float LoadTime;
    private float Timer = 0f;

    private float maxHP = 0f, maxMp = 0f, maxSpeed = 0f, maxJump = 0f, maxReload = 0f;

    private void Start()
    {
        foreach(var player in players.players)
        {
            if (player != null)
            {
                maxHP = Mathf.Max(maxHP, player.maxHp);
                maxMp = Mathf.Max(maxMp, player.maxMp);
                maxSpeed = Mathf.Max(maxSpeed, player.moveSpeed);
                maxJump = Mathf.Max(maxJump, player.jumpForce);
                maxReload = Mathf.Max(maxReload, player.reloadSpeed);
            }
        }
        HpBar.maxValue = maxHP;
        MpBar.maxValue = maxMp;
        SpeedBar.maxValue = maxSpeed;
        JumpBar.maxValue = maxJump;
        ReloadBar.maxValue = maxReload;

        maxHP = 0f; maxMp = 0f; maxSpeed = 0f; maxJump = 0f; maxReload = 0f;
    }

    void Update()
    {
        if (Timer > 0f)
        {
            Timer -= Time.deltaTime;
            float percent = 1.0f - Timer / LoadTime;
            HpBar.value = maxHP * percent;
            HpValue.text = $"{maxHP * percent}";
            MpBar.value = maxMp * percent;
            MpValue.text = $"{maxMp * percent}";
            SpeedBar.value = maxSpeed * percent;
            SpeedValue.text = $"{maxSpeed * percent}";
            JumpBar.value = maxJump * percent;
            JumpValue.text = $"{maxJump * percent}";
            ReloadBar.value = maxReload * percent;
            ReloadValue.text = $"{maxReload * percent}";
            return;
        }
        if (Timer < 0f)
        {
            HpBar.value = maxHP;
            HpValue.text = $"{maxHP}";
            MpBar.value = maxMp;
            MpValue.text = $"{maxMp}";
            SpeedBar.value = maxSpeed;
            SpeedValue.text = $"{maxSpeed}";
            JumpBar.value = maxJump;
            JumpValue.text = $"{maxJump}";
            ReloadBar.value = maxReload;
            ReloadValue.text = $"{maxReload}";
            Timer = 0f;
        }
    }

    public void GetPlayer(int index)
    {
        Timer = LoadTime;

        Sprite newPlayerSprite = Resources.Load<Sprite>($"{playerImageFloderPath}/{players.players[index].PlayerName}");
        PlayerImage.sprite = newPlayerSprite;

        maxHP = players.players[index].maxHp;
        maxMp = players.players[index].maxMp;
        maxSpeed = players.players[index].moveSpeed;
        maxJump = players.players[index].jumpForce;
        maxReload = players.players[index].reloadSpeed;

        Gun newGun = players.players[index].gun.GetComponent<Gun>();
        if (newGun != null)
        {
            Sprite newGunSprite = Resources.Load<Sprite>($"{gunImageFloderPath}/{newGun.data.gunName}");
            GunImage.sprite = newGunSprite;
        }
    }

    public void ClearData()
    {
        Timer = 0f;
        PlayerImage.sprite = null;
        HpBar.value = 0f;
        MpBar.value = 0f;
        SpeedBar.value = 0f;
        JumpBar.value = 0f;
        ReloadBar.value = 0f;
        GunImage.sprite = null;
    }
}
