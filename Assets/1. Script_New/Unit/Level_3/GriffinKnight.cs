using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GriffinKnight : Unit
{
    //데미지 증가량 반환 함수
    override protected float CalculateAttackBoost(Unit target_Unit)
    {
        float dmgBoost = 0;

        //유닛 업그레이드 효과(보스 추가 데미지)
        if (target_Unit.GetComponent<BossGuard>())
        {
            int upgradeLv = PlayerPrefs.GetInt(ReadOnlyData.unitUpgrade + 9);
            if (upgradeLv != 0)
                dmgBoost += DunGeonManager_New.instance.unitUpgradeDatas[9].upgradeValue[upgradeLv - 1];
        }

        //그리폰 발톱(중,대형 추가 데미지)
        if (target_Unit.size == Unit_Size.Medium || target_Unit.size == Unit_Size.Large)
            dmgBoost += 50;

        return dmgBoost;
    }
}
