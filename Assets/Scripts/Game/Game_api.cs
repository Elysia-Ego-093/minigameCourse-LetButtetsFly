using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using System;

[System.Serializable]
public class GameData_Datas
{
    public List<GunData_from_web> guns;
    public List<CharacterData_from_web> characters;
    public BuffData_form_web big_buff;
    public BuffData_form_web small_buff;
    public BuffData_form_web fast_buff;
    public BuffData_form_web slow_buff;
    public BuffData_form_web jump_buff;
}

[System.Serializable]
public class StatusInformation
{
    public int id;
    public int reason_id;
    public float currentHP;
    public float currentShield;
    public StatusInformation(int id, int reason_id, float currentHP, float currentShield)
    {
        this.id = id;
        this.reason_id = reason_id;
        this.currentHP = currentHP;
        this.currentShield = currentShield;
    }
    StatusInformation() { }
}

[System.Serializable]
public class GameOverResult
{
    public int score;
    public bool isNew;
}

public class Game_api : MonoBehaviour
{
    public static Game_api Instance;

    public GameData_Datas gameDataResponse;
    public GameData_Datas gameDataSend;

    public StatusInformation StatusRequest;
    public StatusInformation StatusResponse;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void GetGameData(string json);

    [DllImport("__Internal")]
    public static extern int GetPlayerUid();

    [DllImport("__Internal")]
    public static extern void GetStatus(string json);

    [DllImport("__Internal")]
    public static extern void InitialStatus(int id, int character_id);

    [DllImport("__Internal")]
    public static extern void GameStart();

    [DllImport("__Internal")]
    public static extern void GameOver();
#endif

    private void Start()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //GameData.Instance.uid = 10001;

    }

    public int RequestPlayerUid()
    {
        Debug.Log("Unity向网页获取用户uid");
#if UNITY_WEBGL && !UNITY_EDITOR
        return GetPlayerUid();
#else
        Debug.Log("Unity编辑器模式，模拟数据 => uid:10001");
        return 10001;   // 编辑器测试
#endif
    }

    public void RequestInitialStatus(int id, int character_id)
    {
        Debug.Log($"选择了角色:{character_id}的玩家:{id}请求网页初始化角色数据");
#if UNITY_WEBGL && !UNITY_EDITOR
        InitialStatus(id, character_id);
#else
        Debug.Log("Unity编辑器模式，模拟数据");
        string testJson = @"
{
    ""id"":1,
    ""reason_id"":2,
    ""currentHP"":1000,
    ""currentShield"":0
}";
        ReceiveStatus(testJson);
#endif
    }


    public void RequestGameData()
    {
        gameDataSend.characters = GameData.Instance.CharacterDatas_from_web;
        gameDataSend.guns = GameData.Instance.GunDatas_from_web;
        gameDataSend.big_buff = GameData.Instance.big_buff;
        gameDataSend.small_buff = GameData.Instance.small_buff;
        gameDataSend.fast_buff = GameData.Instance.fast_buff;
        gameDataSend.slow_buff = GameData.Instance.slow_buff;
        gameDataSend.jump_buff = GameData.Instance.jump_buff;

        string json = JsonUtility.ToJson(gameDataSend);
        Debug.Log("Unity向网页发送数据:");
        Debug.Log(json);
#if UNITY_WEBGL && !UNITY_EDITOR
        GetGameData(json);
#else
        Debug.Log("Unity编辑器模式，模拟数据");
        string testJson = CreateTestJson();
#endif

    }

    public void ReceiveGameData(string json)
    {
        if (json == null)
        {
            Debug.LogError("接收网页的游戏数据信息JSON失败");
            return;
        }
        if(json.Length == 0)
        {
            Debug.LogError("接收网页的游戏数据信息JSON为空");
            return;
        }
        Debug.Log("收到网页数据:");
        Debug.Log(json);

        GameData_Datas gameDataResponse = JsonUtility.FromJson<GameData_Datas>(json);

        if (gameDataResponse == null)
        {
            Debug.LogError("JSON解析失败");
            return;
        }

        GameData.Instance.GunDatas_from_web = gameDataResponse.guns;
        GameData.Instance.CharacterDatas_from_web = gameDataResponse.characters;
        GameData.Instance.big_buff = gameDataResponse.big_buff;
        GameData.Instance.small_buff = gameDataResponse.small_buff;
        GameData.Instance.fast_buff = gameDataResponse.fast_buff;
        GameData.Instance.slow_buff = gameDataResponse.slow_buff;
        GameData.Instance.jump_buff = gameDataResponse.jump_buff;

    }

    public void RequestStatus(StatusInformation status)
    {
        string json = JsonUtility.ToJson(status);
        Debug.Log("Unity向网页发送数据:");
        Debug.Log(json);
#if UNITY_WEBGL && !UNITY_EDITOR
        GetStatus(json);
#else
        Debug.Log("Unity编辑器模式，模拟数据");
        string testJson = @"
{
    ""id"":0,
    ""reason_id"":2,
    ""currentHP"":1000,
    ""currentShield"":0
}";
        ReceiveStatus(testJson);
#endif
    }

    private void ReceiveStatus(string json)
    {
        if (json == null)
        {
            Debug.LogError("接收网页的血量状态更新信息JSON失败");
            return;
        }
        Debug.Log("收到网页数据:");
        Debug.Log(json);

        StatusInformation status = JsonUtility.FromJson<StatusInformation>(json);
        if (status == null)
        {
            Debug.LogError("JSON解析失败");
            return;
        }

        GameObject HP_entity = null;
        if (status.id == 0) HP_entity = GameObject.Find("Player1");
        if (status.id == 1) HP_entity = GameObject.Find("Player2");
        if (status.id == 2) HP_entity = GameObject.Find("BOSS");

        if (HP_entity != null)
        {
            if(status.id == 0 || status.id == 1)
            {
                HP_entity.GetComponent<BasePlayerController>().StatusResponse(status.currentHP, status.currentShield);
            }
            if (status.id == 2)
            {
                HP_entity.GetComponent<BOSS>().StatusResponse(status.currentHP);
            }
        }

    }

    public void RequestGameStart()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GameStart();
