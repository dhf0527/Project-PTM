using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReapedSpirit : Buff
{
    [HideInInspector]public float attack_Increase;
    [HideInInspector]public int accuracy_Increase;

    protected override void Init()
    {
        buffIcon_Index = 5;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<ReapedSpirit>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<ReapedSpirit>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        unit.unitStatData_st.attack_Plus += attack_Increase;
        unit.unitStatData_st.accuracy_Plus += accuracy_Increase;
    }

    public override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitStatData_st.attack_Plus -= attack_Increase;
        unit.unitStatData_st.accuracy_Plus -= accuracy_Increase;
    }
}
