using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BOSS_score : MonoBehaviour
{
    [Header("玩家")]
    public List<BasePlayerController> players = new List<BasePlayerController>();
    [Header("BOSS")]
    public BOSS boss;
    [Header("得分")]
    public TMP_Text score;
    [Header("限时")]
    public float maxTime = 300f;
    private float Timer;

    private void Start()
    {
        Timer = maxTime;
    }
    void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            GameData.Instance.TimeFinish = true;
            boss.TimeFinishAttack();
        }

        GameData.Instance.BOSS_scoer = (int)boss.maxHP - (int)boss.getHP();
        score.text = $"score: {(int)boss.maxHP - (int)boss.getHP()}";
        if (boss.getHP() <= 0)
        {
            //Game_api.Instance.RequestGameOver();
            GameData.Instance.BOSS_scoer += (int)(Timer * 30.0f);
            SceneManager.LoadScene("BOSS_Win");
        }
        if (GameData.Instance.TimeFinish) return;
        bool isLose = true;
        foreach(var player in players)
        {
            if (player.PlayerStatus.currentHp > 0) isLose = false;
        }
        if(isLose)
        {
            Game_api.Instance.RequestGameOver();
            SceneManager.LoadScene("BOSS_Lose");
        }
    }
}
