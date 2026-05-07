using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePlayerController : MonoBehaviour
{
    protected Rigidbody2D rb;

    [Header("移动设置")]
    public float moveSpeed = 8f;
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

    [Header("血量设置")]
    public float maxBlood = 1000f;
    public float blood;


    // 抽象方法 - 子类必须实现自己的输入逻辑
    protected abstract float GetHorizontalInput();
    protected abstract bool GetJumpInput();
    protected abstract bool GetSprintInput();
    protected abstract bool GetShootInput();
    protected abstract bool GetSwitchGunInput();
    protected abstract bool GetTestInput();
    protected abstract bool GetJumpHoldInput();    
    protected abstract bool GetDownInput();        
    protected abstract bool GetReloadInput();   

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCountRemain = maxJumpCount;
        currentStamina = maxStamina;
        recoverWaitTimer = staminaRecoverDelay;
        blood = maxBlood;
    }

    protected virtual void Update()
    {
        HandleSprint();
        HandleMove();
        HandleJump();
        UpdateStamina();
        UpdateKnockback();
    }

    // 移动相关
    protected virtual void HandleMove()
    {
        if (isKnockback) return;
        moveInput = GetHorizontalInput();
        float finalSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
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

    // 跳下平台
    protected virtual IEnumerator DisablePlatformCollision()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var col in hitColliders)
        {
            PlatformEffector2D e = col.GetComponent<PlatformEffector2D>();
            if (e != null) Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col, true);
        }
        yield return new WaitForSeconds(0.3f);
        foreach (var col in hitColliders)
        {
            PlatformEffector2D e = col.GetComponent<PlatformEffector2D>();
            if (e != null) Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col, false);
        }
    }

    // 公共方法
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
    public virtual void Attacked(float atk, Vector2 atkForce)
    {
        blood -= atk;
        if (blood <= 0)
        {
            Die();
            return;
        }
        isKnockback = true;
        knockbackTimer = knockbackDuration;
        rb.velocity = atkForce;
        jumpCountRemain = maxJumpCount;
    }

    public virtual void addBlood(float amount)
    {
        blood = Mathf.Min(blood + amount, maxBlood);
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }

    public float GetLastMoveDirection() => lastMoveDirection;
}