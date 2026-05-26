using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    public PlayerController player;

    private float offset = 0f;
    private float speed = 0.2f;
    private Collider2D playerCollider;

    private void Start()
    {
        playerCollider = player.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerCollider = player.GetComponent<BoxCollider2D>();
        offset += speed * Time.deltaTime;
        if (MathF.Abs(offset) > 0.1f) speed *= -1f;
        transform.position = player.GetIndicatorPosition() + new Vector2(0, transform.localScale.y / 2 + offset);
    }
}
