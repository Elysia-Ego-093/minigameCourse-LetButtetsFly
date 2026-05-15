using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : PickableItem
{
    [Header("¼ÓÑªÁ¿")]
    public float addShield;

    protected override void Effect(PlayerController player)
    {
        player.addShield(addShield);
        Destroy(gameObject);
    }
}
