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
            Vector2 position = GetPosition();
            GameObject newItem = Instantiate(item, position, Quaternion.identity);
            currentItems.Add(newItem);
        }
        else
        {
            for (int i = 0; i < currentItems.Count; i++) 
            {
                if (currentItems[i] == null)
                {
                    Vector2 position = GetPosition();
                    GameObject newItem = Instantiate(item, position, Quaternion.identity);
                    currentItems[i] = newItem;
                    return;
                }
            }
        }
        
    }

    private Vector2 GetPosition()
    {
        Vector2 position;
        Collider2D col = null;
        int cnt = 0;
        do
        {
            float x = Random.Range(leftLowerCorner.x, RightUpperCorner.x), y = Random.Range(leftLowerCorner.y, RightUpperCorner.y);
            position = new Vector2(x, y);
            col = Physics2D.OverlapCircle(position, 2f);
            if (cnt++ > 100)
            {
                Debug.LogError("无法获取合适的生成点位!");
                return Vector2.zero;
            }
        } while (!(col == null && CheckBelow(position)));
        return position;
    }
    private bool CheckBelow(Vector2 position)
    {
        RaycastHit2D[] belowInfo = Physics2D.RaycastAll(position, Vector2.down);
        return belowInfo.Length > 0;
    }
}
