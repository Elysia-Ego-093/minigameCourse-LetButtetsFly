using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("玩家总数")]
    public int PlayerCount;

    [Header("胜利分数")]
    public int MaxScore;

    [Header("玩家")]
    public List<PlayerController> players = new List<PlayerController>();

    [Header("分数")]
    public List<int> scores= new List<int>();
    void Start()
    {
        IntInitialization();
        Update();
    }

    private void IntInitialization()
    {
        //PlayerCount = playCount;
        //MaxScore = maxScore;
        for (int i = 0; i < PlayerCount; i++)
        {
            scores.Add(0);
        }
    }

    void Update()
    {
       
    }
}
