using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : PickableItem
{
    [Header("∂‹¡ø")]
    public float addShield;

    protected override void Effect(PlayerController player)
    {
        player.addShield(addShield);
        Destroy(gameObject);
    }
}
