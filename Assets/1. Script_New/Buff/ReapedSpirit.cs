using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReapedSpirit : Buff
{
    float attack_Increase;
    int accuracy_Increase;

    protected override void Init()
    {
        buff_Time = 6f;
        buffIcon_Index = 5;

        attack_Increase = 20;
        accuracy_Increase = 40;
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
        unit.unitData_st.attackDamage += attack_Increase;
        unit.unitData_st.accuracy += accuracy_Increase;
    }

    protected override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitData_st.attackDamage -= attack_Increase;
        unit.unitData_st.accuracy -= accuracy_Increase;
    }
}
