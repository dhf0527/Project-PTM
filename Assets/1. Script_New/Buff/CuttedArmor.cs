using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttedArmor : Buff
{
    //방어 감소 비율(%)
    [HideInInspector]public int armor_Decrease_percent;

    protected override void Init()
    {
        buffIcon_Index = 2;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<CuttedArmor>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<CuttedArmor>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        unit.unitStatData_st.armor_PlusPercent -= armor_Decrease_percent;

    }

    public override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitStatData_st.armor_PlusPercent += armor_Decrease_percent;
    }
}
