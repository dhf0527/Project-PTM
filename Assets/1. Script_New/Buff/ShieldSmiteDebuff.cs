using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSmiteDebuff : Buff
{
    [HideInInspector]public float move_Decrease;
    [HideInInspector]public float attackSpeed_Decrease;

    protected override void Init()
    {
        buffIcon_Index = 0;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        unit.unitStatData_st.moveSpeed_PlusPercent -= 80;
        unit.unitStatData_st.attackSpeed_PlusPercent -= attackSpeed_Decrease;
    }

    protected override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitStatData_st.moveSpeed_PlusPercent += 80;
        unit.unitStatData_st.attackSpeed_PlusPercent += attackSpeed_Decrease;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<ShieldSmiteDebuff>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<ShieldSmiteDebuff>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

}
