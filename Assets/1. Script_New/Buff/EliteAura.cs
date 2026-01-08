using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteAura : Buff
{
    [HideInInspector] public float buffValue;
    float pre_buffValue = 0;

    protected override void Init()
    {
        buffIcon_Index = 7;
        buff_Time = float.MaxValue;
    }

    private void Update()
    {
        base.Update();

        float curBuffValue = buffValue;

        if(curBuffValue != pre_buffValue)
        {
            PlusValues(curBuffValue);
            pre_buffValue = curBuffValue;
        }
    }

    protected override bool PreventStack()
    {
        if (GetComponents<EliteAura>().Length > 1)
        {
            Destroy(this);
            return true;
        }
        return false;
    }

    void PlusValues(float value)
    {
        //기존 버프량만큼 감소(버프 초기화)
        unit.unitStatData_st.max_Hp_Plus -= unit.Max_Hp * pre_buffValue * 0.01f;
        unit.unitStatData_st.attack_PlusPercent -= pre_buffValue;
        unit.unitStatData_st.accuracy_Plus -= (int)(unit.ud.accuracy * pre_buffValue * 0.01f);
        unit.unitStatData_st.avoidance_Plus -= (int)(unit.ud.avoidance * pre_buffValue * 0.01f);

        //새로운 버프량만큼 증가
        unit.unitStatData_st.max_Hp_Plus += unit.Max_Hp * value * 0.01f;
        unit.unitStatData_st.attack_PlusPercent += value;
        unit.unitStatData_st.accuracy_Plus += (int)(unit.ud.accuracy * value * 0.01f);
        unit.unitStatData_st.avoidance_Plus += (int)(unit.ud.avoidance * value * 0.01f);
    }

    protected override void BuffStart()
    {
        base.BuffStart();
    }

    public override void BuffEnd()
    {
        base.BuffEnd();
    }
}
