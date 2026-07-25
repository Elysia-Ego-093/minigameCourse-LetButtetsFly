using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : PickableItem
{
    public bool isDebuff;

    protected override void Effect(PlayerController player)
    {
        player.changeSpeed(isDebuff);
        if (PickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(PickUpSound, transform.position, GameData.Instance.SoundVolume);
        }
        Destroy(gameObject);
    }
}
