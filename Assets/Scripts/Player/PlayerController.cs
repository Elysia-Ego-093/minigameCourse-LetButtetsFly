using UnityEngine;

public class PlayerController : BasePlayerController
{
    [Header("玩家按键设置")]
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;
    public KeyCode sprintKey;
    public KeyCode shootKey;
    public KeyCode switchGunKey;
    public KeyCode downKey;
    public KeyCode reloadKey;
    public KeyCode grenadeKey;
    public KeyCode dropGunKey;

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
    protected override bool GetReloadInput() => Input.GetKeyDown(reloadKey);
    protected override bool GetDownFinishInput() => Input.GetKeyUp(downKey);
    protected override bool GetThrowGrenadeInput() => Input.GetKey(grenadeKey);
    protected override bool GetDropGunInput() => Input.GetKeyDown(dropGunKey);

}
