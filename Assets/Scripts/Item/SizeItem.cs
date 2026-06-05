using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeItem : PickableItem
{
    [Header("大小倍率")]
    public float percent;
    [Header("Buff时间")]
    public float BuffTime;

    protected override void Effect(PlayerController player)
    {
        player.changeSize(percent, BuffTime);
        if (PickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(PickUpSound, transform.position, 1.0f);
        }
        Destroy(gameObject);
    }
}
