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

    private void Start()
    {
        if(GameData.Instance.sqlGunDatas.Count > data.gun_id)
        {
            UpdateData(GameData.Instance.sqlGunDatas[data.gun_id]);
        }
        nowAmmo = data.maxAmmo;
        AmmoNum = data.BasicAmmoNum;
    }
    public void reloadAmmo()
    {
        int temp = nowAmmo;
        nowAmmo = Mathf.Min(data.maxAmmo, AmmoNum + nowAmmo);
        AmmoNum = Mathf.Max(AmmoNum - data.maxAmmo + temp, 0);
        Debug.Log(AmmoNum);
    }

    public void UpdateData(SQLGunData gd)
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
