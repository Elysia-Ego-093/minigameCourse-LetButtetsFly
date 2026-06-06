using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource sound;

    void Update()
    {
        
        if (!sound.isPlaying)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
