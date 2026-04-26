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

    [Header("武器跟随")]
    public Transform weaponPivot;           // 角色的武器挂载点
    //public GameObject weaponPrefab;         // 手枪预制体
    private GameObject currentWeaponObject; // 当前生成的武器实例
    private Transform muzzleTransform;       // 武器的枪口位置

    [Header("射击反馈")]
    private bool isRecoiling = false;          // 是否正在播放后坐力动画       
    private Vector3 weaponOriginalLocalPos;          
    private Quaternion weaponOriginalLocalRot;       
        
    
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

        if (guns.Count > 0)
        {
            currentGun = guns[0];
            UpdateWeaponModel(currentGun);
        }
    }

    protected override void Update()
    {
        base.Update();
        HandleSwitchGun();
        HandleShoot();
        HandleReload();

        if (GetTestInput()) TestBullet();

        // 后坐力期间不更新角色翻转
        if (!isRecoiling && currentWeaponObject != null)
        {
            if (lastMoveDirection > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (lastMoveDirection < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }

    }
        

    protected override float GetHorizontalInput()
    {
        if (isRecoiling) return 0f;
        
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

    private void UpdateWeaponModel(GunData gun)
    {
        // 销毁旧模型
        if (currentWeaponObject != null)
            Destroy(currentWeaponObject);
        
        if (gun.weaponModelPrefab != null && weaponPivot != null)
        {
            currentWeaponObject = Instantiate(gun.weaponModelPrefab, weaponPivot);
            // 应用该武器独有的局部偏移和旋转
            currentWeaponObject.transform.localPosition = gun.weaponLocalOffset;
            currentWeaponObject.transform.localRotation = Quaternion.Euler(0, 0, gun.weaponLocalRotationZ);
            
            // 记录原始局部坐标（用于后坐力）
            weaponOriginalLocalPos = currentWeaponObject.transform.localPosition;
            weaponOriginalLocalRot = currentWeaponObject.transform.localRotation;
            
            // 查找枪口
            muzzleTransform = currentWeaponObject.transform.Find("Muzzle");
            if (muzzleTransform == null)
                Debug.LogWarning($"武器 {gun.gunName} 缺少 Muzzle 子物体");
        }
        else
        {
            Debug.LogError($"武器 {gun.gunName} 缺少 weaponModelPrefab 或 weaponPivot 未设置");
            currentWeaponObject = null;
            muzzleTransform = null;
        }
    }
     private void HandleSwitchGun()
    {
        if (GetSwitchGunInput() && guns.Count > 0)
        {
            currentGunIndex = (currentGunIndex + 1) % guns.Count;
            currentGun = guns[currentGunIndex];
            Debug.Log("切换至:" + currentGun.gunName);
            
            UpdateWeaponModel(currentGun);
            
            // 处理弹药逻辑（原有）
            if (currentGun.nowAmmo == -1)
                currentGun.nowAmmo = currentGun.maxAmmo;
            Debug.Log($"子弹: {currentGun.nowAmmo}/{currentGun.maxAmmo}");
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
            //Vector2 spawnPos = (Vector2)transform.position + lastMoveDirection * new Vector2(1f + 0.15f * shootController, 0f);
            Vector2 spawnPos;
            if (muzzleTransform != null)
                spawnPos = muzzleTransform.position;      // 使用枪口位置
            else
                spawnPos = (Vector2)transform.position + lastMoveDirection * new Vector2(1f, 0f); // 备用方案
            GameObject newBullet = Instantiate(currentGun.bulletPrefab, spawnPos, Quaternion.identity);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetStatus(currentGun.bulletSpeed, lastMoveDirection, currentGun.bulletATK, currentGun.force_x, currentGun.force_y);
            }
            if (!isRecoiling) StartCoroutine(WeaponRecoilCoroutine());
        }
        
    }
    private IEnumerator WeaponRecoilCoroutine()
    {
        if (currentWeaponObject == null || currentGun == null)
        {
            isRecoiling = false;
            yield break;
        }

        isRecoiling = true;

        // 获取当前武器的后坐力参数
        float recoilDist = currentGun.recoilDistance;
        float recoilRot = currentGun.recoilRotation;
        float peakDur = currentGun.recoilPeakDuration;
        float returnDur = currentGun.recoilReturnDuration;
        float holdDur = currentGun.recoilHoldDuration;

        Vector3 startLocalPos = currentWeaponObject.transform.localPosition;
        Quaternion startLocalRot = currentWeaponObject.transform.localRotation;
        Vector3 targetLocalPos = startLocalPos + new Vector3(recoilDist, 0, 0);
        Quaternion targetLocalRot = startLocalRot * Quaternion.Euler(0, 0, recoilRot);

        // 快速达到峰值
        float elapsed = 0f;
        while (elapsed < peakDur)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / peakDur;
            currentWeaponObject.transform.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, t);
            currentWeaponObject.transform.localRotation = Quaternion.Lerp(startLocalRot, targetLocalRot, t);
            yield return null;
        }
        currentWeaponObject.transform.localPosition = targetLocalPos;
        currentWeaponObject.transform.localRotation = targetLocalRot;

        // 峰值停留
        if (holdDur > 0)
            yield return new WaitForSeconds(holdDur);

        // 恢复
        elapsed = 0f;
        while (elapsed < returnDur)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / returnDur;
            currentWeaponObject.transform.localPosition = Vector3.Lerp(targetLocalPos, startLocalPos, t);
            currentWeaponObject.transform.localRotation = Quaternion.Lerp(targetLocalRot, startLocalRot, t);
            yield return null;
        }

        // 最终归位
        currentWeaponObject.transform.localPosition = startLocalPos;
        currentWeaponObject.transform.localRotation = startLocalRot;

        isRecoiling = false;
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
