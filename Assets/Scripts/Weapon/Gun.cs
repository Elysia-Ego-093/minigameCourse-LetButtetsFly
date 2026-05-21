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
}
