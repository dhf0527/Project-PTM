using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicWarrior_New : Unit
{
    public float buff_Time;

    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        target_Unit.AddComponent<CuttedArmor>().buff_Time = buff_Time;

        base.ApplyAttack(target_Unit, damage, attackType);
    }
}
