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
        GameData.Instance.BGMVolume = BGMSlider.value;
        BGMValue.text = $"{(int)(GameData.Instance.BGMVolume * 100f)}";
        GameData.Instance.SoundVolume = SoundSlider.value;
        SoundValue.text = $"{(int)(GameData.Instance.SoundVolume * 100f)}";
    }

    public void setStatus()
    {
        BGMSlider.maxValue = 1f;
        BGMSlider.value = GameData.Instance.BGMVolume;
        BGMValue.text = $"{(int)(GameData.Instance.BGMVolume * 100f)}";
        SoundSlider.maxValue = 1f;
        SoundSlider.value = GameData.Instance.SoundVolume;
        SoundValue.text = $"{(int)(GameData.Instance.SoundVolume * 100f)}";
    }
}
