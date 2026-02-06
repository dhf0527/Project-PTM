using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinded : Buff
{
    [HideInInspector]public int accuracy_Decrease;

    protected override void Init()
    {
        buffIcon_Index = 8;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        unit.unitStatData_st.accuracy_Plus -= accuracy_Decrease;
    }

    public override void BuffEnd()
    {
        base.BuffEnd();
        unit.unitStatData_st.accuracy_Plus += accuracy_Decrease;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<ShieldSmiteDebuff>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<Blinded>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

}
