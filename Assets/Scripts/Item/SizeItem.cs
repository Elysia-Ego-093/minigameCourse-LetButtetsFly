using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeItem : PickableItem
{
    public bool isDebuff;

    protected override void Effect(PlayerController player)
    {
        player.changeSize(isDebuff);
        if (PickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(PickUpSound, transform.position, GameData.Instance.SoundVolume);
        }
        Destroy(gameObject);
    }
}
