using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : PickableItem
{
    public bool isDebuff;
    public float buffTime;
    public float percent;

    protected override void Effect(PlayerController player)
    {
        player.changeSpeed(isDebuff, percent, buffTime);
        if (PickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(PickUpSound, transform.position, GameData.Instance.SoundVolume);
        }
        Destroy(gameObject);
    }
}
