using R3;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BasePlayerController : MonoBehaviour
{
    [Header("玩家ID")]
    public int id;
    

    protected Rigidbody2D rb;
    protected PlayerStatus playerStatus;

    [Header("碰撞监测点")]
    public List<GameObject> ColliderPoints = new List<GameObject>();
    protected BoxCollider2D PlayerCollider;

    [Header("地面监测点")]
    public List<GameObject> GroundCheckers = new List<GameObject>();
    protected bool isGrounded = false;

    [Header("体型设置")]
    public float basicWidth = -0.5f;
    public float basicHeight = 0.5f;
    protected float currentWidth;
    protected float currentHeight;
    public float crouchHeightPercent = 1f;
    // 公开的读取属性
    public PlayerStatus PlayerStatus => playerStatus;

    [Header("移动设置")]
    public float basicMoveSpeed = 8f;
    protected float MoveSpeed;
    protected float moveInput;
    protected bool useInertia = false;
    protected float lastMoveDirection = 1f;

    [Header("跳跃设置")]
    public float basicJumpForce = 12f;
    protected float jumpForce;
    public int maxJumpCount = 2;
    protected int jumpCountRemain;
    protected bool isJumping;

    [Header("加速设置")]
    public float sprintSpeedMultiplier = 2f;
    public float staminaCostPerSecond = 50f;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRecoverDelay = 2f;
    public float staminaRecoverSpeed = 20f;
    protected float recoverWaitTimer;
    protected bool isSprinting;

    [Header("受击设置")]
    protected bool isKnockback = false;
    public float knockbackDuration = 0.2f;
    protected float knockbackTimer = 0f;

    [Header("枪械设置")]
    public int maxGunCount;
    public List<Gun> guns = new List<Gun>();
    public int currentGunIndex = 0;
    protected Gun currentGun;
    protected float lastShootTime = 0f;
    protected int shootController = 0;
    protected bool trySwitchGun = false;//切换枪械预输入
    protected float AmmoTimer = 0f;//换弹计时器
    protected bool isReloadAmmo;
    public GameObject dropGunPrefab;
    protected float reloadSpeed;
    private readonly string gunImageFloderPath = "picture/Weapon";

    [Header("武器跟随")]
    public Transform weaponPivot;           // 角色的武器挂载点
    protected GameObject currentWeaponObject; // 当前生成的武器实例
    protected Transform muzzleTransform;       // 武器的枪口位置

    [Header("射击反馈")]
    protected bool isRecoiling = false;          // 是否正在播放后坐力动画       
    protected Vector3 weaponOriginalLocalPos;
    protected Quaternion weaponOriginalLocalRot;

    [Header("手榴弹设置")]
    public GameObject grenadePrefab;
    public float throwForce_x;
    public float throwForce_y;
    public float throwTime;
    private float currentThrowTimer = 0;
    public int GrenadeCount;
    public GameObject grenadePoint;

    [Header("虚空设置")]
    public float VoidHeight = -5f;
    public float VoidDamage = 200f;
    public float respawnTime = 5f;
    protected bool isInVoid = false;
    protected float respawnTimer = 0f;

    [Header("重生范围")]
    public Vector2 leftLowerCorner;
    public Vector2 RightUpperCorner;

    [Header("暂停状态")]
    public bool isPause = false;

    [Header("动画管理")]
    protected Animator animator;

    [Header("材质管理")]
    protected Renderer[] rendererArray;

    protected Dictionary<string, float> buffs = new Dictionary<string, float>();
    protected int DeathCount = 0;

    // 抽象方法 - 子类必须实现自己的输入逻辑
    protected abstract float GetHorizontalInput();
    protected abstract bool GetLeftInput();
    protected abstract bool GetRightInput();
    protected abstract bool GetJumpInput();
    protected abstract bool GetSprintInput();
    protected abstract bool GetShootInput();
    protected abstract bool GetSwitchGunInput();
    protected abstract bool GetJumpHoldInput();    
    protected abstract bool GetDownInput();        
    protected abstract bool GetReloadInput();   
    protected abstract bool GetDownFinishInput();
    protected abstract bool GetThrowGrenadeInput();
    protected abstract bool GetDropGunInput();

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<BoxCollider2D>();
        playerStatus = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
        if (playerStatus == null)
        {
            Debug.LogError("玩家物体缺少PlayerStatus组件");
        }
        if (GameData.Instance.players[id] == null)
        {
            return;
        }
        PlayerData p0 = GameData.Instance.players[id];
        playerStatus.maxHp = p0.maxHp;
        maxStamina = p0.maxMp;
        basicMoveSpeed = p0.moveSpeed;
        basicJumpForce = p0.jumpForce;
        reloadSpeed = p0.reloadSpeed;
        addGun(p0.gun);
        changeMaterialTexture(p0.textureHead, p0.textureBody, p0.textureHand);
        Initial();
        playerStatus.currentHp = playerStatus.maxHp;
        if (guns.Count > 0)
        {
            currentGunIndex = 0;
            currentGun = guns[currentGunIndex];
            // 初始化子弹数量为满弹匣
            currentGun.nowAmmo = currentGun.data.maxAmmo;
            Debug.Log($"初始化武器: {currentGun.data.gunName}, 子弹: {currentGun.nowAmmo}/{currentGun.data.maxAmmo}");
            for (int i = 0; i < guns.Count; i++)
            {
                guns[i].nowAmmo = guns[i].data.maxAmmo;
                Debug.Log($"初始化武器: {guns[i].data.gunName}, 子弹: {guns[i].nowAmmo}/{guns[i].data.maxAmmo}");
            }
        }

        if (guns.Count > 0)
        {
            currentGun = guns[0];
            UpdateWeaponModel(currentGun);
        }

    }

    protected virtual void Update()
    {
        if (isPause) return;
        if (isInVoid)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                isInVoid = false;
                transform.position = new Vector2(
                    Random.Range(leftLowerCorner.x,RightUpperCorner.x),
                    Random.Range(leftLowerCorner.y,RightUpperCorner.y));
                rb.gravityScale = 1.0f;
                if (PlayerStatus.currentHp <= 0)
                {
                    while (currentGunIndex != 0) SwitchGun();
                    while (guns.Count > 1) guns.RemoveAt(1);
                    addGun(GameData.Instance.players[id].gun);
                    playerStatus.currentHp = playerStatus.maxHp;
                }
            }
            return;
        }
        CheckVoid();
        UpdateCollider();
        isGrounded = CheckGround();
        HandleSprint();
        HandleMove();
        HandleDown();
        HandleJump();
        UpdateStamina();
        UpdateKnockback();
        UpdateBuff();
        HandleDropGun();
        HandleSwitchGun();
        HandleShoot();
        HandleGrenade();
        HandleReload();

        UpdateHorizontal();
    }

    private void Initial()
    {
        playerStatus.currentShield = 0;
        buffs["big"] = 0;
        buffs["small"] = 0;
        buffs["fast"] = 0;
        buffs["slow"] = 0;
        buffs["jump"] = 0;
        MoveSpeed = basicMoveSpeed;
        jumpForce = basicJumpForce;
        jumpCountRemain = maxJumpCount;
        currentStamina = maxStamina;
        recoverWaitTimer = staminaRecoverDelay;
        AmmoTimer = 0f;
        isReloadAmmo = false;
        animator.SetBool("isRun", false);
        animator.SetBool("isDown", false);
        animator.SetBool("isOver", false);
    }

    protected virtual void UpdateCollider()
    {
        if (ColliderPoints.Count == 0) return;
        float x1 = ColliderPoints[0].transform.position.x, y1 = ColliderPoints[0].transform.position.y, x2 = x1, y2 = y1;
        foreach(var point in ColliderPoints)
        {
            x1 = Mathf.Max(x1, point.transform.position.x);
            y1 = Mathf.Max(y1, point.transform.position.y);
            x2 = Mathf.Min(x2, point.transform.position.x);
            y2 = Mathf.Min(y2, point.transform.position.y);
        }
        PlayerCollider.size = new Vector2((x1 - x2) / Mathf.Abs(transform.localScale.x), (y1 - y2) / Mathf.Abs(transform.localScale.y));
        PlayerCollider.offset = new Vector2(((x1 + x2) / 2f - transform.position.x) / transform.localScale.x,
                                            ((y1 + y2) / 2f - transform.position.y) / transform.localScale.y);
    }
    
    protected virtual void UpdateHorizontal()
    {
        if (GetHorizontalInput() > 0)
        {
            lastMoveDirection = 1f;
            transform.localScale = new Vector3(currentWidth, currentHeight, -0.5f);
            animator.SetBool("isRun", true);
        }

        if (GetHorizontalInput() < 0)
        {
            lastMoveDirection = -1f;
            transform.localScale = new Vector3(-1 * currentWidth, currentHeight, -0.5f);
            animator.SetBool("isRun", true);
        }
        if (GetHorizontalInput() == 0)
        {
            transform.localScale = new Vector3(currentWidth * lastMoveDirection, currentHeight, -0.5f);
            animator.SetBool("isRun", false);
        }
        weaponPivot.transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }


    protected virtual void UpdateWeaponModel(Gun gun)
    {
        // 销毁旧模型
        if (currentWeaponObject != null)
            Destroy(currentWeaponObject);

        if (gun.data.weaponModelPrefab != null && weaponPivot != null)
        {
            currentWeaponObject = Instantiate(gun.data.weaponModelPrefab, weaponPivot);
            // 应用该武器独有的局部偏移和旋转
            currentWeaponObject.transform.localPosition = gun.data.weaponLocalOffset;
            currentWeaponObject.transform.localRotation = Quaternion.Euler(0, 0, gun.data.weaponLocalRotationZ);

            // 记录原始局部坐标（用于后坐力）
            weaponOriginalLocalPos = currentWeaponObject.transform.localPosition;
            weaponOriginalLocalRot = currentWeaponObject.transform.localRotation;

            // 查找枪口
            muzzleTransform = currentWeaponObject.transform.Find("Muzzle");
            if (muzzleTransform == null)
                Debug.LogWarning($"武器 {gun.data.gunName} 缺少 Muzzle 子物体");
        }
        else
        {
            Debug.LogError($"武器 {gun.data.gunName} 缺少 weaponModelPrefab 或 weaponPivot 未设置");
            currentWeaponObject = null;
            muzzleTransform = null;
        }
    }

    // 移动相关
    protected virtual void HandleMove()
    {
        if (isKnockback) return;
        moveInput = GetHorizontalInput();
        if (moveInput == 0 && rb.velocity.y != 0 && useInertia) return;
        float finalSpeed = isSprinting ? MoveSpeed * sprintSpeedMultiplier : MoveSpeed;
        rb.velocity = new Vector2(moveInput * finalSpeed, rb.velocity.y);
        useInertia = false;
        
        if (moveInput != 0)
        {
            lastMoveDirection = Mathf.Sign(moveInput);
        }
    }

    // 冲刺相关
    protected virtual void HandleSprint()
    {
        bool wantSprint = GetSprintInput() && Mathf.Abs(GetHorizontalInput()) > 0.1f && currentStamina > 0;
        if (wantSprint)
        {
            isSprinting = true;
            recoverWaitTimer = 0;
        }
        else
        {
            isSprinting = false;
        }
    }

    //下蹲相关
    protected virtual void HandleDown()
    {
        if (isGrounded && GetDownInput())
        {
            Debug.Log("玩家蹲下");
            animator.SetBool("isDown", true);
        }
        if (GetDownFinishInput() || !isGrounded) 
        {
            Debug.Log("玩家站起");
            animator.SetBool("isDown", false);
        }
    }

    // 跳跃相关
    protected virtual void HandleJump()
    {
        if (GetDownInput() && GetJumpHoldInput())
        {
            StartCoroutine(DisablePlatformCollision());
            return;
        }

        if (rb.velocity.y < 0 && isGrounded)
        {
            jumpCountRemain = maxJumpCount;
        }

        if (GetJumpInput() && jumpCountRemain > 0 && knockbackTimer <= 0f)  
        {
            isKnockback = false;
            rb.velocity = new Vector2(moveInput == 0 ? 0 : rb.velocity.x, jumpForce);
            jumpCountRemain--;
            isJumping = true;
            useInertia = false;
        }
        
        if (!GetJumpHoldInput()) isJumping = false;
        
        if (!isJumping) rb.velocity += new Vector2(0, -9.81f * Time.deltaTime);
        rb.velocity += new Vector2(0, -9.81f * Time.deltaTime);
    }

    public bool CheckGround()
    {
        foreach(var checker in GroundCheckers)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(checker.transform.position, 0.05f);
            foreach (var col in cols)
            {
                Collider2D item = col.GetComponent<Collider2D>();
                if (item != null && item != PlayerCollider)
                {
                    if (knockbackTimer <= 0f) isKnockback = false;
                    return true;
                }
            }
        }
        return false;
    }

    public Vector2 GetIndicatorPosition() { return new Vector2(transform.position.x, transform.position.y + (PlayerCollider.offset.y + PlayerCollider.size.y / 2f) * transform.localScale.y); }
    // 耐力更新
    protected virtual void UpdateStamina()
    {
        if (isSprinting)
        {
            currentStamina -= staminaCostPerSecond * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);
        }
        else
        {
            recoverWaitTimer += Time.deltaTime;
            if (recoverWaitTimer >= staminaRecoverDelay)
            {
                currentStamina += staminaRecoverSpeed * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
            }
        }
    }

    //Buff相关
    protected virtual void UpdateBuff()
    {
        if (buffs["big"] > 0 || buffs["small"] > 0)
        {
            if(buffs["big"] > 0) buffs["big"] -= Time.deltaTime;
            else buffs["small"] -= Time.deltaTime;
        }
        else
        {
            currentWidth = basicWidth;
            currentHeight = basicHeight;
            transform.localScale = new Vector3(basicWidth * lastMoveDirection, basicHeight, 1f);
        }

        if (buffs["fast"] > 0 || buffs["slow"] > 0)
        {
            if (buffs["fast"] > 0) buffs["fast"] -= Time.deltaTime;
            else buffs["slow"] -= Time.deltaTime;
        }
        else
        {
            MoveSpeed = basicMoveSpeed;
        }

        if (buffs["jump"] > 0) buffs["jump"] -= Time.deltaTime;
        else
        {
            jumpForce = basicJumpForce;
        }

    }

    // 跳下平台
    protected virtual IEnumerator DisablePlatformCollision()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var col in hitColliders)
        {
            PlatformEffector2D e = col.GetComponent<PlatformEffector2D>();
            if (e != null) Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col, true);
        }
        yield return new WaitForSeconds(0.5f);
        foreach (var col in hitColliders)
        {
            PlatformEffector2D e = col.GetComponent<PlatformEffector2D>();
            if (e != null) Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col, false);
        }
    }

    protected virtual void HandleSwitchGun()
    {
        if (GetSwitchGunInput() && guns.Count > 0)
        {
            trySwitchGun = true;
        }
        if (trySwitchGun && !isRecoiling)
        {
            SwitchGun();
        }
    }
    private void SwitchGun()
    {
        AmmoTimer = 0;
        currentGunIndex = (currentGunIndex + 1) % guns.Count;
        currentGun = guns[currentGunIndex];
        Debug.Log("切换至:" + currentGun.data.gunName);

        UpdateWeaponModel(currentGun);

        // 处理弹药逻辑（原有）
        if (currentGun.nowAmmo == -1)
            currentGun.nowAmmo = currentGun.data.maxAmmo;
        Debug.Log($"子弹: {currentGun.nowAmmo}/{currentGun.data.maxAmmo}");
        trySwitchGun = false;
    }

    protected virtual void HandleShoot()
    {
        if (currentGun == null) return;
        if (currentGun.nowAmmo == 0 && currentGun.AmmoNum == 0) return;
        if (!isReloadAmmo && currentGun.data.needAmmo && currentGun.nowAmmo <= 0)
        {
            isReloadAmmo = true;
            AmmoTimer = currentGun.data.AmmoTime * (1.0f - reloadSpeed);
        }
        if (isReloadAmmo)
        {
            AmmoTimer -= Time.deltaTime;
            if (AmmoTimer <= 0)
            {
                isReloadAmmo = false;
                currentGun.reloadAmmo();
            }
            return;
        }

        float fireInterval = 1f / currentGun.data.fireRate;
        if (Time.time - lastShootTime < fireInterval) return;

        if (GetShootInput())
        {
            Debug.Log("shoot");
            lastShootTime = Time.time;
            shootController = (shootController + 1) % 2;
            if (currentGun.data.needAmmo)
            {
                currentGun.nowAmmo--; // 每次射击 -1 子弹
            }
            Vector2 spawnPos;
            if (muzzleTransform != null)
            {
                spawnPos = muzzleTransform.position;      // 使用枪口位置
            }
            else
            {
                spawnPos = (Vector2)transform.position + new Vector2(1f, 0f); // 备用方案
            }
            spawnPos.x = Mathf.Abs(spawnPos.x - transform.position.x) * lastMoveDirection + transform.position.x;
            GameObject newBullet = Instantiate(currentGun.data.bulletPrefab, spawnPos, Quaternion.identity);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetStatus(currentGun.data.bulletSpeed, lastMoveDirection, currentGun.data.bulletATK, currentGun.data.force_x, currentGun.data.force_y, GetComponent<Collider2D>());
            }
            if (!isRecoiling) StartCoroutine(WeaponRecoilCoroutine());
        }

    }

    protected virtual IEnumerator WeaponRecoilCoroutine()
    {
        if (currentWeaponObject == null || currentGun == null)
        {
            isRecoiling = false;
            yield break;
        }

        isRecoiling = true;

        // 获取当前武器的后坐力参数
        float recoilDist = currentGun.data.recoilDistance;
        float recoilRot = currentGun.data.recoilRotation;
        float peakDur = currentGun.data.recoilPeakDuration;
        float returnDur = currentGun.data.recoilReturnDuration;
        float holdDur = currentGun.data.recoilHoldDuration;

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

    //投掷手榴弹
    protected virtual void HandleGrenade()
    {
        if (currentThrowTimer > 0)
        {
            currentThrowTimer -= Time.deltaTime;
            return;
        }
        if (GetThrowGrenadeInput() && GrenadeCount > 0)
        {
            GrenadeCount--;
            currentThrowTimer = throwTime;
            GameObject newGrenade = Instantiate(grenadePrefab, grenadePoint.transform.position, Quaternion.identity);
            Grenade grenadeScript = newGrenade.GetComponent<Grenade>();
            if (grenadeScript != null)
            {
                grenadeScript.setStatus(new Vector2(throwForce_x * lastMoveDirection, throwForce_y) + rb.velocity);
            }
        }
    }
    protected virtual void HandleReload()
    {
        if (currentGun == null || !currentGun.data.needAmmo || currentGun.AmmoNum == 0) return;

        if (GetReloadInput() && !isReloadAmmo)
        {
            isReloadAmmo = true;
            AmmoTimer = currentGun.data.AmmoTime * (1.0f - 1.0f * currentGun.nowAmmo / currentGun.data.maxAmmo) * (1.0f - reloadSpeed);
        }
    }

    // ===== 供UI调用的公共方法 =====
    public int GetCurrentAmmo()
    {
        if (currentGun == null) return 0;
        return currentGun.nowAmmo;
    }

    public int GetAmmoNum()
    {
        if (currentGun == null) return 0;
        return currentGun.AmmoNum;
    }
    public int GetMaxAmmo()
    {
        if (currentGun == null) return 0;
        return currentGun.data.maxAmmo;
    }

    public string GetCurrentGunName()
    {
        if (currentGun == null) return "无枪械";
        return currentGun.data.gunName;
    }

    public bool IsNeedAmmo()
    {
        if (currentGun == null) return false;
        return currentGun.data.needAmmo;
    }
    public float GetAmmoTime() { return AmmoTimer; }
    public float GetMaxAmmoTime() { return currentGun.data.AmmoTime * (1.0f - reloadSpeed); }
    public Dictionary<string,float> GetBuffs() { return buffs; }
    public int GetDeathCount() { return DeathCount; }

    // 击退处理
    protected virtual void UpdateKnockback()
    {
        if (knockbackTimer > 0f) 
        {
            knockbackTimer -= Time.deltaTime;
            
        }
    }

    //受击
    public virtual void Attacked(float atk, Vector2 atkForce)
    {
        if (playerStatus == null) return;
        
        playerStatus.TakeDamage(atk); // 调用PlayerStatus的扣血（优先扣护盾）
        if (playerStatus.currentHp <= 0)
        {
            Die();
            return;
        }
        if (playerStatus.currentShield <= 0)
        {
            isKnockback = true;
            knockbackTimer = knockbackDuration;
            rb.velocity = atkForce;
            jumpCountRemain = maxJumpCount;
            useInertia = true;
        }
    }

    //加血
    public virtual void addBlood(float amount)
    {
        if (playerStatus == null) return;
        playerStatus.HealthRecovery(amount);
    }
    //加盾
    public virtual void addShield(float amount)
    {
        if (playerStatus == null) return;
        playerStatus.ShieldRecovery(amount);
    }
    //变大/变小Buff
    public virtual void changeSize(float percent,float buffTime)
    {
        if (percent > 1){
            buffs["big"] = buffTime;
            buffs["small"] = 0;
        }
        if (percent < 1)
        {
            buffs["big"] = 0;
            buffs["small"] = buffTime;
        }
        currentWidth = basicWidth * percent;
        currentHeight = basicHeight * percent;
        transform.localScale = new Vector3(currentWidth * lastMoveDirection, currentHeight, -0.5f);
    }
    //加速/减速Buff
    public virtual void changeSpeed(float percent, float buffTime)
    {
        if (percent > 1)
        {
            buffs["fast"] = buffTime;
            buffs["slow"] = 0;
        }
        if (percent < 1)
        {
            buffs["fast"] = 0;
            buffs["slow"] = buffTime;
        }
        MoveSpeed = basicMoveSpeed * percent;
    }
    //跳跃Buff
    public virtual void changeJump(float percent, float buffTime)
    {
        buffs["jump"] = buffTime;
        jumpForce = basicJumpForce * percent;
    }

    //丢弃枪械处理
    protected virtual void HandleDropGun()
    {
        if (GetDropGunInput() && currentGun.data.gunName != "Pistol")
        {
            GameObject newDropGun = Instantiate(dropGunPrefab, weaponPivot.transform.position, Quaternion.identity);
            newDropGun.transform.localScale = transform.localScale;
            SpriteRenderer picture = newDropGun.GetComponent<SpriteRenderer>();
            Sprite newGunSprite = Resources.Load<Sprite>($"{gunImageFloderPath}/{currentGun.data.gunName}");
            picture.sprite = newGunSprite;
            DropGun newGun = newDropGun.GetComponent<DropGun>();
            if (newGun != null)
            {
                newGun.setStatus(new Vector2(-1 * lastMoveDirection * 2.0f, 2.0f));
            }
            guns.RemoveAt(currentGunIndex);
            trySwitchGun = true;
        }
    }

    public void addGun(GameObject gun)
    {
        GameObject newGunPrefab = Instantiate(gun);
        Gun newGun = newGunPrefab.GetComponent<Gun>();
        if (newGun != null && guns.Count < maxGunCount) guns.Add(newGun);
    }
    
    public void addAmmo(int num)
    {
        currentGun.AmmoNum += num * currentGun.data.maxAmmo;
    }
    public void addGrenade(int num)
    {
        GrenadeCount += num;
    }
    //掉下虚空处理
    protected virtual void CheckVoid()
    {
        if (!isInVoid && transform.position.y < VoidHeight)
        {
            isInVoid = true;
            respawnTimer = respawnTime;
            Attacked(VoidDamage, Vector2.zero);
            transform.position = new Vector2(10000, 10000);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            Initial();
        }
    }
    //死亡处理
    protected virtual void Die()
    {
        DeathCount++;
        isInVoid = true;
        respawnTimer = respawnTime;
        transform.position = new Vector2(10000, 10000);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        isRecoiling = false;
        Initial();
    }
    //材质贴图更换
    void changeMaterialTexture(Texture textureHead,Texture textureBody,Texture textureHand)
    {
        rendererArray=transform.GetComponentsInChildren<Renderer>(true);
        Texture[] textures= {textureBody,textureHead,textureHand,textureHand};
        for (int i = 0; i < rendererArray.Length; i++)
        {
            rendererArray[i].material.mainTexture=textures[i];
            //rendererArray[i].material.SetTexture("_MainTex", textures[i]);
            //rendererArray[i].material.SetTexture("_Emission", textures[i]);
            Debug.Log("rendererName:"+rendererArray[i].name);
            Debug.Log("textureName:" + textures[i].name);
        }
    }

    public float GetLastMoveDirection() => lastMoveDirection;
    public bool IsInVoid() { return isInVoid; }
}