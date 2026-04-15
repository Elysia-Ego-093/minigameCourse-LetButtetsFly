using UnityEngine;

public class CoverBox : InteractiveStuff
{
    protected override void Awake()
    {
        isPickable = false;
        base.Awake();
    }

    protected override void BreakCover()
    {
        base.BreakCover();
        Destroy(gameObject,0.1f);
    }
}