#endif
    }
    public void RequestGameOver()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GameOver();
#endif
    }
    public void GameOverResponse(string json)
    {
        if (json == null)
        {
            Debug.LogError("接收网页的血量状态更新信息JSON失败");
            return;
        }
        Debug.Log("收到网页数据:");
        Debug.Log(json);
        GameOverResult result = JsonUtility.FromJson<GameOverResult>(json);
        if (result == null)
        {
            Debug.LogError("JSON解析失败");
            return;
        }
        GameData.Instance.BOSS_scoer = result.score;
        GameData.Instance.isNew = result.isNew;
    }

    public void BOSS_TimeFinishResponse()
    {
        GameObject.Find("BOSS").GetComponent<BOSS>().TimeFinishAttack();
    }


    private string CreateTestJson()
    {

        string json = @"
{
    ""guns"":
    [
        {
            ""fireRate"":5,
            ""bulletSpeed"":25,
            ""bulletATK"":3,
            ""force_x"":6,
            ""force_y"":5,
            ""needAmmo"":false,
            ""maxAmmo"":20,
            ""BasicAmmoNum"":0,
            ""AmmoTime"":0,
            ""gunName"":""Pistol""
        },
        {
            ""fireRate"":24,
            ""bulletSpeed"":45,
            ""bulletATK"":4.5,
            ""force_x"":20,
            ""force_y"":7.5,
            ""needAmmo"":true,
            ""maxAmmo"":60,
            ""BasicAmmoNum"":120,
            ""AmmoTime"":2,
            ""gunName"":""M416""
        },
        {
            ""fireRate"":27.5,
            ""bulletSpeed"":40,
            ""bulletATK"":4.2,
            ""force_x"":20,
            ""force_y"":7.5,
            ""needAmmo"":true,
            ""maxAmmo"":60,
            ""BasicAmmoNum"":120,
            ""AmmoTime"":2,
            ""gunName"":""MachineGun""
        },
        {
            ""fireRate"":2,
            ""bulletSpeed"":70,
            ""bulletATK"":20,
            ""force_x"":50,
            ""force_y"":10,
            ""needAmmo"":true,
            ""maxAmmo"":20,
            ""BasicAmmoNum"":40,
            ""AmmoTime"":5,
            ""gunName"":""Rifle""
        },
        {
            ""fireRate"":30,
            ""bulletSpeed"":50,
            ""bulletATK"":4,
            ""force_x"":18,
            ""force_y"":7.3,
            ""needAmmo"":true,
            ""maxAmmo"":80,
            ""BasicAmmoNum"":160,
            ""AmmoTime"":1.7,
            ""gunName"":""SMG""
        },
        {
            ""fireRate"":1,
            ""bulletSpeed"":100,
            ""bulletATK"":50,
            ""force_x"":100,
            ""force_y"":20,
            ""needAmmo"":true,
            ""maxAmmo"":10,
            ""BasicAmmoNum"":20,
            ""AmmoTime"":10,
            ""gunName"":""Sniper_Rifle""
        },
        {
            ""fireRate"":6,
            ""bulletSpeed"":40,
            ""bulletATK"":23.3,
            ""force_x"":3,
            ""force_y"":5,
            ""needAmmo"":true,
            ""maxAmmo"":648,
            ""BasicAmmoNum"":0,
            ""AmmoTime"":0,
            ""gunName"":""SpecialPistol""
        }
    ],
    ""characters"":
    [
        {
            ""maxHp"":1000,
            ""maxMp"":40,
            ""moveSpeed"":8,
            ""jumpForce"":12.3,
            ""reloadSpeed"":0.075
        },
        {
            ""maxHp"":900,
            ""maxMp"":50,
            ""moveSpeed"":8.3,
            ""jumpForce"":12.8,
            ""reloadSpeed"":0.2
        },
        {
            ""maxHp"":1200,
            ""maxMp"":31,
            ""moveSpeed"":7,
            ""jumpForce"":12.3,
            ""reloadSpeed"":0.025
        },
        {
            ""maxHp"":1050,
            ""maxMp"":39,
            ""moveSpeed"":7.8,
            ""jumpForce"":12.5,
            ""reloadSpeed"":0.1
        }
    ]
}";

        return json;

    }

}
