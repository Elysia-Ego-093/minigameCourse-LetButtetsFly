using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOver_UI : MonoBehaviour
{
    public TMP_Text score;
    public Image isNewImage;

    void Update()
    {
        if(score != null)
        {
            score.text = $"score:{GameData.Instance.BOSS_scoer}";
        }
        if (isNewImage != null)
        {
            isNewImage.gameObject.SetActive(GameData.Instance.isNew);
        }
    }
}
