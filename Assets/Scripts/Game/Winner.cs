using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Winner : MonoBehaviour
{
    [Header("胜利玩家")]
    protected PlayerData winner;
    [Header("胜利玩家头像")]
    public Image winnerImage;
    // Start is called before the first frame update
    void Start()
    {
        //getWinner();
        setWinner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    void getWinner() 
    {
        foreach(var player in GameData.Instance.players)
        {
            if(player.PlayerName== GameData.Instance.winnerName)
            {
                winner = player;
                break;
            }
        }
    }
    */
    void setWinner()
    {
        if (winnerImage == null) Debug.LogError("winnerImage 未赋值");
        if (GameData.Instance == null) Debug.LogError("GameData.Instance 为空");

        Sprite loadedSprite = Resources.Load<Sprite>("picture/Player/" + GameData.Instance.winnerName);
        if (loadedSprite == null) Debug.LogError("加载图片失败，检查路径和名称");

        winnerImage.sprite = loadedSprite;
    }
}
