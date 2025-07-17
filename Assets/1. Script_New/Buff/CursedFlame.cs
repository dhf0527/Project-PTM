using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedFlame : Buff
{
    //피해를 받는 주기
    float damage_Delay;
    //주기당 피해
    float damage;
    //회복 감소량
    float heal_Decrease;

    protected override void Init()
    {
        buffIcon_Index = 4;

        damage_Delay = 0.5f;
        damage = 2;
        heal_Decrease = 50f;
    }

    protected override bool PreventStack()
    {
        if (GetComponents<CursedFlame>().Length > 1)
        {
            //지속 시간 초기화
            GetComponent<CursedFlame>().cur_BuffTime = 0;
            Destroy(this);
            return true;
        }
        return false;
    }

    protected override void BuffStart()
    {
        base.BuffStart();
        InvokeRepeating("TickDamage", damage_Delay, damage_Delay);
        //회복 감소
    }

    protected override void BuffEnd()
    {
        base.BuffEnd();
        CancelInvoke();
        //회복 감소 해제
    }

    void TickDamage()
    {
        unit.Cur_Hp -= damage;
    }
}
