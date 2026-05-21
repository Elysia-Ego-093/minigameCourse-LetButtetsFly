using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemManager : ItemManager
{
    [Header("生成区域设置")]
    public Vector2 leftLowerCorner;
    public Vector2 RightUpperCorner;

    [Header("最大数量")]
    public int maxCount;
    protected List<GameObject> currentItems = new List<GameObject>();
    protected override void CreateItem()
    {
        GameObject item = GetItem();
        if (item == null) return;
        if (currentItems.Count < maxCount)
        {
            float x = Random.Range(leftLowerCorner.x, RightUpperCorner.x), y = Random.Range(leftLowerCorner.y, RightUpperCorner.y);
            GameObject newItem = Instantiate(item, new Vector2(x, y), Quaternion.identity);
            currentItems.Add(newItem);
        }
        else
        {
            for(int i=0;i<currentItems.Count;i++)
            {
                if (currentItems[i] == null)
                {
                    float x = Random.Range(leftLowerCorner.x, RightUpperCorner.x), y = Random.Range(leftLowerCorner.y, RightUpperCorner.y);
                    GameObject newItem = Instantiate(item, new Vector2(x, y), Quaternion.identity);
                    currentItems[i] = newItem;
                    return;
                }
            }
        }
        
    }
}
