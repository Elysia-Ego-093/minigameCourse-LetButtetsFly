using UnityEngine;
using System.Collections;

public class SequencePlayer : MonoBehaviour
{
    public AudioClip sound;
    public float frameRate = 10f;    // Ö¡ÂÊ
    private SpriteRenderer spriteRenderer;
    private readonly string ImageFloderPath = "picture/Explosive/ezgif-split";

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject background = GameObject.Find("BackGround");
        if(background != null)
        {
            transform.position = background.transform.position;
            transform.localScale = background.transform.localScale * 10f;
        }
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        if(mainCamera != null)
        {
            AudioSource.PlayClipAtPoint(sound, mainCamera.transform.position, GameData.Instance.SoundVolume);
        }
        float frameTime = 1f / frameRate;

        for (int i = 0; i < 201; i++)
        {
            string frameName = "";
            if (i < 10)
            {
                frameName = $"frame_00{i}_delay-0.1s";
            }
            if (10 <= i && i < 100)
            {
                frameName = $"frame_0{i}_delay-0.1s";
            }
            if (i >= 100)
            {
                frameName = $"frame_{i}_delay-0.1s";
            }
            spriteRenderer.sprite = Resources.Load<Sprite>($"{ImageFloderPath}/{frameName}");
            yield return new WaitForSeconds(frameTime);
        }
        Destroy(gameObject);
    }
}