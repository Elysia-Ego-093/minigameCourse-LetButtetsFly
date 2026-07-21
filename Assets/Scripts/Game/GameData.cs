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
public class SQLGunData
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
    public SQLGunData(float fireRate, float bulletSpeed, float bulletATK, float force_x, float force_y, bool needAmmo, int maxAmmo, int basicAmmoNum, float ammoTime, string name)
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
public class SQLCharacterData
{
    [Range(0, 10000)] public float maxHp;
    [Range(0, 200)] public float maxMp;
    [Range(0, 20)] public float moveSpeed;
    [Range(0, 20)] public float jumpForce;
    [Range(0, 0.2f)] public float reloadSpeed;
    public SQLCharacterData(float maxHp, float maxMp, float moveSpeed, float jumpForce, float reloadSpeed)
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
    public static GameData Instance;

    public List<SQLGunData> sqlGunDatas = new List<SQLGunData>();
    public List<SQLCharacterData> sqlCharacterDatas = new List<SQLCharacterData>();

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

        addSQLGunData();
        addSQLCharacterData();
    }
    private void Update()
    {
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

    private void addSQLGunData()
    {
        sqlGunDatas.Add(new SQLGunData(5f, 25f, 3f, 6f, 5f, false, 20, 0, 0, "Pistol"));
        sqlGunDatas.Add(new SQLGunData(24f, 45f, 4.5f, 20f, 7.5f, true, 60, 120, 2f, "M416"));
        sqlGunDatas.Add(new SQLGunData(27.5f, 40f, 4.2f, 20f, 7.5f, true, 60, 120, 2f, "MachineGun"));
        sqlGunDatas.Add(new SQLGunData(2f, 70f, 20f, 50f, 10f, true, 20, 40, 5f, "Rifle"));
        sqlGunDatas.Add(new SQLGunData(30f, 50f, 4f, 18f, 7.3f, true, 80, 160, 1.7f, "SMG"));
        sqlGunDatas.Add(new SQLGunData(1f, 100f, 50f, 100f, 20f, true, 10, 20, 10f, "Sniper_Rifle"));
        sqlGunDatas.Add(new SQLGunData(6f, 40f, 23.3f, 3f, 5f, true, 648, 0, 0f, "SpecialPistol"));
    }
    
    private void addSQLCharacterData()
    {
        sqlCharacterDatas.Add(new SQLCharacterData(1000f, 40f, 8f, 12.3f, 0.075f));
        sqlCharacterDatas.Add(new SQLCharacterData(900f, 50f, 8.3f, 12.8f, 0.2f));
        sqlCharacterDatas.Add(new SQLCharacterData(1200f, 31f, 7f, 12.3f, 0.025f));
        sqlCharacterDatas.Add(new SQLCharacterData(1050f, 39f, 7.8f, 12.5f, 0.1f));
    }
}
