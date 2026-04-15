using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : BasePlayerController
{
    [Header("玩家1按键设置")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.K;
    public KeyCode shootKey = KeyCode.J;
    public KeyCode switchGunKey = KeyCode.R;
    public KeyCode testBulletKey = KeyCode.U;
    public KeyCode testSizeKey = KeyCode.T;
    public KeyCode downKey = KeyCode.S;

    public KeyCode reloadKey = KeyCode.Q;

    
    // ===== 弹药系统 =====
    protected override void Start()
    {
        base.Start();
        if (guns.Count > 0)
        {
            currentGunIndex = 0;
            currentGun = guns[currentGunIndex];
            // 初始化子弹数量为满弹匣
            currentGun.nowAmmo = currentGun.maxAmmo;
            Debug.Log($"初始化武器: {currentGun.gunName}, 子弹: {currentGun.nowAmmo}/{currentGun.maxAmmo}");
            for(int i = 0; i < guns.Count; i++)
            {
                guns[i].nowAmmo = guns[i].maxAmmo;
                Debug.Log($"初始化武器: {guns[i].gunName}, 子弹: {guns[i].nowAmmo}/{guns[i].maxAmmo}");
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override float GetHorizontalInput()
    {
        float input = 0;
        if (Input.GetKey(leftKey)) input = -1;
        if (Input.GetKey(rightKey)) input = 1;
        return input;
    }
    protected override bool GetJumpInput() => Input.GetKeyDown(jumpKey);
    protected override bool GetJumpHoldInput() => Input.GetKey(jumpKey);
    protected override bool GetDownInput() => Input.GetKey(downKey);
    protected override bool GetSprintInput() => Input.GetKey(sprintKey);
    protected override bool GetShootInput() => Input.GetKey(shootKey);
    protected override bool GetSwitchGunInput() => Input.GetKeyDown(switchGunKey);
    protected override bool GetTestBulletInput() => Input.GetKeyDown(testBulletKey);
    protected override bool GetReloadInput() => Input.GetKeyDown(reloadKey);
    protected override bool GetDownFinishInput() => Input.GetKeyUp(downKey);
    protected override bool GetTestSizeInput() => Input.GetKeyDown(testSizeKey);

}
