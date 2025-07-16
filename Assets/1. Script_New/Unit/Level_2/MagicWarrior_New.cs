using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicWarrior_New : Unit
{
    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        target_Unit.AddComponent<CuttedArmor>();

        base.ApplyAttack(target_Unit, damage, attackType);
    }
}
