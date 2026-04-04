using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//新增

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

    //新增-----------------------------------------------
    public float sprintSpeedMultiplier = 2f;
    public float staminaCostPerSecond = 50f;
    public float maxStamina = 100f;
    private float currentStamina;
    public float staminaRecoverDelay = 2f;
    public float staminaRecoverSpeed = 20f;
    private float recoverWaitTimer;
    private bool isSprinting;
    public Slider staminaSlider;
    //------------------------------------------------

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCountRemain = maxJumpCount;
        //新增-------------------------------------
        currentStamina=maxStamina;
        recoverWaitTimer=staminaRecoverDelay;
        //-----------------------------------------------
    }

    void Update()
    {   
        SprintInput(); //新增
        move();
        jump();
        UpdateStamina();//新增
        UpdateStaminaUI();//新增
    }

    //新增----------------------------------------------
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

    //-----------------------------------------------------
    private void move()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        float finalSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;//修改
        rb.velocity = new Vector2(moveInput * finalSpeed, rb.velocity.y);//修改
    }

//新增--------------------------------------------------------------
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
    
    private void UpdateStaminaUI(){
        if(staminaSlider!=null){
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }
//--------------------------------------------------------------------

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
