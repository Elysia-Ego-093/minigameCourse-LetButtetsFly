using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelector : MonoBehaviour
{
    [Header("角色列表")]
    public List<PlayerData> players = new List<PlayerData>();

    [Header("UI显示列表")]
    public List<PlayerData_UI> playerUIs = new List<PlayerData_UI>();

    private void Start()
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].character_id < GameData.Instance.sqlCharacterDatas.Count 
                && GameData.Instance.sqlCharacterDatas[players[i].character_id] != null)
            {
                UpdateCharacterData(players[i], GameData.Instance.sqlCharacterDatas[players[i].character_id]);
            }
        }
    }

    private void Update()
    {
        for(int i = 0; i < GameData.Instance.players.Count; i++)
        {
            if (GameData.Instance.players[i].maxHp == 0) playerUIs[i].gameObject.SetActive(false);
            else playerUIs[i].gameObject.SetActive(true);
        }
    }

    public void SelectPlayer(int index)
    {
        if (index >= players.Count || players[index] == null) return;
        for(int i = 0; i < GameData.Instance.players.Count; i++)
        {
            if (GameData.Instance.players[i].maxHp == 0)
            {
                playerUIs[i].GetPlayer(index);
                GameData.Instance.players[i] = new PlayerData(players[index]);
                return;
            }
        }
    }
    public void SelectPlayer_release()
    {
        for (int i = GameData.Instance.PlayerCount - 1; i >= 0; i--)
        {
            if (GameData.Instance.players[i].maxHp != 0)
            {
                GameData.Instance.players[i].ClearData();
                playerUIs[i].ClearData();
                return;
            }
        }
    }
    public void SelectPlayer_continue()
    {
        foreach (var player in GameData.Instance.players)
        {
            if (player.maxHp == 0)
            {
                return;
            }
        }
        SceneManager.LoadScene("SelectMap");
    }

    private void UpdateCharacterData(PlayerData pd, SQLCharacterData cd)
    {
        pd.maxHp = cd.maxHp;
        pd.maxMp = cd.maxMp;
        pd.jumpForce = cd.jumpForce;
        pd.moveSpeed = cd.moveSpeed;
        pd.reloadSpeed = cd.reloadSpeed;
    }

}
