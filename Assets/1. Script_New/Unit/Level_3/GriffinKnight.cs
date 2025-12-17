using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GriffinKnight : Unit
{
    //데미지 증가량 반환 함수
    override public float CalculateAttackBoost(Unit target_Unit)
    {
        float dmgBoost = base.CalculateAttackBoost(target_Unit);

        //그리폰 발톱(중,대형 추가 데미지)
        if (target_Unit.size == Unit_Size.Medium || target_Unit.size == Unit_Size.Large)
            dmgBoost += 0.5f;

        return dmgBoost;
    }
}
