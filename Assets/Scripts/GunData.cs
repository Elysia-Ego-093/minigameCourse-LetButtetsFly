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
    public float bulletATK = 50f;

    [Header("子弹击退力度")]
    public float force_x = 10f;
    public float force_y = 5f;

    [Header("当前子弹数量")]
    public int nowAmmo = -1; // -1表示没有初始化
    public bool needAmmo = true; // 是否需要子弹
    public int maxAmmo = 100; // 最大子弹数量
}
