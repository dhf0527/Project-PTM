using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGuard : Buff
{
    private void Update()
    {
        base.Update();
    }

    protected override void Init()
    {
        buff_Time = float.MaxValue;
        buffIcon_Index = 6;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<BossGuard>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<BossGuard>().cur_BuffTime = 0;
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

    private void OnDestroy()
    {
        EnemySpawnManager.instance.isBossDead = true;
    }
}
