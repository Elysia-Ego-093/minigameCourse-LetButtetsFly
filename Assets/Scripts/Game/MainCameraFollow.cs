using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{
    [Header("��������")]
    public List<PlayerController> players = new List<PlayerController>();
    public float smoothSpeed = 5f;
    public float minZoom = 5f;
    public float zoomSpeed = 2f;

    [Header("BGM")]
    public List<AudioClip> BGMs = new List<AudioClip>();

    private AudioSource BGMplayer;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        BGMplayer = GetComponent<AudioSource>();
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (!BGMplayer.isPlaying)
        {
            BGMplayer.clip = BGMs[Random.Range(0, BGMs.Count)];
            BGMplayer.Play();
        }
        BGMplayer.volume = GameData.Instance.BGMVolume * 0.3f;
    }
    private void LateUpdate()
    {
        if (players.Count == 0) return;

        // 1. 计算有效玩家的中点
        Vector2 center = Vector2.zero;
        int validCount = 0;
        foreach (var p in players)
        {
            if (p != null && !p.IsInVoid())
            {
                center += (Vector2)p.transform.position;
                validCount++;
            }
        }
        if (validCount == 0) return;
        center /= validCount;

        // 2. 计算所有有效玩家到中点的最大距离
        float maxDist = 0f;
        foreach (var p in players)
        {
            if (p != null && !p.IsInVoid())
            {
                float dist = Vector2.Distance(center, p.transform.position);
                if (dist > maxDist) maxDist = dist;
            }
        }

        float margin = 0.10f;       
        float requiredSize = maxDist * (1 + margin);

        float targetZoom = Mathf.Max(minZoom, requiredSize);
        Vector3 desiredPos = new Vector3(center.x, center.y, -10f);
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }
}
