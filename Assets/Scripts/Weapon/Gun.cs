using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("数据配置")]
    public GunData data;

    [Header("当前子弹数量")]
    public int nowAmmo = -1; // -1表示没有初始化
    public int AmmoNum; //当前子弹剩余数量

    private float UpdateDataTimer = 1f;

    private void Start()
    {
        if(GameData.Instance.GunDatas_from_web.Count > data.gun_id)
        {
            UpdateData(GameData.Instance.GunDatas_from_web[data.gun_id]);
        }
        UpdateDataTimer = 1f;
        nowAmmo = data.maxAmmo;
        AmmoNum = data.BasicAmmoNum;
    }

    private void Update()
    {
        if (UpdateDataTimer >= 0f)
        {
            UpdateDataTimer-= Time.deltaTime;
        }
        else
        {
            UpdateDataTimer = 1f;
            if (GameData.Instance.GunDatas_from_web.Count > data.gun_id)
            {
                UpdateData(GameData.Instance.GunDatas_from_web[data.gun_id]);
            }
        }
        
    }
    public void reloadAmmo()
    {
        int temp = nowAmmo;
        nowAmmo = Mathf.Min(data.maxAmmo, AmmoNum + nowAmmo);
        AmmoNum = Mathf.Max(AmmoNum - data.maxAmmo + temp, 0);
    }

    public void UpdateData(GunData_from_web gd)
    {
        data.AmmoTime = gd.AmmoTime;
        data.BasicAmmoNum = gd.BasicAmmoNum;
        data.maxAmmo = gd.maxAmmo;
        data.needAmmo = gd.needAmmo;
        data.fireRate = gd.fireRate;
        data.bulletATK = gd.bulletATK;
        data.bulletSpeed = gd.bulletSpeed;
        data.force_x = gd.force_x;
        data.force_y = gd.force_y;
        data.gunName = gd.gunName;
    }
}
