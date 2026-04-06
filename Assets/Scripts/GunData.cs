using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Gun Data")]
public class GunData : ScriptableObject
{
    public string gunName = "Pistol";
    public float fireRate = 5f;          // 每秒发射次数
    public GameObject bulletPrefab;       // 子弹预制体
    public float bulletSpeed = 10f;       // 子弹速度
}
