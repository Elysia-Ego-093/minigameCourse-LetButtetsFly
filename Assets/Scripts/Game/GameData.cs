using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public int character_id;
    public string PlayerName;
    [Range(0, 10000)] public float maxHp;
    [Range(0, 200)] public float maxMp;
    [Range(0, 20)] public float moveSpeed;
    [Range(0, 20)] public float jumpForce;
    [Range(0, 0.2f)] public float reloadSpeed;
    public GameObject gun;
    public Texture textureHead;
    public Texture textureBody;
    public Texture textureHand;
    public PlayerData(PlayerData player){
        character_id = player.character_id;
        PlayerName = player.PlayerName;
        maxHp = player.maxHp;
        moveSpeed = player.moveSpeed;
        jumpForce = player.jumpForce;
        maxMp = player.maxMp;
        reloadSpeed = player.reloadSpeed;
        gun = player.gun;
        textureHead = player.textureHead;
        textureBody = player.textureBody;
        textureHand = player.textureHand;
    }
    public void ClearData()
    {
        PlayerName = "";
        maxHp = 0f;
        moveSpeed = 0f;
        jumpForce = 0f;
        maxMp = 0f;
        reloadSpeed = 0f;
        gun = null;
    }
}

[System.Serializable]
public class GunData_from_web
{
    public float fireRate;
    public float bulletSpeed;
    public float bulletATK;
    public float force_x;
    public float force_y;
    public bool needAmmo;
    public int maxAmmo;
    public int BasicAmmoNum;
    public float AmmoTime;
    public string gunName;
    public GunData_from_web() { }
    public GunData_from_web(float fireRate, float bulletSpeed, float bulletATK, float force_x, float force_y, bool needAmmo, int maxAmmo, int basicAmmoNum, float ammoTime, string name)
    {
        this.fireRate = fireRate;
        this.bulletSpeed = bulletSpeed;
        this.bulletATK = bulletATK;
        this.force_x = force_x;
        this.force_y = force_y;
        this.needAmmo = needAmmo;
        this.maxAmmo = maxAmmo;
        BasicAmmoNum = basicAmmoNum;
        AmmoTime = ammoTime;
        gunName = name;
    }
}

[System.Serializable]
public class BuffData_form_web
{
    public float time;
    public float percent;
    public BuffData_form_web() { }
    public BuffData_form_web(float time, float percent)
    {
        this.time = time;
        this.percent = percent;
    }
}
[System.Serializable]
public class CharacterData_from_web
{
    [Range(0, 10000)] public float maxHp;
    [Range(0, 200)] public float maxMp;
    [Range(0, 20)] public float moveSpeed;
    [Range(0, 20)] public float jumpForce;
    [Range(0, 0.2f)] public float reloadSpeed;
    public CharacterData_from_web() { }
    public CharacterData_from_web(float maxHp, float maxMp, float moveSpeed, float jumpForce, float reloadSpeed)
    {
        this.maxHp = maxHp;
        this.maxMp = maxMp;
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;
        this.reloadSpeed = reloadSpeed;
    }
}


public class GameData : MonoBehaviour
{
    public int uid = -1;
    public static GameData Instance;
    private float UpdateDatasTimer = 10f;

    public List<GunData_from_web> GunDatas_from_web = new List<GunData_from_web>();
    public List<CharacterData_from_web> CharacterDatas_from_web = new List<CharacterData_from_web>();
    public BuffData_form_web big_buff;
    public BuffData_form_web small_buff;
    public BuffData_form_web fast_buff;
    public BuffData_form_web slow_buff;
    public BuffData_form_web jump_buff;

    private AudioSource BGM;

    [Header("音量设置")]
    [Range(0f, 1f)] public float BGMVolume;
    [Range(0f, 1f)] public float SoundVolume;

    [Header("选中的玩家数据")]
    public int PlayerCount;
    public List<PlayerData> players = new List<PlayerData>();
    [Header("胜利者")]
    public string winnerName=null;
    [Header("胜利分数")]
    public int winnerScore = 3;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this) 
        {
            Destroy(gameObject);
        }


        BGM = GetComponent<AudioSource>();
        for (int i = 0; i < PlayerCount; i++) players.Add(null);

        addCharacterData();
        addGunData();
        initialBuff();

        Game_api.Instance.RequestGameData();
        UpdateDatasTimer = 5f;
    }
    private void Update()
    {
        RequestGameData();

        string currentSecneName = SceneManager.GetActiveScene().name;
        if (!(currentSecneName == "MainTheme" 
            || currentSecneName == "SelectPlayer" 
            || currentSecneName == "SelectMap"))
        {
            BGM.Stop();
        }
        else if (!BGM.isPlaying) BGM.Play();
        BGM.volume = BGMVolume * 0.5f;
    }

    public void ClearPlayerData()
    {
        foreach(var player in players)
        {
            player.ClearData();
        }
    }

    private void RequestGameData()
    {
        if (UpdateDatasTimer >= 0)
        {
            UpdateDatasTimer -= Time.deltaTime;
        }
        else
        {
            UpdateDatasTimer = 5f;
            Game_api.Instance.RequestGameData();
        }
    }


    private void addGunData()
    {
        GunDatas_from_web.Add(new GunData_from_web(5f, 25f, 3f, 6f, 5f, false, 20, 0, 0, "Pistol"));
        GunDatas_from_web.Add(new GunData_from_web(24f, 45f, 4.5f, 20f, 7.5f, true, 60, 120, 2f, "M416"));
        GunDatas_from_web.Add(new GunData_from_web(27.5f, 40f, 4.2f, 20f, 7.5f, true, 60, 120, 2f, "MachineGun"));
        GunDatas_from_web.Add(new GunData_from_web(2f, 70f, 20f, 50f, 10f, true, 20, 40, 5f, "Rifle"));
        GunDatas_from_web.Add(new GunData_from_web(30f, 50f, 4f, 18f, 7.3f, true, 80, 160, 1.7f, "SMG"));
        GunDatas_from_web.Add(new GunData_from_web(1f, 100f, 50f, 100f, 20f, true, 10, 20, 10f, "Sniper_Rifle"));
        GunDatas_from_web.Add(new GunData_from_web(6f, 40f, 23.3f, 3f, 5f, true, 648, 0, 0f, "SpecialPistol"));
    }
    
    private void addCharacterData()
    {
        CharacterDatas_from_web.Add(new CharacterData_from_web(1000f, 40f, 8f, 12.3f, 0.075f));
        CharacterDatas_from_web.Add(new CharacterData_from_web(900f, 50f, 8.3f, 12.8f, 0.2f));
        CharacterDatas_from_web.Add(new CharacterData_from_web(1200f, 31f, 7f, 12.3f, 0.025f));
        CharacterDatas_from_web.Add(new CharacterData_from_web(1050f, 39f, 7.8f, 12.5f, 0.1f));
    }

    private void initialBuff()
    {
        big_buff = new BuffData_form_web(10f, 1.5f);
        small_buff = new BuffData_form_web(8f, 0.6f);
        fast_buff = new BuffData_form_web(8f, 1.5f);
        slow_buff = new BuffData_form_web(6f, 0.3f);
        jump_buff = new BuffData_form_web(10f, 1.5f);
    }
}
