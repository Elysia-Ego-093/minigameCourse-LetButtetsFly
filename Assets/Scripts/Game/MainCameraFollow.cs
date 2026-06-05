using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{
    [Header("∏˙ÀÊ…Ë÷√")]
    public List<PlayerController> players = new List<PlayerController>();
    public float smoothSpeed = 5f;
    public float minZoom = 5f;
    public float zoomSpeed = 2f;

    [Header("BGM")]
    public List<AudioClip> BGMs = new List<AudioClip>();

    private AudioSource BGMplayer;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        BGMplayer = GetComponent<AudioSource>();
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (!BGMplayer.isPlaying)
        {
            BGMplayer.clip = BGMs[Random.Range(0, BGMs.Count - 1)];
            BGMplayer.Play();
        }
    }
    private void LateUpdate()
    {
        if (players.Count == 0) return;
        float x = 0f, y = 0f;
        int cnt = 0;
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].IsInVoid()) continue;
            cnt++;
            x += players[i].transform.position.x;
            y += players[i].transform.position.y;
        }
        Vector3 desiredPosition = new Vector3(x / cnt, y / cnt, -10f);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        Vector2 center = new Vector2(x / cnt, y / cnt);
        float zoom = Vector2.Distance(center, players[0].transform.position);
        for(int i = 1; i < players.Count; i++)
        {
            if (players[i].IsInVoid()) continue;
            zoom = Mathf.Min(zoom, Vector2.Distance(center, players[i].transform.position));
        }
        float targetZoom = Mathf.Max(minZoom, zoom * 0.8f);
        if (cnt != 0) 
        {
            transform.position = smoothPosition;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
        }
        
    }
}
