using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [Header("BGM")]
    public Slider BGMSlider;
    public TMP_Text BGMValue;

    [Header("“Ù–ß")]
    public Slider SoundSlider;
    public TMP_Text SoundValue;

    void Update()
    {
        //if (!flag) return;
        GameData.Instance.BGMVolume = BGMSlider.value;
        BGMValue.text = $"{GameData.Instance.BGMVolume}";
        GameData.Instance.SoundVolume = SoundSlider.value;
        SoundValue.text = $"{GameData.Instance.SoundVolume}";
    }

    public void setStatus()
    {
        BGMSlider.maxValue = 1f;
        BGMSlider.value = GameData.Instance.BGMVolume;
        BGMValue.text = $"{GameData.Instance.BGMVolume}";
        SoundSlider.maxValue = 1f;
        SoundSlider.value = GameData.Instance.SoundVolume;
        SoundValue.text = $"{GameData.Instance.SoundVolume}";
    }
}
