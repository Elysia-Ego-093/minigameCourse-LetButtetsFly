using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Bullet_2 : Bullet
{
    private GameObject Player1;
    private GameObject Player2;
    public float rorateSpeed = 1f;
    public float addRorate = 90f;
    public float addSpeed = 0.1f;
    private float Timer = 1f;
    private void Start()
    {
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        Timer = 1f;
    }
    void Update()
    {
        UpdateVilocity();

        col.size = new Vector2(Mathf.Abs(rb.velocity.x * 0.05f) / transform.localScale.x, col.size.y);
        col.offset = new Vector2(col.size.x / 2f * (Mathf.Abs(rb.velocity.x) / rb.velocity.x), 0f);
        // ×Ô¶¯Ïú»Ù
        if (transform.position.x > 300
            || transform.position.x < -300
            || transform.position.y < -10
            || transform.position.y > 100)
            Destroy(gameObject);
    }
    private void UpdateVilocity()
    {
        speed += addSpeed * Time.deltaTime;
        rorateSpeed += addRorate * Time.deltaTime;
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;

            float targetAngle = GetDirection();
            float currentAngle = transform.eulerAngles.z;
            float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
            float maxRotate = rorateSpeed * Time.deltaTime;
            if (Mathf.Abs(angleDiff) <= maxRotate)
            {
                currentAngle = targetAngle;
            }
            else
            {
                currentAngle += maxRotate * Mathf.Sign(angleDiff);
            }
            transform.eulerAngles = new Vector3(0, 0, currentAngle);
            float rad = currentAngle * Mathf.Deg2Rad;
            rb.velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * speed;
        }
        else
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }
    private float GetDirection()
    {
        Vector2 direction = Vector2.zero;
        if (Player1 != null && Player2 != null)
        {
            float distance1 = Vector2.Distance(transform.position, Player1.transform.position);
            float distance2 = Vector2.Distance(transform.position, Player2.transform.position);
            if (distance1 > distance2)
            {
                direction = (Player2.transform.position - transform.position).normalized;
            }
            else
            {
                direction = (Player1.transform.position - transform.position).normalized;
            }
        }
        else if (Player1 != null)
        {
            direction = (Player1.transform.position - transform.position).normalized;
        }
        else if (Player2 != null)
        {
            direction = (Player1.transform.position - transform.position).normalized;
        }
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
