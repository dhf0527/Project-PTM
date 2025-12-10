using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBuff : Buff
{
    protected override void Init()
    {
        buffIcon_Index = 7;
        buff_Time = float.MaxValue;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<HardBuff>().Length > 1)
        {
            Destroy(this);
            return true;
        }
        return false;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
    }

    public override void BuffEnd()
    {
        base.BuffEnd();
    }
}
