using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAnimation : MonoBehaviour
{
    public List<Sprite> Images = new List<Sprite>();
    public float AnimationTime = 0.5f;
    private SpriteRenderer sprite;
    private float Timer = 0f;
    private float UpdateTime;
    private int index = 0;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        UpdateTime = AnimationTime / Images.Count;
        sprite.sprite = Images[index];
        Timer = UpdateTime;
    }

    void Update()
    {
        if (Timer >= 0f) Timer -= Time.deltaTime;
        else
        {
            index++;
            if (index >= Images.Count)
            {
                Destroy(gameObject);
                return;
            }
            sprite.sprite = Images[index];
            Timer = UpdateTime;
        }
    }
}
