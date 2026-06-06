using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [Header("…Ë÷√ΩÁ√Ê")]
    public SettingMenu Menu;
    
    void Start()
    {
        Menu.gameObject.SetActive(false);
    }

    public void OpenSettingMenu()
    {
        Menu.setStatus();
        Menu.gameObject.SetActive(true);
    }

    public void CloseSettingMenu()
    {
        Menu.gameObject.SetActive(false);
    }
}
