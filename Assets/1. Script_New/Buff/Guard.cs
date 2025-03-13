using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Buff
{
    int armorIncrease;

    protected override void Init()
    {
        buff_Time = 0.1f;

        armorIncrease = 4;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<Guard>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<Guard>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

    protected override void BuffStart()
    {
        unit.unitData_st.armor += armorIncrease;
    }

    protected override void BuffEnd()
    {
        unit.unitData_st.armor -= armorIncrease;
    }
}
