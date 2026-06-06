using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
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
public class GameData : MonoBehaviour
{
    public static GameData Instance;

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
    }
    private void Update()
    {
        string currentSecneName = SceneManager.GetActiveScene().name;
        if (!(currentSecneName == "MainTheme" || currentSecneName == "SelectPlayer" || currentSecneName == "SelectMap"))
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

    
}
