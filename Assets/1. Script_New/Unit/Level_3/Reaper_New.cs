using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_New : Unit
{
    float healAmount = 10f;

    protected override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        //체력 비율이 30% 이하라면 즉시 처치
        if (target_Unit.Cur_Hp / target_Unit.unitData_st.max_Hp <= 0.3f && !target_Unit.GetComponent<TeamBase_Unit>() && !target_Unit.GetComponent<EnemyBase_Unit>())
        {
            target_Unit.TakeDamage(target_Unit.Cur_Hp);
        }
        else
            base.ApplyAttack(target_Unit, damage, attackType);

        //처치 시 '수확된 영혼'버프 획득
        if (target_Unit.Cur_Hp <= 0)
        {
            GetHp(healAmount);
            gameObject.AddComponent<ReapedSpirit>();
        }
    }
}
