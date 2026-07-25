using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpItem : PickableItem
{
    protected override void Effect(PlayerController player)
    {
        player.changeJump();
        if (PickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(PickUpSound, transform.position, GameData.Instance.SoundVolume);
        }
        Destroy(gameObject);
    }
}
