using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [Header("得分")]
    public TMP_Text score;

    [Header("玩家")]
    public Player_UI_Manager player1;
    public Player_UI_Manager player2;
    private PlayerController P1;
    private PlayerController P2;

    private void Start()
    {
        P1 = player1.player;
        P2 = player2.player;
    }
    // Update is called once per frame
    void Update()
    {
        score.text = $"{P2.GetDeathCount()}:{P1.GetDeathCount()}";
        IsHereAWinner();
    }
    //判断是否出现赢家
    void IsHereAWinner()
    {
        if (P2.GetDeathCount() >= GameData.Instance.winnerScore)
        {
            GameData.Instance.winnerName = GameData.Instance.players[P1.id].PlayerName;
            SceneManager.LoadScene("GameWinner");

        }
        if(P1.GetDeathCount() >= GameData.Instance.winnerScore)
        {
            GameData.Instance.winnerName = GameData.Instance.players[P2.id].PlayerName;
            SceneManager.LoadScene("GameWinner");
        }
    }
}
