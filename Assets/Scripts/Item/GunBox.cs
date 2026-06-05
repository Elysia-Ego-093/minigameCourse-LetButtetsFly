using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GunBox : PickableItem
{
    [Header("Ç¹Ðµ»ã×Ü")]
    public List<SerializablePair> gunsPrefab = new List<SerializablePair>();

    protected override void Effect(PlayerController player)
    {
        if (player.guns.Count >= player.maxGunCount) return;
        player.addGun(GetGun());
        if (PickUpSound != null)
        {
            AudioSource.PlayClipAtPoint(PickUpSound, transform.position, 1.0f);
        }
        Destroy(gameObject);
    }

    protected GameObject GetGun()
    {
        int totalWeight = 0;
        foreach (var gun in gunsPrefab)
        {
            totalWeight += gun.value;
        }
        if (totalWeight == 0) return null;
        int randomWeight = UnityEngine.Random.Range(0, totalWeight), currentWeight = 0;
        foreach (var gun in gunsPrefab)
        {
            currentWeight += gun.value;
            if (currentWeight >= randomWeight) return gun.key;
        }
        return null;
    }
}
