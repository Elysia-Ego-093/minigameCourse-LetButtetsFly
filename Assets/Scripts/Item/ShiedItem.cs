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
        if (PickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(PickUpSound, transform.position, GameData.Instance.SoundVolume);
        }
        Destroy(gameObject);
    }
}
