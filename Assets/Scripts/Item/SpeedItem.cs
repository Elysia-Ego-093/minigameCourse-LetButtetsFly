using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : PickableItem
{
    [Header("速度倍率")]
    public float percent;
    [Header("Buff时间")]
    public float BuffTime;

    protected override void Effect(PlayerController player)
    {
        player.changeSpeed(percent, BuffTime);
        Destroy(gameObject);
    }
}
