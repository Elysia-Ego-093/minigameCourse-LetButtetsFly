using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoint : ItemManager
{
    protected GameObject currentItem = null;

    protected override void CreateItem()
    {
        if (currentItem == null)
        {
            GameObject item = GetItem();
            if (item != null)
            {
                GameObject newItem = Instantiate(item, transform.position, Quaternion.identity);
                if (newItem != null) currentItem = newItem;
            }
        }
    }
}
