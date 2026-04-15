using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : BasePlayerController
{
    [Header("玩家1按键设置")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.K;
    public KeyCode shootKey = KeyCode.J;
    public KeyCode switchGunKey = KeyCode.R;
    public KeyCode testKey = KeyCode.U;
    public KeyCode downKey = KeyCode.S;

    public KeyCode reloadKey = KeyCode.Q;

    [Header("玩家1专用枪械")]
    public List<GunData> guns = new List<GunData>();
    private int currentGunIndex = 0;
    private GunData currentGun;
    private float lastShootTime = 0f;
    private int shootController = 0;
    
    // ===== 弹药系统 =====
    protected override void Start()
    {
        base.Start();
        if (guns.Count > 0)
        {
            currentGunIndex = 0;
            currentGun = guns[currentGunIndex];
            // 初始化子弹数量为满弹匣
            currentGun.nowAmmo = currentGun.maxAmmo;
            Debug.Log($"初始化武器: {currentGun.gunName}, 子弹: {currentGun.nowAmmo}/{currentGun.maxAmmo}");
            for(int i = 0; i < guns.Count; i++)
            {
                guns[i].nowAmmo = guns[i].maxAmmo;
                Debug.Log($"初始化武器: {guns[i].gunName}, 子弹: {guns[i].nowAmmo}/{guns[i].maxAmmo}");
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        HandleSwitchGun();
        HandleShoot();
        HandleReload();

        if (GetTestInput()) TestBullet();
    }

    protected override float GetHorizontalInput()
    {
        float input = 0;
        if (Input.GetKey(leftKey)) input = -1;
        if (Input.GetKey(rightKey)) input = 1;
        return input;
    }
    protected override bool GetJumpInput() => Input.GetKeyDown(jumpKey);
    protected override bool GetJumpHoldInput() => Input.GetKey(jumpKey);
    protected override bool GetDownInput() => Input.GetKey(downKey);
    protected override bool GetSprintInput() => Input.GetKey(sprintKey);
    protected override bool GetShootInput() => Input.GetKey(shootKey);
    protected override bool GetSwitchGunInput() => Input.GetKeyDown(switchGunKey);
    protected override bool GetTestInput() => Input.GetKeyDown(testKey);
    protected override bool GetReloadInput() => Input.GetKeyDown(reloadKey);
    protected override bool GetDownHoldInput() => Input.GetKeyDown(downKey);
    protected override bool GetDownFinishInput() => Input.GetKeyUp(downKey);

    private void HandleSwitchGun()
    {
        if (GetSwitchGunInput() && guns.Count > 0)
        {
            currentGunIndex = (currentGunIndex + 1) % guns.Count;
            currentGun = guns[currentGunIndex];
            Debug.Log("玩家1切换至：" + currentGun.gunName);
            if(currentGun.nowAmmo == -1)
            {
                currentGun.nowAmmo = currentGun.maxAmmo;
            }
            Debug.Log($"切换至：{currentGun.gunName}, 子弹: {currentGun.nowAmmo}/{currentGun.maxAmmo}");
        }
    }

    private void HandleShoot()
    {
        if (currentGun == null) return;

        if (currentGun.needAmmo && currentGun.nowAmmo <= 0)
        {
            // 子弹耗尽，无法射击
            return;
        }
        
        float fireInterval = 1f / currentGun.fireRate;
        if (Time.time - lastShootTime < fireInterval) return;
        
        if (GetShootInput())
        {
            lastShootTime = Time.time;
            shootController = (shootController + 1) % 2;
            if (currentGun.needAmmo)
            {
                currentGun.nowAmmo--; // 每次射击 -1 子弹
            }
            Vector2 spawnPos = (Vector2)transform.position + lastMoveDirection * new Vector2(1f + 0.15f * shootController, 0f);
            GameObject newBullet = Instantiate(currentGun.bulletPrefab, spawnPos, Quaternion.identity);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetStatus(currentGun.bulletSpeed, lastMoveDirection, currentGun.bulletATK, currentGun.force_x, currentGun.force_y);
            }
        }
    }

    private void TestBullet()
    {
        Vector2 spawnPos = (Vector2)transform.position + lastMoveDirection * new Vector2(10f, 0f);
        GameObject newBullet = Instantiate(currentGun.bulletPrefab, spawnPos, Quaternion.identity);
        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetStatus(currentGun.bulletSpeed, -lastMoveDirection, currentGun.bulletATK, currentGun.force_x, currentGun.force_y);
        }
    }
    private void HandleReload()
    {
        if (currentGun == null || !currentGun.needAmmo) return;

        if (GetReloadInput())
        {
            currentGun.nowAmmo = currentGun.maxAmmo;
            Debug.Log($"重新装填: {currentGun.gunName}, 子弹: {currentGun.nowAmmo}/{currentGun.maxAmmo}");
        }
    }

    // ===== 供UI调用的公共方法 =====
    public int GetCurrentAmmo()
    {
        if (currentGun == null) return 0;
        return currentGun.nowAmmo;
    }
    
    public int GetMaxAmmo()
    {
        if (currentGun == null) return 0;
        return currentGun.maxAmmo;
    }
    
    public string GetCurrentGunName()
    {
        if (currentGun == null) return "无枪械";
        return currentGun.gunName;
    }
    
    public bool IsNeedAmmo()
    {
        if (currentGun == null) return false;
        return currentGun.needAmmo;
    }
}
