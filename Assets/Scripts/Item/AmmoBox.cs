using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : PickableItem
{
    [Header("补给弹药基数量")]
    public int AmmoNum;

    protected override void Effect(PlayerController player)
    {
        player.addAmmo(AmmoNum);
        if (PickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(PickUpSound, transform.position, GameData.Instance.SoundVolume);
        }
        Destroy(gameObject);
    }
}
