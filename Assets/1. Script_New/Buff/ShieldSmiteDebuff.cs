using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSmiteDebuff : Buff
{
    float move_Decrease;
    float attackSpeed_Decrease;

    protected override void Init()
    {
        buff_Time = 4f;
        buffIcon_Index = 0;
        move_Decrease = 80;
        attackSpeed_Decrease = 0.8f * unit.AttackSpeed;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        unit.unitStatData_st.moveSpeed_PlusPercent -= 80;
        unit.unitStatData_st.attackSpeed_Plus -= attackSpeed_Decrease;
    }

    protected override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitStatData_st.moveSpeed_PlusPercent += 80;
        unit.unitStatData_st.attackSpeed_Plus += attackSpeed_Decrease;
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
