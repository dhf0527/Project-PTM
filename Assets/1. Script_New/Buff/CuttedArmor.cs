using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttedArmor : Buff
{
    //방어 감소 비율(%)
    float armor_Decrease_percent;

    int armor_Decrease;

    protected override void Init()
    {
        buff_Time = 4;
        buffIcon_Index = 2;
        armor_Decrease_percent = 50;
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
        armor_Decrease = (int)(armor_Decrease_percent * unit.unitData_st.armor * 0.01f);
        unit.unitData_st.armor -= armor_Decrease;

    }

    protected override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitData_st.armor += armor_Decrease;
    }
}
