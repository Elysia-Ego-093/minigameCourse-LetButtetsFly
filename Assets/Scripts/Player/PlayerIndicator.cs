using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    public PlayerController player;

    private float offset = 0f;
    private float speed = 0.2f;

    // Update is called once per frame
    void Update()
    {
        offset += speed * Time.deltaTime;
        if (MathF.Abs(offset) > 0.1f) speed *= -1f;
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y + (player.transform.localScale.y + transform.localScale.y) / 2 + offset);
    }
}
