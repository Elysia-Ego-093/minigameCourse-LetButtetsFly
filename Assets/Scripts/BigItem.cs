using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigItem : PickableItem
{
    [Header("变大倍率")]
    public float percent;
    [Header("Buff时间")]
    public float BuffTime;

    protected override void Effect(PlayerController player)
    {
        player.changeSize(percent, BuffTime);
        Destroy(gameObject);
    }
}
