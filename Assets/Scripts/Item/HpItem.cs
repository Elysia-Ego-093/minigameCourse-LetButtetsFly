using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : PickableItem
{
    [Header("¼ÓÑªÁ¿")]
    public float addHp;

    protected override void Effect(PlayerController player)
    {
        player.addBlood(addHp);
        Destroy(gameObject);
    }
}
