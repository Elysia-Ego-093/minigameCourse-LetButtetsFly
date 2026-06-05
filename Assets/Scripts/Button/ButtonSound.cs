using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource sound;
    private float Timer = 0.1f;

    void Update()
    {
        if (Timer > 0f)
        {
            Timer -= Time.deltaTime;
            return;
        }
        if (!sound.isPlaying)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
