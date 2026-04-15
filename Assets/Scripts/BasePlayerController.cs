using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePlayerController : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected PlayerStatus playerStatus;
    protected BoxCollider2D PlayerCollider;
    protected Transform PlayerTransform;

    [Header("体型设置")]
    public float basicWidth = 1f;
    public float basicHeight = 1f;
    public float bigWidth = 1.5f;
    public float bigHeight = 1.5f;
    public float smallWidth = 0.5f;
    public float smallHeight = 0.5f;
    protected float sizeBuffTimer;
    public float crouchHeightPercent = 0.5f;
    // 公开的读取属性
    public PlayerStatus PlayerStatus => playerStatus;

    [Header("移动设置")]
    public float basicMoveSpeed = 8f;
    protected float MoveSpeed;
    protected float moveInput;

    protected float lastMoveDirection = 1f;

    [Header("跳跃设置")]
    public float jumpForce = 12f;
    public int maxJumpCount = 2;
    protected int jumpCountRemain;
    protected bool isJumping;

    [Header("地面检测")]
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    protected bool isGrounded;

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
    protected float knockbackDuration = 0.3f;
    protected float knockbackTimer = 0f;

    [Header("枪械设置")]
    public List<GunData> guns = new List<GunData>();
    protected int currentGunIndex = 0;
    protected GunData currentGun;
    protected float lastShootTime = 0f;
    protected int shootController = 0;

    // 抽象方法 - 子类必须实现自己的输入逻辑
    protected abstract float GetHorizontalInput();
    protected abstract bool GetJumpInput();
    protected abstract bool GetSprintInput();
    protected abstract bool GetShootInput();
    protected abstract bool GetSwitchGunInput();
    protected abstract bool GetTestBulletInput();
    protected abstract bool GetJumpHoldInput();    
    protected abstract bool GetDownInput();        
    protected abstract bool GetReloadInput();   
    protected abstract bool GetDownFinishInput();
    protected abstract bool GetTestSizeInput();

    protected virtual void Start()
    {
        PlayerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<BoxCollider2D>();
        MoveSpeed = basicMoveSpeed;
        jumpCountRemain = maxJumpCount;
        currentStamina = maxStamina;
        recoverWaitTimer = staminaRecoverDelay;
        playerStatus = GetComponent<PlayerStatus>();
        if (playerStatus == null)
        {
            Debug.LogError("玩家物体缺少PlayerStatus组件");
        }
    }

    protected virtual void Update()
    {
        HandleSprint();
        HandleMove();
        HandleDown();
        HandleJump();
        UpdateStamina();
        UpdateKnockback();
        UpdateBuff();
        HandleSwitchGun();
        HandleShoot();
        HandleReload();

        if (GetTestBulletInput()) TestBullet();
        if (GetTestSizeInput()) TestSize();
    }
    protected virtual void TestSize()
    {
        changeSize(1.5f, 5f);
    }

    protected virtual void TestBullet()
    {
        Vector2 spawnPos = (Vector2)transform.position + lastMoveDirection * new Vector2(10f, 0.3f);
        GameObject newBullet = Instantiate(currentGun.bulletPrefab, spawnPos, Quaternion.identity);
        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetStatus(currentGun.bulletSpeed, -lastMoveDirection, currentGun.bulletATK, currentGun.force_x, currentGun.force_y);
        }
    }

    // 移动相关
    protected virtual void HandleMove()
    {
        if (isKnockback) return;
        moveInput = GetHorizontalInput();
        float finalSpeed = isSprinting ? MoveSpeed * sprintSpeedMultiplier : MoveSpeed;
        rb.velocity = new Vector2(moveInput * finalSpeed, rb.velocity.y);
        
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
        if (rb.velocity.y == 0 && GetDownInput())
        {
            Debug.Log("玩家蹲下");
            PlayerCollider.size = new Vector2(1f, crouchHeightPercent);
            PlayerCollider.offset = new Vector2(0, -(1f - crouchHeightPercent) / 2f);
        }
        if (GetDownFinishInput() || Mathf.Abs(rb.velocity.y) > 1f)
        {
            Debug.Log("玩家站起");
            PlayerCollider.size = new Vector2(1f, 1f);
            PlayerCollider.offset = Vector2.zero;
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
        
        if (rb.velocity.y == 0)
        {
            jumpCountRemain = maxJumpCount;
        }
        
        if (GetJumpInput() && jumpCountRemain > 0 && !isKnockback) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCountRemain--;
            isJumping = true;
        }
        
        if (!GetJumpHoldInput()) isJumping = false;
        
        if (!isJumping) rb.velocity += new Vector2(0, -9.81f * Time.deltaTime);
        rb.velocity += new Vector2(0, -9.81f * Time.deltaTime);
    }
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
        if (sizeBuffTimer > 0) sizeBuffTimer -= Time.deltaTime;
        else PlayerTransform.localScale = new Vector3(basicWidth, basicHeight, 1f);
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
            currentGunIndex = (currentGunIndex + 1) % guns.Count;
            currentGun = guns[currentGunIndex];
            Debug.Log("玩家1切换至：" + currentGun.gunName);
            if (currentGun.nowAmmo == -1)
            {
                currentGun.nowAmmo = currentGun.maxAmmo;
            }
            Debug.Log($"切换至：{currentGun.gunName}, 子弹: {currentGun.nowAmmo}/{currentGun.maxAmmo}");
        }
    }

    protected virtual void HandleShoot()
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

    protected virtual void HandleReload()
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

    // 受击处理
    protected virtual void UpdateKnockback()
    {
        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockback = false;
            }
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
        
        isKnockback = true;
        knockbackTimer = knockbackDuration;
        rb.velocity = atkForce;
        jumpCountRemain = maxJumpCount;
    }

    //加血
    public virtual void addBlood(float amount)
    {
        if (playerStatus == null) return;
        playerStatus.HealthRecovery(amount);
    }
    //变大/变小Buff
    public virtual void changeSize(float percent,float buffTime)
    {
        sizeBuffTimer = buffTime;
        PlayerTransform.localScale = new Vector3(basicWidth * percent, basicHeight * percent, 1f);
    }

    //死亡处理
    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }

    public float GetLastMoveDirection() => lastMoveDirection;
}