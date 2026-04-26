using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Gun Data")]
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
    public float bulletATK = 200f;

    [Header("子弹击退力度")]
    public float force_x = 10f;
    public float force_y = 5f;

    [Header("当前子弹数量")]
    public int nowAmmo = -1; // -1表示没有初始化
    public bool needAmmo = true; // 是否需要子弹
    public int maxAmmo = 100; // 最大子弹数量
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
