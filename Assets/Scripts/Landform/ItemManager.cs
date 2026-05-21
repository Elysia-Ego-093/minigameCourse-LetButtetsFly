using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ItemManager : MonoBehaviour
{
    [Header("可生成物品列表及权重")]
    public List<SerializablePair> items = new List<SerializablePair>();

    [Header("时间间隔参数")]
    public float spawnInterval;
    public float RandomTime;
    protected float spwanTimer = 0f;
    void Start()
    {
        spwanTimer = spawnInterval + UnityEngine.Random.Range(-spawnInterval, spawnInterval);
    }

    void Update()
    {
        if (spwanTimer > 0)
        {
            spwanTimer -= Time.deltaTime;
            return;
        }
        spwanTimer = spawnInterval + UnityEngine.Random.Range(-RandomTime, RandomTime);
        CreateItem();
    }
    protected GameObject GetItem()
    {
        int totalWeight = 0;
        foreach(var item in items)
        {
            totalWeight += item.value;
        }
        if (totalWeight == 0) return null;
        int randomWeight = UnityEngine.Random.Range(0, totalWeight), currentWeight = 0;
        foreach(var item in items)
        {
            currentWeight += item.value;
            if (currentWeight >= randomWeight) return item.key;
        }
        return null;
    }
    protected abstract void CreateItem();
}

[Serializable]
public class SerializablePair
{
    public GameObject key;
    [Range(0, 100)]
    public int value;
}