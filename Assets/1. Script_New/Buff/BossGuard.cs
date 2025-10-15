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
            Debug.Log("bossGuard 중복 오류");
            return true;
        }
        return false;
    }

    protected override void BuffStart()
    {

    }

    public override void BuffEnd()
    {

    }

    private void OnDisable()
    {
        EnemySpawnManager.instance.isBossDead = true;
    }
}
