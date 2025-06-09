using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Reborn : Buff
{
    float moveSpeed_Increase_Percent;
    float attackSpeed_Increase_Percent;
    float hp_Decrease_Per_Sec;

    float moveSpeed_Increase;
    float attackSpeed_Increase;

    protected override void Init()
    {
        buff_Time = 9999;
        buffIcon_Index = 3;

        moveSpeed_Increase_Percent = 50;
        attackSpeed_Increase_Percent = 50;
        hp_Decrease_Per_Sec = 10;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<Reborn>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<Reborn>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        //이동속도 증가
        unit.unitStatData_st.moveSpeed_Square *= (1 + moveSpeed_Increase * 0.01f);
        //공격속도 증가
        attackSpeed_Increase = unit.ud.attack_Speed * attackSpeed_Increase_Percent * 0.01f;
        unit.unitStatData_st.attackSpeed_Plus += attackSpeed_Increase;
        //체력 지속적으로 감소
        InvokeRepeating("HpDecrease", 1, 1);
    }

    protected override void BuffEnd()
    {
        base.BuffEnd();
        //이동속도 복구
        unit.unitStatData_st.moveSpeed_Square *= (1 - moveSpeed_Increase * 0.01f);
        //공격속도 복구
        unit.unitStatData_st.attackSpeed_Plus -= attackSpeed_Increase;
        //체력 감소 삭제
        CancelInvoke();
    }

    void HpDecrease()
    {
        unit.Cur_Hp -= hp_Decrease_Per_Sec * unit.Max_Hp * 0.01f;
    }
}
