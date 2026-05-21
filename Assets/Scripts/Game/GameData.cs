using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float maxHp;
    public float moveSpeed;
    public GameObject gun;
    public PlayerData(float maxHp, float moveSpeed,GameObject gun){
        this.maxHp = maxHp;
        this.moveSpeed = moveSpeed;
        this.gun = gun;
    }
}
public class GameData : MonoBehaviour
{
    public static GameData Instance;

    [Header("选中的玩家数据")]
    public int PlayerCount;
    public List<PlayerData> players = new List<PlayerData>();

    private void Awake()
    {
        for (int i = 0; i < PlayerCount; i++) players.Add(null);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
