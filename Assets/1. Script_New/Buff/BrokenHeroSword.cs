using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenHeroSword : Buff
{
    //명중률 증가량
    [SerializeField] float accuracy_increase = 40;

    protected override void Init()
    {
        buff_Time = 6f;
        buffIcon_Index = 0;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        unit.unitData_st.accuracy += accuracy_increase;
        Debug.Log("Start " + unit.unitData_st.accuracy);
    }

    protected override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitData_st.accuracy -= accuracy_increase;
        Debug.Log("End " + unit.unitData_st.accuracy);
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
