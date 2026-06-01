using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunData : ScriptableObject
{
    [Header("子弹预制体")]
    public GameObject bulletPrefab;

    [Header("枪械类型")]
    public string gunName = "Pistol";

    [Header("射速")]
    public float fireRate = 5f;

    [Header("子弹速度")]
    public float bulletSpeed = 10f;

    [Header("子弹伤害")]
    public float bulletATK = 50f;

    [Header("子弹击退力度")]
    public float force_x = 10f;
    public float force_y = 5f;

    [Header("弹量设置")]
    public bool needAmmo = true; // 是否需要子弹
    public int maxAmmo = 100; // 弹夹最大装弹数量
    public int BasicAmmoNum;
    public int AmmoTime; //换弹时间
    [Header("射击音效")]
    public AudioClip shootSound;

    [Header("枪械模型预制体")]
    public GameObject weaponModelPrefab;
    public Vector2 weaponLocalOffset = Vector2.zero;   // 相对于 weaponPivot 的局部位置偏移
    public float weaponLocalRotationZ = 0f;            // 相对于 weaponPivot 的局部旋转（仅Z轴）

    [Header("后坐力设置")]
    public float recoilDistance = -0.1f;      // 后退距离
    public float recoilRotation = 5f;          // 旋转角度
    public float recoilPeakDuration = 0.04f;   // 达到峰值的时间
    public float recoilReturnDuration = 0.1f;  // 恢复回原位的时间
    public float recoilHoldDuration = 0.02f;   // 峰值停留时间
}
