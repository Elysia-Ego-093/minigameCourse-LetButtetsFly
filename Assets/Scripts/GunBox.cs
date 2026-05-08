using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBox : PickableItem
{
    [Header("Ç¹Ðµ»ã×Ü")]
    public List<GameObject> gunsPrefab = new List<GameObject>();

    protected override void Effect(PlayerController player)
    {
        if (player.guns.Count >= player.maxGunCount) return;
        player.addGun(gunsPrefab[Random.Range(0, gunsPrefab.Count)]);
        Destroy(gameObject);
    }
}
