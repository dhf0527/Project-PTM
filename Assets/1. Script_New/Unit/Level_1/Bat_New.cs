using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat_New : Unit
{
    protected override void ApplyAttack(Unit target_Unit, float attackDamage, AttackType attackType)
    {
        base.ApplyAttack(target_Unit, attackDamage, attackType);
        target_Unit.AddComponent<EarringDebuff>();
    }
}
