using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOSS_UI : MonoBehaviour
{
    public BOSS boss;
    public Slider slider;

    void Update()
    {
        slider.maxValue = boss.maxHP;
        slider.value = boss.getHP();
    }
}
