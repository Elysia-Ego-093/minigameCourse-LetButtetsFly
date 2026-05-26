using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWinners : MonoBehaviour
{
    [Header("出生点列表")]
    protected List<Transform> playerStartPoints = new List<Transform>();
    [Header("玩家预制体")]
    protected GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        createPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //生成所有玩家
    protected void createPlayers()
    {
        foreach (var player in GameData.Instance.players)
        {
            int i = 1;
            if (player.PlayerName == GameData.Instance.winnerName)
            {
                createPlayer(player, playerStartPoints[0]);
            }
            else
            {
                createPlayer(player, playerStartPoints[i]);
                i++;
            }
        }
    }
    //生成玩家
    protected void createPlayer(PlayerData player,Transform spawnPoint)
    {
        GameObject newPlayer= Instantiate(playerPrefab,spawnPoint);
    }
}
