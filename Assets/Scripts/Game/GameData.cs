using System.Collections.Generic;
using UnityEngine;

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
    public PlayerData(PlayerData player){
        PlayerName = player.PlayerName;
        maxHp = player.maxHp;
        moveSpeed = player.moveSpeed;
        jumpForce = player.jumpForce;
        maxMp = player.maxMp;
        reloadSpeed = player.reloadSpeed;
        gun = player.gun;
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

    [Header("选中的玩家数据")]
    public int PlayerCount;
    public List<PlayerData> players = new List<PlayerData>();

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        for (int i = 0; i < PlayerCount; i++) players.Add(null);
    }
    public void ClearPlayerData()
    {
        foreach(var player in players)
        {
            player.ClearData();
        }
    }
}
