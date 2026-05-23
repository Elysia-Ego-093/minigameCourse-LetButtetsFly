using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("µÃ·Ö")]
    public TMP_Text score;

    [Header("Íæ¼Ò")]
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
    }
}
