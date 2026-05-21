using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpItem : PickableItem
{
    [Header("ÌøÔ¾±¶ÂÊ")]
    public float percent;
    [Header("BuffÊ±¼ä")]
    public float BuffTime;

    protected override void Effect(PlayerController player)
    {
        player.changeJump(percent, BuffTime);
        Destroy(gameObject);
    }
}
