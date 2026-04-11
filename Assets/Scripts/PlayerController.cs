using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
    private Rigidbody2D rb;

    [Header("玩家ID")]
    public int playerID = 1;

    [Header("移动设置")]
    public float moveSpeed = 8f;
    private float moveInput;

    [Header("跳跃设置")]
    public float jumpForce = 12f;
    public int maxJumpCount = 2;
    private int jumpCountRemain;
    private bool isJumping;

    [Header("地面检测")]
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("加速设置")]
    public float sprintSpeedMultiplier = 2f;
    public float staminaCostPerSecond = 50f;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRecoverDelay = 2f;
    public float staminaRecoverSpeed = 20f;
    private float recoverWaitTimer;
    private bool isSprinting;

    [Header("受击设置")]
    private bool isKnockback = false;

    [Header("血量设置")]
    public float maxBlood = 1000f;
    public float blood;

    [Header("枪械系统")]
    public List<GunData> guns = new List<GunData>();   // 枪械列表（在 Inspector 中拖入）
    private int currentGunIndex = 0;
    private GunData currentGun;                         // 当前使用的枪械数据
    private float lastShootTime = 0f;                   // 上次发射时间
    private float lastMoveDirection = 1f;               // 最后移动方向（用于子弹方向）
    private int shootController = 0;

    //-----------------------------------------------------
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCountRemain = maxJumpCount;
        currentStamina=maxStamina;
        recoverWaitTimer=staminaRecoverDelay;
        blood = maxBlood;
        if (guns.Count > 0)
        {
            currentGunIndex = 0;
            currentGun = guns[currentGunIndex];
        }
        else
        {
            Debug.LogError("未设置任何枪械！请在 Inspector 中为 guns 列表添加枪械数据。");
        }
    }
    //-----------------------------------------------------
    void Update()
    {   
        SprintInput();
        move();
        jump();
        UpdateStamina();
        SwitchGun();
        Shoot();
        if (Input.GetKey(KeyCode.U)) testbullet();//测试用例，用U键模拟敌方射击子弹
    }

    //测试用例，用U键模拟敌方射击子弹-------------------------------------------
    private void testbullet()
    {
        if (currentGun == null) return;
        // 射速冷却检查
        float fireInterval = 1f / currentGun.fireRate;
        if (Time.time - lastShootTime < fireInterval)
            return;
        lastShootTime = Time.time;
        Vector2 spawnPos = new Vector2(10f + 0.15f * shootController, -3.65f);
        GameObject newBullet = Instantiate(currentGun.bulletPrefab, spawnPos, Quaternion.identity);
        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetStatus(currentGun.bulletSpeed, -1, currentGun.bulletATK, currentGun.force_x, currentGun.force_y);
        }
    }
    
    // 切换枪械-----------------------------------------------------
    private void SwitchGun()
    {
        if (Input.GetKeyDown(KeyCode.R) && guns.Count > 0)
        {
            currentGunIndex = (currentGunIndex + 1) % guns.Count;
            currentGun = guns[currentGunIndex];
            Debug.Log("切换至：" + currentGun.gunName);
        }
    }

    // 射击---------------------------------------------------------------
    private void Shoot()
    {
        if (currentGun == null) return;
        // 射速冷却检查
        float fireInterval = 1f / currentGun.fireRate;
        if (Time.time - lastShootTime < fireInterval)
            return;
        if (Input.GetKey(KeyCode.J))
        {
            lastShootTime = Time.time;
            //shootController = (shootController + 1) % 2;
            Vector2 spawnPos = (Vector2)transform.position + lastMoveDirection * new Vector2(1f /*+ 0.15f * shootController*/, 0f);
            GameObject newBullet = Instantiate(currentGun.bulletPrefab, spawnPos, Quaternion.identity);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetStatus(currentGun.bulletSpeed, lastMoveDirection, currentGun.bulletATK, currentGun.force_x, currentGun.force_y);
            }
        }
    }

    //-----------------------------------------------------
    private void SprintInput(){
        moveInput = Input.GetAxisRaw("Horizontal");
        
        bool wantSprint = Input.GetKey(KeyCode.K) && Mathf.Abs(moveInput)>0.1f && currentStamina > 0;
        if(wantSprint){
            isSprinting = true;
            recoverWaitTimer = 0;
        }
        else{
            isSprinting = false;
        }
    }

    //左右移动-----------------------------------------------------
    private void move()
    {
        if (isKnockback) return;
        moveInput = Input.GetAxisRaw("Horizontal");
        float finalSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
        rb.velocity = new Vector2(moveInput * finalSpeed, rb.velocity.y);
        if (moveInput != 0)
        {
            lastMoveDirection = Mathf.Sign(moveInput);
        }
    }

    //-----------------------------------------------------
    private void UpdateStamina(){
        if(isSprinting){
            currentStamina-=staminaCostPerSecond*Time.deltaTime;
            currentStamina=Mathf.Max(currentStamina,0);
        }
        else{
            recoverWaitTimer+=Time.deltaTime;
            if(recoverWaitTimer >= staminaRecoverDelay){
                currentStamina+=staminaRecoverSpeed*Time.deltaTime;
                currentStamina=Mathf.Min(currentStamina,maxStamina);
            }
        }
    }


    //跳跃-----------------------------------------------------
    private void jump()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(DisablePlatformCollision());
            return;
        }
        if (rb.velocity.y == 0)
        { 
            jumpCountRemain = maxJumpCount;
            isKnockback = false;
        } 
        if (Input.GetButtonDown("Jump") && jumpCountRemain > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCountRemain--;
            isJumping = true;
            isKnockback = false;
        }
        if (Input.GetButtonUp("Jump")) isJumping = false;
        if (!isJumping) rb.velocity += new Vector2(0, -9.81f * Time.deltaTime);
        rb.velocity += new Vector2(0, -9.81f * Time.deltaTime);
    }

    //实现跳下平台-----------------------------------------------------
    System.Collections.IEnumerator DisablePlatformCollision()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach(var col in hitColliders)
        {
            PlatformEffector2D e = col.GetComponent<PlatformEffector2D>();
            if (e != null) Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col, true);
        }
        yield return new WaitForSeconds(0.3f);
        foreach(var col in hitColliders)
        {
            PlatformEffector2D e = col.GetComponent<PlatformEffector2D>();
            if (e != null) Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col, false);
        }
    }

    //受击函数，供外部引用--------------------------------------------------------
    public void Attacked(float atk,Vector2 atkForce)
    {
        blood -= atk;
        if (blood <= 0)
        {
            Die();
            return;
        }
        isKnockback = true;
        rb.velocity = atkForce;
        jumpCountRemain = maxJumpCount;
    }

    //回血，供外部引用-----------------------------------------------------
    public void addBlood(float amout)
    {
        blood = Mathf.Min(blood + amout, maxBlood);
    }

    //玩家死亡处理-----------------------------------------------
    private void Die()
    {
        gameObject.SetActive(false);
    }
    
}
