using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicWarrior_New : Unit
{
    public float buff_Time;
    public int armor_Decrease_percent;

    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        CuttedArmor cuttedArmor = target_Unit.AddComponent<CuttedArmor>();
        cuttedArmor.buff_Time = buff_Time;
        cuttedArmor.armor_Decrease_percent = armor_Decrease_percent;

        base.ApplyAttack(target_Unit, damage, attackType);
    }
}
