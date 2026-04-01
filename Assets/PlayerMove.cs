using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour 
{
    private Rigidbody2D rb;

    // 移动跳跃
    public float moveSpeed = 8f, jumpForce = 12f;
    public int maxJumpCount = 2;
    private int jumpCountRemain;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    private float moveInput;
    private bool isGrounded;
    private bool isJumping;

    // 枪械系统
    public List<GunData> guns = new List<GunData>();   // 枪械列表（在 Inspector 中拖入）
    private int currentGunIndex = 0;
    private GunData currentGun;                         // 当前使用的枪械数据
    private float lastShootTime = 0f;                   // 上次发射时间
    private float lastMoveDirection = 1f;               // 最后移动方向（用于子弹方向）

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCountRemain = maxJumpCount;


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

    void Update()
    {
        move();
        jump();
        SwitchGun();  
        Shoot();       
    }
    // 切换枪械
    private void SwitchGun()
    {
        if (Input.GetKeyDown(KeyCode.R) && guns.Count > 0)
        {
            currentGunIndex = (currentGunIndex + 1) % guns.Count;
            currentGun = guns[currentGunIndex];
            Debug.Log("切换至：" + currentGun.gunName);
        }
    }
    // 射击
    private void Shoot()
    {
        if (currentGun == null) return;
        // 射速冷却检查
        float fireInterval = 1f / currentGun.fireRate;
        if (Time.time - lastShootTime < fireInterval)
            return;
        if (Input.GetKeyDown(KeyCode.J))
        {
            lastShootTime = Time.time;
            Vector2 spawnPos = (Vector2)transform.position + lastMoveDirection * new Vector2(0.6f, 0f);
            GameObject newBullet = Instantiate(currentGun.bulletPrefab, spawnPos, Quaternion.identity);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetSpeed(currentGun.bulletSpeed, lastMoveDirection);
            }
        }
    }

    // 移动
    private void move()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput != 0)
        {
            lastMoveDirection = Mathf.Sign(moveInput);
        }
    }

    // 跳跃（保持不变）
    private void jump()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(DisablePlatformCollision());
            return;
        }
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.1f, groundLayer);
        if (isGrounded && !wasGrounded) jumpCountRemain = maxJumpCount;
        if (Input.GetButtonDown("Jump") && jumpCountRemain > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCountRemain--;
            isJumping = true;
        }
        if (Input.GetButtonUp("Jump")) isJumping = false;
        if (!isJumping) rb.velocity += new Vector2(0, -9.81f * Time.deltaTime);
    }

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
}