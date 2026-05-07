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
        GameObject newGunPrefab = Instantiate(gunsPrefab[Random.Range(0, gunsPrefab.Count)]);
        Gun newGun = newGunPrefab.GetComponent<Gun>();
        if (newGun!=null)
        {
            player.addGun(newGun);
        }
        Destroy(gameObject);
    }
}
