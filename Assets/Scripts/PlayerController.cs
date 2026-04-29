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

    protected override float GetHorizontalInput()
    {
        float input = 0;
        if (GetLeftInput() && !GetRightInput()) input = -1;
        if (!GetLeftInput() && GetRightInput()) input = 1;
        return input;
    }
    protected override bool GetLeftInput() => Input.GetKey(leftKey);
    protected override bool GetRightInput() => Input.GetKey(rightKey);
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
