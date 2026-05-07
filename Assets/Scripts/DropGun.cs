using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGun : MonoBehaviour
{
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.y < -10) Destroy(gameObject);
    }

    public void setStatus(Vector2 force)
    {
        rb.velocity = force;
        rb.angularVelocity = Random.Range(120f, 360f);
    }
}
