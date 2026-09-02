using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{
    [Header("��������")]
    public List<PlayerController> players = new List<PlayerController>();
    public BOSS boss;
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
        if (boss != null)
        {
            center += (Vector2)boss.transform.position;
            validCount++;
        }
        if (validCount == 0) return;
        center /= validCount;

        // 2. 计算所有有效玩家到中点的最大距离
        float maxDist_x = 0f, maxDist_y = 0f;
        foreach (var p in players)
        {
            if (p != null && !p.IsInVoid())
            {
                float dist_x = Mathf.Abs(p.transform.position.x - center.x);
                float dist_y = Mathf.Abs(p.transform.position.y - center.y);
                if (dist_x > maxDist_x) maxDist_x = dist_x;
                if (dist_y > maxDist_y) maxDist_y = dist_y;
            }
        }
        if(boss != null)
        {
            float dist_x = Mathf.Abs(boss.transform.position.x - center.x);
            float dist_y = Mathf.Abs(boss.transform.position.y - center.y);
            if (dist_x > maxDist_x) maxDist_x = dist_x;
            if (dist_y > maxDist_y) maxDist_y = dist_y;
        }

        float requiredSize = Mathf.Max(maxDist_x * 1.5f, maxDist_y * 2.5f) / 2f;

        float targetZoom = Mathf.Max(minZoom, requiredSize);
        Vector3 desiredPos = new Vector3(center.x, center.y, -10f);
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }
}
