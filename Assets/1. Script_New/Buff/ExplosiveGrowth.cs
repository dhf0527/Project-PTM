using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGrowth : Buff
{
    [HideInInspector] public List<int> buffValues;
    float pre_buffValue;
    Unit babyDragonUnit;

    private void Update()
    {
        base.Update();

        int buffValue = buffValues[EnemySpawnManager.instance.cur_Wave];
        PlusAttack(buffValue);
    }

    protected override void Init()
    {
        buffIcon_Index = -1;
        babyDragonUnit = GetComponent<Unit>();
    }

    protected override bool PreventStack()
    {
        if (GetComponents<ExplosiveGrowth>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<ExplosiveGrowth>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

    void PlusAttack(int value)
    {
        if (pre_buffValue == value)
            return;

        //기존 버프량만큼 감소(버프 초기화)
        babyDragonUnit.unitStatData_st.attack_Plus -= pre_buffValue;

        //새로운 버프량만큼 증가
        babyDragonUnit.unitStatData_st.attack_Plus += value;
        //버프량 기록
        pre_buffValue = value;
    }
}
