using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarringDebuff : Buff
{
    int accuracy_Decrease = 20;
    int avoidance_Decrease = 20;

    protected override void Init()
    {
        buffIcon_Index = 6;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<EarringDebuff>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<EarringDebuff>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        unit.unitStatData_st.accuracy_Plus -= accuracy_Decrease;
        unit.unitStatData_st.avoidance_Plus -= avoidance_Decrease;
    }

    protected override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitStatData_st.accuracy_Plus += accuracy_Decrease;
        unit.unitStatData_st.avoidance_Plus += avoidance_Decrease;
    }
}
