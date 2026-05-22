using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    [Header("╫ги╚ап╠М")]
    public List<PlayerData> players = new List<PlayerData>();

    public void SelectPlayer(int index)
    {
        if (index >= players.Count || players[index] == null) return;
        for(int i = 0; i < GameData.Instance.players.Count; i++)
        {
            if (GameData.Instance.players[i].maxHp == 0)
            {
                GameData.Instance.players[i] = players[index];
                return;
            }
        }
    }
}
