using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_New : Unit
{
    public float attack_Increase;
    public int accuracy_Increase;
    public float buff_Time;
    public float healAmount = 10f;

    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        //체력 비율이 30% 이하라면 즉시 처치
        if (target_Unit.Cur_Hp / target_Unit.Max_Hp <= 0.3f && !target_Unit.GetComponent<TeamBase_Unit>() && !target_Unit.GetComponent<EnemyBase_Unit>())
        {
            target_Unit.TakeDamage(target_Unit.Cur_Hp);
        }
        else
            base.ApplyAttack(target_Unit, damage, attackType);

        //처치 시 '수확된 영혼'버프 획득
        if (target_Unit.Cur_Hp <= 0)
        {
            GetHp(healAmount);
            ReapedSpirit reapedSpirit = gameObject.AddComponent<ReapedSpirit>();
            reapedSpirit.buff_Time = buff_Time;
            reapedSpirit.attack_Increase = attack_Increase;
            reapedSpirit.accuracy_Increase = accuracy_Increase;
        }
    }
}
