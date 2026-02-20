using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenHeroSword : Buff
{
    //명중률 증가량
    [HideInInspector] public int accuracy_increase;

    protected override void Init()
    {
        buff_Time = 6f;
        buffIcon_Index = 1;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        unit.unitStatData_st.accuracy_Plus += accuracy_increase;
        unit.unitStatData_st.isPenetration = true;
    }

    public override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitStatData_st.accuracy_Plus -= accuracy_increase;
        unit.unitStatData_st.isPenetration = false;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<BrokenHeroSword>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<BrokenHeroSword>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }
}
