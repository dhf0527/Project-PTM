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
        move_Decrease = 0.8f * unit.unitData_st.moveSpeed;
        attackSpeed_Decrease = 0.8f * unit.unitData_st.attackSpeed;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        unit.unitData_st.moveSpeed -= move_Decrease;
        unit.unitData_st.attackSpeed -= attackSpeed_Decrease;
    }

    protected override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitData_st.moveSpeed += move_Decrease;
        unit.unitData_st.attackSpeed += attackSpeed_Decrease;
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
