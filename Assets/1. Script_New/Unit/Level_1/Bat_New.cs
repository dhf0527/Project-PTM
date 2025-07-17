using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat_New : Unit
{
    public float buff_Time;

    public override void ApplyAttack(Unit target_Unit, float damage, AttackType attackType)
    {
        base.ApplyAttack(target_Unit, damage, attackType);
        target_Unit.AddComponent<EarringDebuff>().buff_Time = buff_Time;
    }
}
