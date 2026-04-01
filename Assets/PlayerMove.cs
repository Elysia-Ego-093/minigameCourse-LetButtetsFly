using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour 
{
    private Rigidbody2D rb;
    public float moveSpeed = 8f, jumpForce = 12f;
    public int maxJumpCount = 2;
    private int jumpCountRemain;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    private float moveInput;
    private bool isGrounded;
    private bool isJumping;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCountRemain = maxJumpCount;
    }

    void Update()
    {
        move();
        jump();
    }
    private void move()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
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
