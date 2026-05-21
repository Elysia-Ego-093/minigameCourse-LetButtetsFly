using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBox : PickableItem
{
    [Header("手榴弹补给数量")]
    public int num;

    protected override void Effect(PlayerController player)
    {
        player.addGrenade(num);
        Destroy(gameObject);
    }
}
